---
layout: post
title: Let's talk about some code (Javascript)
categories: code javascript programming
tags: code javascript programming
---

Ya'know, I'd always intended this blog to be one of them cool ".Net Blogs", with just a occasional entry about my life.  Ok, so two years later, I've got the occasion random life entry thing down, but in all the time, not one thing about programming.  Let's try correcting that.

Lately, I've been reading <a href="http://www.amazon.com/gp/product/0975240269/ref=as_li_ss_tl?ie=UTF8&amp;camp=1789&amp;creative=390957&amp;creativeASIN=0975240269&amp;linkCode=as2&amp;tag=njtheatercom-20">The JavaScript Anthology: 101 Essential Tips, Tricks &amp; Hacks</a><img src="http://www.assoc-amazon.com/e/ir?t=njtheatercom-20&amp;l=as2&amp;o=1&amp;a=0975240269" width="1" height="1" border="0" alt="" style="border:none !important; margin:0px !important;" />
, which is otherwise a fine book on Javscript technique. ( [website](http://www.sitepoint.com/books/jsant1/) )  However, every now & then, the authors just get a bit caught up in "clever" code.  For example, this snippet from pg. 178:

	function dofade(steps, img, value, targetvisibility, otype)
	{
		value+=(targetvisibility 1 : -1) / steps;
		if (targetvisibility value > 1 : value < 0)
			value = targetvisibility 1 : 0;
		setfade(img, value, otype);
		if (targetvisibility value < 1 : value > 0)
		{
			setTimeout(function()
				{
					dofade(step, img, value, targetvisibility, otype);
				}, 1000/ fps);
		}
	}

Now, the important thing to note here, is that nearly every line is affected by the value of targetvisibility, and we test for that value 4 times in just that short function.  Now, let's consider if we just tested for it once:

 	function dofade(steps, img, value, targetvisibility, otype)
	{
		if (targetvisibility)
		{
			value += 1 / steps;
			if (value > 1)
				value = 1;
			setfade(img, value, otype);
			if (value < 1)
			{
				setTimeout(function()
				{
					dofade(step, img, value, true, otype);
				}, 1000/ fps);
			}
		}
		else
		{
			value += -1 / steps;
			if (value < 0)
				value = 0;
			setfade(img, value, otype);
			if (value > 0)
			{
				setTimeout(function()
				{
					dofade(step, img, value, false, otype);
				}, 1000/ fps);
			}
		}
	}
 
So, now while, it's a bit longer, it's a whole bunch easier to read & understand. But let's take this a step further.  How do we initially call this function   Again, from pg. 178:

	if (dir == 'out') {dofade(steps, img, 1, false, otype);}
	else {dofade(steps, img, 1, true, otype);}
	
The significance of this is that now, nowhere is the targetvisibility parameter not set explicitly. In which case, there's no need for it:

<script src="https://gist.github.com/jamescurran/5464815.js">   </script>
>   
>   