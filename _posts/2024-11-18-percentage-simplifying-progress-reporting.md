---
layout: post
title: Percentage --- Simplifying Progress Reporting
tags: code programming csharp
---

## Percentage --- Simplifying Progress Reporting

A few years, I wrote a post about a simple class a wrote called `Every` ([Every -- Doing Something Occasionally](https://honestillusion.com/blog/2017/10/12/every-doing-something-occasionally/)). It's purpose was to ease handling, if you had, say, 50,000 records to process, and you wanted some notification after every 1000 were complete. 

That's all well and good, but sometimes, rather than knowing some fixed number of records is complete, you prefer knowing what percentage is complete.

I came upon that need recently at work, so I created this class, which builds on `Every`.

Use is simple:

       var progress = new Percentage(numRecords, 
            (count, percentage) => Console.WriteLine($"We're at {percentage}% done.");
	   foreach(var item in MyLongList)
	   {
	          Process(item);
	          ++progress;
	   }


It's rather straightforward, much like it predecessor, except for one tricky part, which causes it to have two basic modes.

If the total number of items being processed is greater than 100, then the provided `Action<int, double>` will be called 100 times, once every time another percent of the total is complete.  The two parameters are the number of items processed, and the percentage complete, which will increase by one each time.

If the total is less than 100, then then action is called every time the `operator++` is called, in this case, with the percentage increased appropriately.  For example, if you only have 17 steps, then the percentage will step about 6 each time.

Of course, that is handled internally, so you don't have to worry about it. 

Source code is available here: [https://github.com/jamescurran/Every](https://github.com/jamescurran/Every)

