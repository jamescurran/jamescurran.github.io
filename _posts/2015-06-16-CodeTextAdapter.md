---
layout: post
title: A C# (Over-)Engineering Project - CodeTextAdapter
categories: code C# csharp programming ASP.NET aspnet MVC javascriptloader
tags: code C# csharp programming
---

Frequently, I need to deal with tables that map a code to some text.  I prime example would a list mapping state abbreviations to the full state name (e.g. `"NJ"` -> `"New Jersey"`).   Or perhaps some status code to it's meaning (`"A"` -> `Approved`).

In for those, we'd probably write a collection of methods to deal with them, which we'd need a common interface for them, something like this:

<script src="https://gist.github.com/jamescurran/3b2e9ea5a0daae303bcc.js">    </script> 

Then we'd be able to write a method that we could use on any of them, something like this:

<script src="https://gist.github.com/jamescurran/e530d0e24ee187cb4293.js">     </script>

And everyone's happy --- _Except_ if we have a class holding a State name and it's abbreviation, then we probably don't want those fields called `Code` & `Text`.  We'd probably prefer calling them something like `Name` & `Abbrev`.  But if we change the property names, then it no longer matches the interface, and nothing works.

Which I why I wrote `CodeTextAdapter` a helper base class which solves this problem.   One uses it like this:

<script src="https://gist.github.com/jamescurran/768f244fb896c1d9b530.js">  </script>

The key parts are the base class, which takes, as it's generic type parameter, the class that's being defined (A technique known is some quarters as the [_Curiously recurring template pattern_](https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern) ).   And the constructor,  where the `base()` constructor call take two lambdas, which respectively, retrieve that Code and Text values.

If you want to create a specialized constructor for your class, just make sure you include a `this()` call in it, as shown here.  (If you forget, it won't compile.)

<script src="https://gist.github.com/jamescurran/001fe2bebdefdd1e3458.js">  </script>

Here's the code for the adapter itself:

<script src="https://gist.github.com/jamescurran/56778cccdb883945140b.js"> </script>