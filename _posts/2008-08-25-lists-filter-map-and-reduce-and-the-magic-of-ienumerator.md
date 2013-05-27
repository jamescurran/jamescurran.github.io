---
layout: post
title: Lists:Filter, Map and Reduce - and the Magic of IEnumerator.
categories: 
tags: 
---

I have this bad habit.  I will frequently stumble upon a blog post describing some new technique, to which I will post a brilliant comment offering an improvement, which, of course, will get lost in the flotsam and jetsam of the blogosphere.  I have to keep reminding myself that is what I have my own blog for.

Case in point, I recent found this article by Sarah Taraporewalla about writing tradition [Filter, Map and Reduce methods](http://sarahtaraporewalla.blogspot.com/2008/08/lists-filter-map-and-reduce.html) for Java Lists.  She wondered if they could be written in C#.  I did so in the comments, and now expanded on them here. 
  
The main difference between mine and those of Sarah's (and also those of [Peter](http://dotnet.org.za/pieter/archive/2008/08/17/filter-and-map-in-c.aspx), by way of whose blog I reached Sarah's) is that they pass in a List&lt;&gt; object, and create a new List to return.  This is limiting and unnecessary.

	namespace FilterMapReduce
	{
		static public class FMR
		{
			public static IEnumerable<T> Filter<T>(this IEnumerable<T> list, Func<T, bool> filter)
			{
				foreach (T item in list)
				{
					if (filter(item))
						yield return item;
				}
			}

			public static IEnumerable<T> Map<T>(this IEnumerable<T> list, Func<T, T> map)
			{
				foreach (T item in list)
				{
					yield return map(item);
				}
			}

			public static U Reduce<T, U>(this IEnumerable<T> list, Func<T, U, U> reduce, U accum)
			{
				foreach (T item in list)
				{
					accum = reduce(item, accum);
				}
				return accum;
			}
		}
	}

I used a number of .Net v3.5 features there -- notably extension methods and the Func&lt;&gt; delegate, but neither is vital (the non-extension version has just a little messier calling syntax, and you'd have to define your own Func delegate replacement -- so, UPGRADE!)

With those, we can now write code like this:

	int \[\] nums = new int\[\] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	int sumOdds10 = nums.Filter(n => (n % 2) == 1)
						.Map(n => n * 10)
						.Reduce((n, a) => (n + a), 0);

I broke the last line to three lines for easier reading, but you could just string it out of you'd like.

Now that says, "Take the nums array, filter it so we're left with just the odd numbers, map each remaining value to itself times 10, and then sum each of those."

The important question here is: "How many times have I looped through that array "  You might think that since I call all three methods, and each has a foreach loop, that it would be only logical that we've go through the array three time.  Logical, maybe, but wrong.   To understand this, let's split that code up a bit:

	var a = nums.Filter(n => (n % 2) == 1);
	var b = a.Map(n => n * 10);
	int sumOdds10 = b.Reduce((n, a) => (n + a), 0);

First we create object a, which if you recall is IEnumerator&lt;int&gt; object. We haven't looped through the array yet -- we just have an object that <em>will</em> loop throught the array.

Next, we create object b.  Again, an IEnumerable&lt;int&gt; object, but notably, one which enumerates over, not our array, but the a object.

Finally, we call Reduce which actually does the work.  It starts to iterate over the list, which is our b object, which is just an IEnumerable object.  So, as we enter Reduce's foreach, it calls **b**'s (i.e. Map's) MoveNext() method, and enters it's foreach to iterate over the **a** object -- for which is calls a's (Filter's) MoveNext method. 

OK, so now we finally enter FIlter's foreach loop, which iterates over an array, so we get a real number (1 at first). It passes the filter so the yield return sends it back to Map, which call the mapping function on it, and yields it up Reduce, which uses it in the reduce function.

Then Reduce moves on it the next value, which means calling B's MoveNext, which calls Filter's MoveNext, which gets the next value from the array (2).  This fails the filter, so it gets the next value (3), which passes and goes back to Map, and so on, to Reduce.

In the end, we've only gone through the array once.

<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2008%2f08%2f25%2flists-filter-map-and-reduce-and-the-magic-of-ienumerator.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2008%2f08%2f25%2flists-filter-map-and-reduce-and-the-magic-of-ienumerator.aspx" border="0" /></a>