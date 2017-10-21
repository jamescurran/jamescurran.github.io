---
layout: post
title: ASP.NET MVC Helper - MoreLessText
tags: code C# csharp programming ASP.NET aspnet MVC javascriptloader
---

I've been meaning to write about the MVC Helper I wrote, but hesitated for a while because it used my JavascriptLoader component. But since that up on [NuGet where everyone can installed it](/blog/2014/01/15/JavascriptLoader-for-MVC-now-on-NuGet/), it's time to present it here.

Large blocks of text are often unwieldy (and ugly) on webpages.  But, sometimes, we just need all that text available.  A common way to deal with this is to initially display just an excerpt of the whole text block, but allow the user to "open up" the text to reveal the full version.  I use this technique when displaying show description on my other site, [NJTheater.com](http://www.NJTheater.com)

So, let's look at how this is handled.  It is implemented in ASP.NET MVC as a Helper.  It's called like this:

	 <div class="Description">
	 @Html.MoreLessText(Model.Description, 100)
	 </div>

The two parameters are the long text to be displayed, and the maximum number of characters to display before hiding the rest.  These are followed by two optional parameters, text which will be used for button to "open" and "close" the text, but here's we're just using their defaults, "(more)" and "(less)" respectively. 

<script src="https://gist.github.com/jamescurran/94bb06354b506fc0bac2.js">     </script>

The code is fairly straightforward, but that leads rather dull blog articles, so let's elaborate. 

First we check if the text is short enough to display straight. If so, we just return it, and move on.

If not, we invoke JavascriptLoader to ensure that "JQuery" is loaded. Granted, virtually every ASP.NET MVC page is automatically loading jQuery, but being robust here comes free.

Next, we add a short Javascript function, again using JavascriptLoader.  By including a tag as the first parameter, JavascriptLoader knows to include that block only once, regardless of how many times that block as added with that tag.  This is important as you'll frequently be calling MoreLessText several times on the same page, and we only need the function to appear once.

Then, we search for the last space *before* the maximum length -- so we don't chop off a word in the middle -- and use that to build the Html for our text.  It will look like this.

         [Truncate text]
         <span id="extraText-12ABF3D">
         [Remainder of text]
         <a onclick="moreless('12ABF3D');">(less)</a>
         </span>
         <span id="more-12ABF3D" style="display:none">
         <a onclick="moreless('12ABF3D');">(more)</a>
         </span>

The value "12ABF3D" is a random id generated new each time.  It's based on a GUID, but shorted to be less unwieldy (however, also now no longer guaranteed to be unique, although collisions on a page are unlikely).

The onclicks call the moreless function we defined before, which toggles the extra text on & off (and also which button in displayed).

Finally, we add a call to that function is the startup script.  This is because the Html displays the full text, so we now want to immediately hide the extra. I wrote it this way, just in case javascript isn't running on someone's browser.  Better to have the full text always showing, rather than giving the user a cut-down version and no way to expand it.
