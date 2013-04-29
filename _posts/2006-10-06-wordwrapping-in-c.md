---
layout: post
title: Wordwrapping in C#
categories: code c# .net programming
tags: code c# .net programming
---

Some time ago, I needed a function that would take a block of text, and word-wrap it at a specific line length. As apparently you have now done, I googled for it, and found a blog with a seemingly appropriate algorithm.  

Except it wasn't.  I immediately noticed that it wasn't very efficient -- but that was merely annoying.  It wasn't grave enough to actually effect the running time of my program noticeably.  However, I soon noticed something worse -- it just didn't work.  It would occasionally drop the final line of the block.  So, I spent an hour or so rewriting it - fixing that problem and giving it a much more efficient algorithm while I was at it. (plus adding a feature I needed for my project).

The key to my speed-up was to copy characters and create new strings as little as possible.  In the original, he was concatenating strings merely to measure how long they would be. When a line got to be too long, he throws away the string, and used the save int value of the previous length.  This could be don't not only faster, but also simpler by counting the characters and subtracting.

The revised code is given below.  Very little is left from the original (although I think I'm still using some of the original variable names).  The key to the speed-up is the avoid creating new string except when absolutely necessary.
 
<script src="https://gist.github.com/jamescurran/5471549.js">   </script>

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2006/10/06/Wordwrapping-in-C_2300_.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2006/10/06/Wordwrapping-in-C_2300_.aspx" border="0" /></a>