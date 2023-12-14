---
layout: post
title: All Purpose Object Updater  : don't publish
tags: code programming dotnet advent
---


This post is for day 15  of the [2023 C# Advent Calendar](https://www.csadvent.christmas/) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

In past years, when I've written for the Advent Calender (this is my sixth year participating), I've noted how many weeks, months or ...um... years, since my last blog post. *But not this year!* I actually did **SIX** last month! In fact, I'm in the midst of a series where I refactor the classic game *Oregon Trail* into modern C#. [Check it out!](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)

So, now back to the matter at hand, a one-off bit coding for those just popping up for the holidays.
 
In my day job, we had a need to test a object for some condition, and if true, make some change to the object.  For example, if in an order record, a credit card number is given, then the `PayementType` property must be set to `CreditCard`.  The trick is that both the condition (there are many of them) and the changes to be made are stored in a database as JSON.

Testing for the condition is handled by the [MicroRulesEngine](https://github.com/runxc1/MicroRuleEngine), an open source project I've contributed heavily to.

The method to handle updating the object had to be home-grown, and now is time to share it with the world.

Perhaps you are familiar with the "non-destructive mutation" syntax added in C# v 9:

    var me = new Person { FirstName = "James", LastName ="Curran" };
    var sister = me with { FirstName = "Jean" };

This is conceptional similar, except it's a ***destructive,*** *in-place* mutation, which actually means it's quite different.

    var me = new Person { FirstName = "James", LastName ="Curran" };
    me.Update( { FirstName = "Jean" });

<script src="https://gist.github.com/jamescurran/c5e4887a3a688e397528fd8a8d581e1b.js"> </script>

The process is quite simple: Iterate over the properties in the `changes` object, look up the same-named property in the target object, and update it, with some sanity checks along the way.

Now that's fine if you have an object with the changes (or can deserialize a bit of JSON), but sometimes it's easier to have the changes in a `Dictionary<string,object>`:

<script src="https://gist.github.com/jamescurran/2579061be5b5e74257aae8eb5068f26e.js"> </script>

This runs basically the same way, except now we are iterating over the items in the dictionary.  It has a couple special features: You can use the word "null" instead of a `null` value to set the property to `null`. (`null` itself also works)  And you can use "Y" or "N" (or "Yes" or "no") to set a boolean to `true` or `false`.

Which brings us to another special case we had.  Sometimes there was only one property to change, but it had to be set to one of two values depending on condition.  Again, the changes were stored as txt in the database, so we settles on the format: "{propertyname}={truevalue}|{falsevalue},  e.g., `Mode=3|5`.

<script src="https://gist.github.com/jamescurran/8ed9ad531d799e17091bd30f19cb9d83.js"> </script>