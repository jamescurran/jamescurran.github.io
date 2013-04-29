---
layout: post
title: What's inside a foreach() statement 
categories: code c# .net programming
tags: code c# .net programming
---

Recently I was looking at some C# code where the author, to loop through some collection,  would frequently use call GetEnumerator() and the manually step through the collections, calling MoveNext().  It seemed to me that the code could be written more cleanly using a foreach.   I got to wondering it the author knew some funky optimization detail, that I didn't, that made his code more efficient.
 
So I tried an experiment.  I wrote two methods: one, a lightly adapter version of the code I had been studying; and the other, the equivalent code, using a foreach.  I compiled them, and examined the generated IL in Reflector.
 
<script src="https://gist.github.com/jamescurran/5472511.js">    </script>


I'll spare you the IL listings, but Test1() was shorter by a small amount.  More interestingly, while some parts of the IL matched in both methods, a lot was different.  I studied the code to see the variations. One difference was in Test1, get_Current() was called twice, but only once in Test2(). But, mainly, Reflector claimed the Test2 had try and finally statements, which I figured were essentially a using{} block.  After a bit of playing, I came up with this:

<script src="https://gist.github.com/jamescurran/5472520.js">   </script>


Except for a few insignificant deviations (the order of temporaries on the stack, the placement of NOP instructions), Test2 &amp; Test3 produce identical IL code.  More interesting, if I ask Reflector to disassemble the assembly into C# code, it displays identical foreach loops for both methods. 

So, what have we learned from this   Well, a simply manual GetEnumerator loop will end up skipping the call to Enumerator.Dispose().  Whether that is an optimization or a bug depends on your code.  And, if you're not careful, you'll probably end up retrieving the Current property more than you need to.

In the end, you're probably better off calling foreach, which, one suspects, is why it was added to the language in the first place.

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/05/18/what-s-inside-a-foreach-statement.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/05/18/what-s-inside-a-foreach-statement.aspx" border="0" /></a>
> 