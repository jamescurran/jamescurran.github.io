---
layout: post
title: Inside a Where() - Understanding IEnumerables
tags: code programming csharp adventcalendar
---

## Inside a Where() - Understanding IEnumerables

This post is for day 5  of the [2024 C# Advent Calendar](https://www.csadvent.christmas/) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate! (I'm filling in for someone who dropped out at the last minute.)

`IEnumerable<T>` is one of the most basic elements of the .NET framework (and really Computer Engineering itself), so I'm often surprised when I see some commentator giving a rather confused description of it.

I found this especially true when IEnumerables are chained together, say in 

     var newList = 
         myCollection
             .Where(x => x > 5)
             .Where(x => x % 3 == 0)
             .ToList();

(Yes, that's silly, but I kept it simple). I've heard people claim that the first `Where()` is building a whole new list, which it passes to the second `Where()`, which passes it on the the `ToList()`, which apparently they think builds a new list out of the list....

That's not at all what's happening.

At it's most basic level, an `IEnumerable` is simple a object generator, which, when asked, will give you the next item in a sequence.  How exactly "the next item" is defined depends on the `IEnumerable`.

The important thing to remember, is that it is *not* limited to getting the next item from a collection. The basic case of this is the `Enumerable.Range()` method, which is essentially this (simplified for the example):

     IEnumerable<int> Range(int start, int count)
     {
        while(count-- > 0)
            yield return start++;
     }

No collection behind it.  Just "the next item", which in this case is the next higher number.

Now, with the case of `Where()`, and other LINQ methods like `Select`, which take a `IEnumerable` as a parameter, and returns an `IEnumerable`, the method really just creates a new IEnumerable object which wraps the given `IEnumerable`.

Essentially what is happening it the example I wrote above, is that the `ToList()` says "I need to build a list. Give me the first item to start", and asks the it's input `IEnumerable` for some item.  This input `IEnumerable` is the object created by the second `Where()` method.  

This `IEnumerable` says "I have no object to complete that request, so I must ask *my* input `IEnumerable` for one". That input `IEnumerable` is, of course, the object created by the first `Where()`.

And so on.  That `IEnumerable` similarly says "I have no object to complete that request, so I must ask *my* input `IEnumerable` for one". That input `IEnumerable` is `myCollection`, which we'll assume is a `List<int>` holding [4, 7, 2, 6, etc] for this example.

`myCollection` (which, here, is just another `IEnumerable`) cordially offers up it's first element, 4,  when asked.  To which the first Where() says "Nope, not what I was looking for", and asks for another.  `myCollection` then gives up 7, which the first Where() likes, so it passes it onto the second Where().

*But*, the second Where rejects the 7, and thus must ask the first Where() for another `int`. For this, the first Where() must go back to `myCollection`, first for the 2 (which the first Where rejects), and then for 6, which it likes and passes onto the second `Where()`.

The second Where() also like the 6, and therefore passes it onto the `ToList()` which finally can place a value into the list it's building. But then, it immediately starts the whole process over --- asking for an `int` from the second Where(), which then asks the first `Where`, which asked `myCollection` and so on, until `myCollection` says it has no more, and that information is passed down the chain.

We can see this taking our example from before and augmenting it with some logging into the predicate lambdas. 

	int[] myCollection = {4,7,2,6};

	var newList =
		myCollection
			.Where(x =>
			{
				Console.WriteLine($"is {x} greater than 5?");
				return x > 5;
			})
			.Where(x => 
			{
				Console.WriteLine($"is {x} divisible by 3?");
				return x % 3 == 0;
			})
			.ToList();

And we'll get the output:

    Is 4 greater than 5?
    Is 7 greater than 5?
    Is 7 divisible by 3?
    Is 2 greater than 5?
    Is 6 greater than 5?
    Is 6 divisible by 3?
    