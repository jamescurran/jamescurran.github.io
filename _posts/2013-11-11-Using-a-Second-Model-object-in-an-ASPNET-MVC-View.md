---
layout: post
title: Using a Second Model object in an ASP.NET MVC View
categories: code c# .net programming dotnet csharp aspnet mvc aspnetmvc
tags: code c# .net programming dotnet csharp aspnet mvc aspnetmvc
---

The "correct" way to pass information to an ASP.NET MVC view page is by way of the "model", which is returned by the controller (as a System.Object) and is given a type within the view by the Razor `@model` directive.

And this is just fine, if the entirety of what needs to be displayed is logically one object, but breaks down if you need some other information on the page.  

Say for example, you are displaying a personnel record.  It would be logical from your model to be a `Employee` object.  But, perhaps, instead of just displaying that person&apos;s department, you want to include a drop-down listbox, containing all the department names, so you could re-deploy him.  The list of departments wouldn&apos;t be part of the `Employee` object, so we&apos;d need a different way to get it to the page. 

Basically, there are two way : The quick and easy and "bad" way (ViewBag), and the difficult and "correct" (a ViewModel object).

The "correct" way would be to create a new class, say DisplayEmployeeViewModel, which has two properties, `Employee` and `Departments`, and you return that from the controller as your model. 

But, that&apos;s really just as sloppy as the "bad" way.  We&apos;re creating a object holding two things which have not logical connection to each other, and cluttering up our source code with a class which will be used exactly once.   Further, in our view, where we&apos;d like to say `@Model.Name`, we now have to write `@Model.Employee.Name`.  In all, it adds a lot of effort &amp; complexity, just so we can say we did it the "right" way.

But, what about the "Bad" way -- just stuffing the other data into the `ViewBag`.  It&apos;s quick and easy, but everyone knows it&apos;s "wrong" because data in the ViewBag is **untyped** (!!).  

However, a quick peek at what&apos;s going on under the covers reveals that&apos;s not really the case, and provides a simple solution to our problem.

You see, when you return an object from a controller as the model, all the ASP.NET MVC does is put it into the ViewBag as a property named `model`.   And when you add the directive:

	@model Employee

it treats it as if you had written

	@{ var Model = ViewBag.model as Employee; }

Now, going back to our example, if we were to just use the `ViewBag` in our controller, for the list of departments, as well as use the return value to set the employee record as the model:

		// :
		ViewBag.departments = GetListOfDepartments();
		return View(employee);
		}

then in our view, we use a normal `@model` directive plus duplicating the effect for the other object:

	@model  Employee
	@{
			var DepartmentModel = ViewBag.departments as List<Department>;
	}
       
Now, we can use `Model` just for the Employee object like we always wanted, and we can use `DepartmentModel` -- which is just as "strongly-typed" as Model itself -- as if it were the sole Model on the page.


