---
layout: post
title:TimeThis! - Simple timings for code blocks (Fun with IDisposable)
tags: code programming csharp dotnet
---

This post is for day 14  of the [2021 C# Advent Calendar](https://www.csadvent.christmas/) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

After I reserving my spot for this year, I realized two things:
- I forgot to sign up last year, after participating the first three years and
- Largely due to that, I haven't written *anything* to my blog in **two years**!

So, to make up for that, I figured, "Let's post a couple in a row".  So, if you like this, check back here tomorrow and Thursdays for more!

-----
Sometimes, you want to know long a bit of code it taking to run.  Maybe not a full code analysis, but a read-out of how many second something is taking, particular if your not sure which section is the problem, or if one section varies a lot from run to run.

Doing this in C# is rather straight-forward : Just create and Start() a Stopwatch object before the code you want to time:

    var sw = new StopWatch();
    sw.Start();

And then afterwards, Stop() the timer, and report the results:

    sw.Stop();
    Console.WriteLine($"It took {sw.Elapsed}");

(Plus you have to remember to include "using System.Diagnostics" in your code.)

Two simple steps, but that's the problem -- *Two* steps, with your code in between, which makes it tricky to package into a neat library function.

One could make it into a method which takes a lambda, which would be the code you're trying to time, but that messy -- all local variables used need to be passed in, and all outputs need to be passed out.  And now you're changing the code you want to test.

Now, bak when I was wrting C++, this was simple.  Just create a class, which did the setup in its constructor, and the reporting in its destructor.  We create an object of that class where we want to start it, and it is automatically destroyed (and reports the results) when it goes out of scope.

Unfortunately, in C#, we don't have destructors that work like C++'s.  But we do have the IDisposable.   

    using (var tt = new TimeThis(_logger, "My Complex code"))
    {
        // my complex code here
    }

This will add to your logs something like this:

    Complete:My Complex code: Elapsed: 00:00:01.3521282

The message ("My Complex code" in the example), is optional.   That's nice, but we can make it a bit neater with an extension method.

    using (_logger.TimeThis())
    {
        // my complex code here
    }

Here's the full code.

<script src="https://gist.github.com/jamescurran/dbb537c68d2fd898178b4b3d4ef6f290.js"> </script>

