---
layout: post
title: Code Tune-Up:Shuffling a List
categories: code c# .net programming dotnet csharp codeproject
tags: code c# .net programming dotnet csharp codeproject
---
Over on CodeProject, I spotted an article by Mahdi Yousefi called "[Creating an ASP.NET captcha using jQuery and s3capcha](http://www.codeproject.com/KB/validation/aspnet_capcha.aspx)", which include this routine to shuffle a `List<>` of integers.


<script src="https://gist.github.com/jamescurran/5439129.js">   </script>

So, what's wrong with this   Well, let's see:

 * It takes a `List` as a parameter and returns a list.  This is rather limiting.  What if we have array we want to shuffle? 

 * It takes and returns a list of **integers**. Again rather limiting.  What if we had strings or say, PlayingCard objects that we want to shuffle?

 * It creates a new `Random` object every time it's called.  Two problems there.  First, when a `Random` object is created, a seed is produced.  This is a fairly time-consuming task, which you don't want to do repeatedly unnecessarily.  Second, when the default constructor is called, like here, the seed is initialized using the internal clock's `TickCount` --- which is the time in _milliseconds_.  If you called `shuffle`() twice within a millisecond -- not unreasonable if you were writing a game - the `Random` objects would be using the same seed, and produce the same sequence.

 * It creates a new list for output.  This  is a problem only in that it's a time expense we might as well avoid if possible.  It also builds this list by repeated calls to Add(), but without specifying an initial size, meaning that Add() will frequently have to resize the list to keep expanding it.  The fix to this would be trivial.  Just create the new list as "`new List<int>(input.Count);`".  But as you'll see, this won't be necessary.

 * It destroys the list input.  In fact, it destroys it three times over: First by removing all of it's items.  Then by calling Clear() on the empty list.  Then by setting the local reference to null.  That last one might cause it to be garbage-collected a couple microseconds earlier - if the calling routine wasn't holding a reference to it. I don't want to claim this as a "Problem", as much as a "Behavior" - It's just something it does, so if our replacement does that as well (as it will), we haven't lost anything.  But, if you nevertheless thing that _is_ a problem, don't worry, we'll address that to.

 * It removes that items from the list using Remove() - This is a very time-consuming method on Lists (which, contrary to popular belief are not linked-lists, but are internally implement as arrays).  One call to `List.Remove()` is *O(N)* by itself.  Since it's called in a loop, that makes the complicity of this method *O(N^2)*.  Clearly that's something we should avoid.

So,  let's tackle these.  First the signature.  We say we want a `List`, but we really only want some features of a list - that usually means we want an interface.  And we want to be usable for `List` of all types, so it's wants to be generic:

    public static IList<T> Shuffle<T>(this IList<T> input)

`IList` has just the features we need, and allows us to pass different collection types (notably arrays) to the method.  I've also implemented it as an extension method, because it seemed more useful that way.  But to be an extension method, it has to be in a static class, which brings us to our next change:

    static class Helper
    {
           static readonly Random rnd = new Random();
        // :
        
I've moved that Random object to be a static member.  That way, only one is created &amp; initialized, and every call to `Shuffle` reuses the same one.

Next is that main loop:

    for(var top = input.Count -1; top < 1; --top)
    {
        var swap = rnd.Next(0, top);
        T tmp = input[top];
        input[top] = input[swap];
        input[swap] = tmp;
    }

Here's where we see the major change to the implementation. Both methods use basically the same algorithm (The [Fisher-Yates shuffle](http://en.wikipedia.org/wiki/Knuth_shuffle) also known as the Knuth Shuffle) , but instead of building a new List, I move the elements around within the same list.  Essentially, I'm doing the same thing, if you imagine the two arrays occupying that same space - as one grows small the other grows bigger. (Basically, the original closely modeled Fisher & Yates's method, which was intended to be done by hand with paper &amp; pencil, and the revision here follows Knuth's optimization for computers)

And with that, we're done.  Since the original List is now shuffled, we could return void, but by returning the input, we can allow chaining (and it also maintains the original method signature)

So, How does it work? Here's a quick example, showing off some of it's new abilities:

        string[] A = {"A", "B", "C", "D", "E", "F", "G"};
        A.Shuffle().Print();

       output: D-F-A-G-B-E-C-
       
`Print()` is a simple-minded extension method which just takes a list and prints it's elements separated by dashes.  Good for demos but not much else.  Also for these examples, I've hard-coded the seed for Random to be 1234, so the sequence is always repeated.  Again, good for demos, but not for production work.

But, you said you didn't want the original list destroyed. (Yes, you did in fact say that!)  No problem, we'll just write a second method, and since the problem definition requires two lists, there's no shame in eating the cost of creating a copy of the input list.  To keep it simple, I'll also create & return a `List<T>` regardless of what type of `IList<>` you passed in. 

     return new List<T>(input).Shuffle();

But that brings us to another key point.  The only thing that the input is being used for is to seed the new List, and that ctor doesn't take an` IList<>`, it takes the much more common `IEnumerable<>` (which `IList` just happens to be a descendant of).  So, we might as well make that our input parameter.

    public static IList<T> ShuffleCopy<T>(this IEnumerable<T> input)
    {        return new List<T>(input).Shuffle();    }

With this, we can do some interesting things, since you input doesn't have to be a collection at all:

      Enumerable.Range(1,10).ShuffleCopy().Print();

      output:  1-5-7-9-10-2-6-3-8-4-
      
Here's the full source code:

<script src="https://gist.github.com/jamescurran/5439217.js">   </script>
