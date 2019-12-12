---
layout: post
title: Cleanup Scaffolded Code with modelbuild
tags: site
---

This post is for day 12  of the [2019 C# Advent Calendar](https://crosscuttingconcerns.com/The-Third-Annual-csharp-Advent) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

Now, this isn't specifically about C#; it's more directly about .NET Core and Visual Studio tooling, but we use C# in it, so it counts....

 EntityFramework would really like you to use the "code first" approach to defining your database.  In fact, EntityFramework.Core seems to have removed support for the "Database First"  method.  Unfortunately, in my experience, in at least 90% of projects, the database already exists, and we have to work with that, and reverse engineer Code First classes from our database.

Fortunately, .NET Core offers us the `Scaffold-DbContext` command of the EF Core Package Manager Console to do that reverse engineering for us.   It will generate a DbContext-derived class for the database, plus one file (with one class) for each table, defining the fields. But this article is not about that, you can read more about it [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding).


Unfortunately, the code for all the other details about each table --- Keys, indexes, relationships --  are put entirely in the OnModelCreating() method of the DbContext class.  

<script src="https://gist.github.com/jamescurran/d70cc694d379878542d7cb998b0645b2.js"> </script>
   
There are two problems with this: First, any reasonable database will probably have a couple dozen tables, meaning that method will be several hundred lines long --- I never want to have a method that long. More importantly, the properties and the other data are both equal parts of the schema, and they should be recorded in the same place (or at least the same file).

So I devised a plan to do just that.  For each  table/class, I'd create a secondary static class holding just a single extension method.   I'd move the schema defining code for that table into the extension method, and then call  it from the dbcontext's OnCreate.  That leaves the OnCreate with essentially just a list of the tables in the database.

<script src="https://gist.github.com/jamescurran/20f6744169bffa6586c8fc86792d10be.js"> </script>

<script src="https://gist.github.com/jamescurran/c07052a6279f41c1c4996ddc228d509b.js"></script>
	
However, writing the boilerplate code for this got rather tedious, especially since the rest of the work was mostly just cut'n'paste.  So I figured I'd create a code snippet to provide the framework.

<script src="https://gist.github.com/jamescurran/9177395e9a4d7e89a230b101612fca9a.js"> </script>
	
Do use, crate a file, `modelbuild.snippet`, out of the gist, then install it as a code snippet: Tools/Code Snippet Manager.../Add.

Then, in a source file for one of the classes for a table, just before the final closing brace (closing the namespace), type `modelbuild`, *[tab]* *tablename* *[enter]*, and Visual Studio will fill in the boring part, then you just have to cut'n'paste from the DbContext class to this class, and replace the code with `modelBuilder.Configure`*tablename*`()`

