---
layout: post
title: C# Code:Adding Skip First to Foreach
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---

A couple years back, I made a [proposal online](http://groups.google.com/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/412fd19c65ea81de) for a new feature in C#.  It gathered some interest in the newsgroup.  I later emailed it to someone on the C# team at Microsoft (I believe it was [Eric Gunnerson](http://blogs.msdn.com/ericgu/default.aspx) but I'd really have to look it up), who emailed back a very nice response saying basically, "We thought about something like this, but decided again it, because it can be done with the Iterators we're adding to C# v2.0".

So, I figured, three years later, perhaps it's time I actually did implement them using iterator.

The idea was to allow developers to use foreach to iterator over a collection, in the case where the first or last item in the collection needed to be handled differently.   My first crack at it works like this:

	int[] ary = new int[ 6 ] { 1, 2, 3, 4, 5, 6 };
	foreach (int a in new SkipFirst<int>(ary))
	{
		Console.Write(a);  // Prints 23456
	}

The code for it looks like this:
<script src="https://gist.github.com/jamescurran/5471694.js">    </script>

On the whole, quite simple minded.  The while() loop is the essence of a standard IEnumerator.  I basically just added a MoveNext() first.   Handling SkipLast is a little bit trickier:


<script src="https://gist.github.com/jamescurran/5472285.js">    </script>
    
Here's we must keep of copy of the Current item, and return the copy only after we've successfully done the MoveNext(). If you can't MoveNext, then the copy we've saved in the Last that we want to skip.

However, the problem with these is that they have a rather ugly syntax : You have to include "new" and specify the type. But, there's a way out:  If we can create a function which returns on object of our IEnumerator class, we can have the compiler figure out the types:

	static public class Skip
	{
		public class SkipFirst<T> : IEnumerable<T>
		{  /* As above */  }
		public class SkipLast<T> : IEnumerable<T>
		{  /* As above */  }

		static public SkipFirst<T> First<T>(IEnumerable<T> enm)
		{
			return new SkipFirst<T>(enm);
		}
		static public SkipLast<T> Last<T>(IEnumerable<T> enm)
		{
			return new SkipLast<T>(enm);
		}
	}

Now all we have to write is this: 

	int[] ary = new int[ 6 ] { 1, 2, 3, 4, 5, 6 };
	foreach (int a in Skip.First(Skip.Last(ary)))
	{
		Console.Write(a); // writes "2345"
	}

Somewhat enhanced source code is available <a href="/files/Skip.cs">here</a>.

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/05/c-code-adding-skip-first-to-foreach.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/05/c-code-adding-skip-first-to-foreach.aspx" border="0" /></a>