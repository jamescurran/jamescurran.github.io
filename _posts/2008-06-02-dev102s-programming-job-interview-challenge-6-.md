---
layout: post
title: DEV102's Programming Job Interview Challenge 6 
tags: code c# .net dotnet csharp
---

Another week, another C# interview question from the good folk's at Dev102.com -- Although I use the term "good folks" advisedly, as this week they did not even acknowledge the solution I posted for last weeks puzzle (which was both correct, and, I believe, the first blog post about it).

Anyway, time to move on to [this week's question](http://www.dev102.com/2008/06/02/a-programming-job-interview-challenge-6-c-games/).

>*Look at the following Code segment written in C#:*

    ArrayList a = new ArrayList();
    ArrayList b = new ArrayList();
   
    a.Add(1);
    b.Add(1);
    a.Add(2);
    b.Add(2.0);
   
    Console.WriteLine((a[0] == b[0]));
    Console.WriteLine((a[1] == b[1]));
  
  
> *What will be typed into the console? And **WHY**?*

This one is fairly trivial (They seem to be alternating between difficult and easy questions).  And, as requested, I formulated my answer before typing it into VS (actually, I copy'n'pasted in SnippetCompiler), but the compiler DID confirm the answer I'd already theorized.

This answer is short enough that we can use the cool "white-on-white; select to see it" trick--- However, RSS feed (and apparently the theme of this blog) seem to ignore the color style, so it's probably visible to you below..


<div style="color:white;">
ArrayList is deep-down, just an object[].  To store an valuetype, like an int or float, in an ArrayList, that value would first have to be boxed.  Each valuetype is boxed separately, in distinct objects, even if they do happen to have the same value. When we get to the WriteLines, we are just performing (object) == (object) (actually, `Object.ReferenceEquals(object1, object2);` )  ReferenceEquals knows nothing about unboxing.  It just asks, "Are these two references pointing to the exact same object ".  For any two boxed objects, regardless of their value, the answer would be "No".  Hence, both lines print "False".
</div>

 (select the blank space above)
