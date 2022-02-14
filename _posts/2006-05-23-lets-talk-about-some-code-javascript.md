---
layout: post
title: Let's talk about some code (Javascript)
tags: code javascript programming
---

Ya'know, I'd always intended this blog to be one of them cool ".Net Blogs", with just a occasional entry about my life.  Ok, so two years later, I've got the occasion random life entry thing down, but in all the time, not one thing about programming.  Let's try correcting that.

Lately, I've been reading [The JavaScript Anthology: 101 Essential Tips, Tricks &amp; Hacks](http://www.amazon.com/gp/product/0975240269/ref=as_li_ss_tl?ie=UTF8&camp=1789&creative=390957&creativeASIN=0975240269&linkCode=as2&tag=njtheatercom-20)
![The JavaScript Anthology](http://www.assoc-amazon.com/e/ir?t=njtheatercom-20&l=as2&o=1&a=0975240269), 
which is otherwise a fine book on Javscript technique. ( [website](http://www.sitepoint.com/books/jsant1/) ) 
However, every now &amp; then, the authors just get a bit caught up in "clever" code.  For example, this snippet from pg. 178 :

<script src="https://gist.github.com/jamescurran/7443388.js">     </script>

Now, the important thing to note here, is that nearly every line is affected by the value of `targetvisibility`, and we test for that value 4 times in just that short function.  Now, let's consider if we just tested for it once :

<script src="https://gist.github.com/jamescurran/7443419.js">  </script>

So, now while, it's a bit longer, it's a whole bunch easier to read &amp; understand. But let's take this a step further.  How do we initially call this function? Again, from pg. 178 :

<script src="https://gist.github.com/jamescurran/7443825.js">  </script>
    
The significance of this is that now, nowhere is the `targetvisibility` parameter not set explicitly. In which case, there's no need for it:

<script src="https://gist.github.com/jamescurran/5464815.js">   </script>

