---
layout: post
title: Using a Second Model object in an ASP.NET MVC View
categories: code c# .net programming dotnet csharp aspnet mvc aspnetmvc
tags: code c# .net programming dotnet csharp aspnet mvc aspnetmvc
---

The "correct" way to pass information to an ASP.NET MVC view page is by way of the "model", which is returned by the controller (as a System.Object) and is given a type within the view by the Razor `@model` directive.

And this is just fine, if the entirety of what needs to be displayed is logically one object, but breaks down if you need some other information on the page.  

Say for example, you are displaying a personnel record.  It would be logical from your model to be a `Employee` object.  But, perhaps, instead of just displaying that person&apos;s department, you want to include a drop-down listbox, containing all the department names, so you could re-deploy him.  The list of departments wouldn&apos;t be part of the `Employee` object, so we&apos;d need a different way to get it to the page. 
