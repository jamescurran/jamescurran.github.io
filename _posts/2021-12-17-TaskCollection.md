---
layout: post
title: TaskCollection - Making your async code parallel
tags: code programming csharp dotnet
---

When I wrote an article for the [2021 C# Advent Calendar](https://www.csadvent.christmas/) on Tuesday, I promised to post a couple days in a row.  

That didn't work out.

But at least I eventually got to it.

-----

The `async\await` keyword added to C# a few years ago greatly simplified use asynchronous code in our applications.  However, asynchronous code is inheriently tricky, and in trying to make it simple, `async/await` sometimes undermines itself.

Consider the code:

    _cacheCustomers = await LoadTableAsync("Customers");
    _cacheProducts = await LoadTableAsync("Products");
    _cacheOrders = await LoadTableAsync("Orders");

Fairly typical code to preload three tables from a database that we are going to use.  Three separate round-trips to the data server with much waiting for the response from that server. So the perfect situation for those actions to be run in parallel, which is why we used the `...Async` version of the method, and used `await`.

> Note that `LoadTableAsync` is just generic example representative of long-running non-CPU bound method. Could be a database, could be a Web API, etc.

But they *aren't* going to be run in parallel, because `await` means, well, await.  Wait for the first call to `LoadTableAsync` to complete before we start the second call.  Now, granted, *something* will be run at the same time as each of those calls, but since you probably have `await`s going all the way up the call chain back to the Main() function, it's probably going to be the operating system.  But, importantly, it won't be the other calls to `LoadTableAsync`.

What we need to do is start them all first, and then wait for them to finish.  Something like:

    var taskCustomers = LoadTableAsync("Customers");
    var taskProducts = LoadTableAsync("Products");
    var taskOrders = LoadTableAsync("Orders");

And then, at some point later, 

    _cacheCustomers = await taskCustomers;
    _cacheProducts = await taskProducts;
    _cacheOrders = await taskOrders;

or alternately,

    Task.WaitAll(taskCustomers, taskProducts, taskOrders);
    _cacheCustomers = taskCustomers.Result;
    _cacheProducts = taskProducts.Result; 
    _cacheOrders = taskOrders.Result;


The advantage of this is that between the LoadTableAsync()s, and the waits, you can put some cpu-bound actions, so that when you get to the waits, your data is already waiting for you.  The database calls become *free*!

Now, this is all well & good, but we've added a bunch of lines of code, and that gets messy, particularly if the return value of the method isn't being used.  Consider:

    await FillListFromTableAsync("Customers", _cacheCustomers);
    await FillListFromTableAsync("Products", _cacheProducts);
    await FillListFromTableAsync("Orders", _cacheOrders);

To make these parallel, now we have to create the local task variables, just to use the the Task.WaitAll. And if you have more than three, it really starts to get out of hand.

Which brings us to `TaskCollection`, a simple class to manage that for you.

	var tasks = new TaskCollection();
    tasks += FillListFromTableAsync("Customers", _cacheCustomers);
    tasks += FillListFromTableAsync("Products", _cacheProducts);
    tasks += FillListFromTableAsync("Orders", _cacheOrders);

and then,

	_logger.LogInformation($"{tasks.Count} tasks queued, waiting on {tasks.Running}");
	tasks.WaitAll();

It includes the properties:
     **Count** - total number of tasks in the collection (running and completed)
     **Running** - number of tasks still running.

And the methods:
    **RemoveCompleted** - Removes completed tasks from internal list.  Afterward, Count == Running.
    **WhenAll** - Returns a task which completes when all tasks on the list has completed.

Full code given here:

<script src="https://gist.github.com/jamescurran/46d90e84646c3c308cdea0d664addd1d.js"> </script>


