---
layout: post
title: Get an ApplicationBarIconButton by name (Redux)
tags: code c# .net generics-without-collections dotnet csharp codeproject
---
Yesterday, I read a blog post on [getting an ApplicationBarIconButton by name](http://www.ariankulp.com/get-an-applicationbariconbutton-by-name).  The author made a couple minor errors in the code, which I was going to leave a comment about, except his comment page is broken. So, another excuse to write something for my own blog.

The basics of the article are that in Windows Phone 7 coding, when referencing the ApplicationBar buttons, you never get an direct reference - you have to look the one you want up by name - and the author provided some code:

<script src="https://gist.github.com/jamescurran/5437371.js">    </script>

That's rather ugly code for a simple function.  What really bothered me was that he didn't just cast the object - he casted it *twice* -- pointlessly.

The author states that this is a bit uglier than you'd expect (and non-LINQ-able) because ApplicationBar.Button returns an IList, and because "the collection of buttons are of type *Object*, so you need to cast them." 

Neither of those statements are exactly accurate.  The objects in the collection really are of type ApplicationBarIconButton.  They just appear to be Object types due to the effect of the IList.  Why exactly that property returns an IList instead of the more precise IList&lt;ApplicationBarIconButton&gt; is a mystery known only to the devs at Microsoft.

So, are we stuck with this   No, we can improve that code.  "var" is often a very useful keyword, but you must understand what it does.  It says "declare this variable of the type of the object presented to initialize it" - which is expressly what we do **not** want here.  The IList is presenting the objects in the Buttons collections as Objects; but we know that they are ApplicationBarIconButtons and we want they treated like that.

<script src="https://gist.github.com/jamescurran/5437397.js">   </script>
Much cleaner, huh   But, let's return to the author's original point.  He wanted to use LINQ, but was blocked by the ILIST.   However, Microsoft realized that's often a problem, and wrote a way around it:  The **Cast&lt;T&gt;()** method.   It takes a non-generic IList, and a type, and transforms it into an generic IList&lt;T&gt;.  With that, the LINQ version is trivial:


<script src="https://gist.github.com/jamescurran/5437420.js">   </script>

