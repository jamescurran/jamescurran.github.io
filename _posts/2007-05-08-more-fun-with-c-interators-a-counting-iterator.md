---
layout: post
title: More Fun with C# Interators:A Counting Iterator
categories: 
tags: 
---
[Michael Herman](http://www.dotnetjunkies.com/WebLog/mwherman2000/default.aspx) recently discovered a 
[typo in the MSDN docs](http://msdn2.microsoft.com/en-us/library/ms131103.aspx):   
"[IEnumberable: the integer version of IEnumerable  :-)](http://www.dotnetjunkies.com/WebLog/mwherman2000/archive/2007/05/06/233749.aspx)"

This reminded me of a method that I've been planning on presenting here:  A simple Counting iterator.  Basically, it appears to be a collection filled with a sequences of numbers.  For example, the following code will print the number from 20 to 41 stepping by three:

	 foreach(int i in Iter.Count(20,41, 3))
	 WL(i.ToString());

It's also good to create an actual collection pre-filled with sequence of numbers:
<script src="https://gist.github.com/jamescurran/5472360.js">    </script>

Also, I should point out that the idea behind this came from a C++ iterator that I first saw presented by Andy Koenig. 

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/05/08/more-fun-with-c-interators-a-counting-iterator.aspx">

<img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/05/08/more-fun-with-c-interators-a-counting-iterator.aspx" border="0" /></a>
