---
layout: post
title: A quick way to go to Shrinkster links.
categories: random-thoughts code programming
tags: random-thoughts code programming
---
I just read an article over on CodeProject on creating a small application to quickly [go to a Shrinkster.com url](http://www.codeproject.com/csharp/GotoShrinkster.asp), and as I was reading it, a simpler method occurred to me.

Backing up a bit, Shrinkster.com is a site which holds a giant database of web addresses.  If you wanted to give out a web page link on, say, an podcast, where you won't want to spell out a long URL ("double-you, double-you, double-you, Honest Illusion dot Com, Slash Blogs, slash blog underscore 0, slash archive, slash two thousand seven, slash zero two, slash....."), you could go to Shrinkster, enter the URL, and they would give you a short code (usually, three letters).  Then all you have to say is "Shrinkster.com slash A B C" (or what ever the code is).

However, in this fast-paced world we live in, even typing that little bit is too much for some people. They want it down to just entering the code, and going to the web page.

That was the gist of the CodeProject article cited above.  But, then I realized I could do it without any code at all.

OK, this works in for IE7 and Mozilla FireFox.  The concept is the same, although the setup for each is a bit different.

**Let's start with IE7 first.**
In the Instant Search Box (in the upper right corner), click the down arrow, to show the list of search providers.  (If you are doing this as you read this article, you'll want to open a new tab first)  Near the bottom should be "Find More Providers....".  Select that.  You will be brought to a page on Microsoft.com : ("Add Search Providers to Internet Explorer 7").  On the right, there's an Orange box labeled "Create You Own".  Fill it in as below, and click "Install".

Add your own search provider to your copy of Internet Explorer 7 by following these steps:

1. Visit the desired search engine in another window or tab
2. Use the search engine to search for TEST (all capital letters)
3. Paste the URL of the Search results page   
URL: **http://www.shrinkster.com/TEST**
1. Specify a name for the search provider
 Name: **Shrinkster**
1. [Install] View XML
  
Then, from now on, you'll have a Shrinkster provider for the search box.  Just select it from the menu and enter the code in the box.

**Now for Firefox:**

Well, you could go to [Searchy add provider page](http://searchy.protecus.de/en/add2.php), and enter things similar to the above, but since Searchy *saves* all the providers that others have created, you can just install the one I created : [http://searchy.protecus.de/en/searchbox-add-ons.php sort=newest](http://searchy.protecus.de/en/searchbox-add-ons.php?sort=newest).

Presently, it's right on top.  If it's moved down too much, try sorting by name, and looking under the "S"s.