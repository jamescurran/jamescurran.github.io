---
layout: post
title: JavascriptHelper (Redux) - Now with Bundling &amp; Compression!
categories: code c# .net javascript programming dotnet csharp aspnetmvc
tags: code c# .net javascript programming dotnet csharp aspnetmvc
---

Eight weeks ago, I revealed to the world my JavaScript Helper component.  At the time, I promised that I would be adding bundling and compression to it.
  
And you scoffed.
  
Ha!
  
Ok, I really hadn't planned on adding it this soon , but right after my first blog article on the helper appeared, lots of other bloggers started writing  about the bundling features in the MVC4 beta, and is *seemed* simple enough. 
  
For those of you who missed the first part ([JavascriptHelper-Managing JS files for ASP.NET MVC](http://honestillusion.com/blog/2012/03/29/javascripthelper-managing-js-files-for-asp-net-mvc.html)), a quick review (You'll probably want to read that article eventually anyway):   JavascriptHelper is a MVC component which allows you to specify that a JavaScript file is needed, wherever you need it (view, partial views, layouts, helpers etc.) and the helper will collection them all up, plus all their dependencies - in the right order--  and insert all the &lt;script&gt; tags in one spot --- and at the same time, do the same thing for the CSS files those JS scripts need.    For example, in a partial view, you wish to use the jQuery UI Slider widget, you can simply write:

  `@Script.Std("slider")`
  
and at the spots you designate in your layout file, it will insert the &lt;script&gt; tags for jQuery, and just the parts of jQuery UI needed for the slider (core, widget, mouse, and slider), plus the jQuery UI CSS file.  And, now with v2.0, it will also optionally bundle them into one big file (optionally compressing them in one small file)
  
### Usage: 
  
Sparing you the ugly details of how I got this to work, using it is simple.   Upgrade to the latest version.  Turn it on.  It just works.   Essentially, you use it exactly like the non-bundling version described in the previous article.   All bundling &amp; compression is now just handled automatically behind your back.

###Enabling Bundling:

There is just one global on/off switch, so you can leave it off during development, and then switch it on as you prepare for deployment.  On the &lt;libraries&gt; element of the jslibraries.xml file, there's a new attribute, "transform".  It takes one of three options: "None", "BundleOnly", and "Compress".  They should be self-explanatory.  The default is "None", so you must add the option to see any effect.     (Note that, presently the bundling features are in `#if MVC4`  blocks in the code, so you will need to add "MVC4" to the "Conditional Compilation Symbols" section of the "Build" tab on your web project's property page.  These will be removed after MVC4 is officially released and comes into common usage.)  Oh, and naturally, we'll have to be running the beta of ASP.NET MVC4.

### Ugly details that shouldn't matter to you, but I need to vent:
   
First of all, as far as I can tell, MVC bundling (not this code; the bundling code provided by Microsoft in the beta), is just broken.   The first indication of this is that the "RegisterTemplateBundles()" method loads a very specific set of JS files, with their filenames hard-coded right in the IL code.   I'm certain that's going to be changed before the RTM version.    
  
It also appears that any two bundles given the same virtual file name (regardless of the page it's on, the content of the bundle or the query string parameter) will get the content of the first bundle created with that name back.   If you say "Well, that's to be expected", remember that the default bundling code created by the Wizard uses the same virtual file name for every page.   That Wizard-generated code also puts every JS file in your ~/Scripts folder into the bundle, so every page uses the same bundle anyway.  The problem only shows up when you try to control the bundle contents more closely.   When I deviated from the default,  I started seeing that the second page I viewed only got the JS files that the first page had requested.
  
So, there's a good chance Microsoft may rewrite a lot of this before MVC4 reaches it's production release, and with any rewrites, this code might break.  (Although, what I think is the worst that might happen, is that this code has a bunch of workarounds that will have become unnecessary)
  
I discovered this when testing a scenario which Microsoft didn't seem to plan for - a selection of JS files used on a page includes both local files to be bundled, and external files which are not, and with the external files in the middle of the list, requiring two separate bundles on one page.
  
Say for instance, you are loading jQuery off of a CDN (like the one Microsoft runs), or you are using a WebService where the service wants you to load the API JS file directly from their site (like Microsoft's Bing Maps), then you'll have JS files, which would be handled by the JavascriptHelper, which should not be part of the bundle, but may, through the dependency tree, show up between local files.   In that case, you'll need one bundle to be loaded before the external files, and a second to be loaded after them. 
  
### Bonus feature, unrelated to Bundling:
   
I realized that sometimes you'll want a JavaScript file loaded on every page (before you say "jQuery!", remember, that's handled by the dependencies). OK, what you really want to a particular CSS file loaded on every page.  JavascriptHelper now offer a way to handle that and include them in bundling.  Define a &lt;library&gt; entry with the name of "base".  You don't need to specify a path name for it, but include a dependsOn attribute which specifies the JS files you always want loaded.   Then add a &lt;sheet&gt; element named "base" which has a pathname of your standard CSS file (site.css if you follow the MVC4 defaults)
  
### What's the next step 
  
This haven't changed since the last article.  I think there's still a bit more to be done before it's ready to be "Production-Ready v1.0". And I STILL need some feedback....

 * First of all, how exactly should it be packaged  I've been just adding the source file to my project, which is simple, but not very elegant. The alternative would be to create an assembly for it, but it's just one file, so that seem like overkill. I'd really like to create a NuGet package for this, but that question needs to be settled first. 
 * Or do we think bigger  Microsoft has open-sourced the MVC framework, and is [now accepting pull-requests](http://haacked.com/archive/2012/03/29/asp-net-mvc-now-accepting-pull-requests.aspx). Should it be deeply embedded into the MVC eco-system  
 * Also, how is the API  Are the method names sufficiently intuitive 
 * Is an XML file the proper way to store the dependency information 
 * Is there any feature that really needs to be added 


### The code:
  
  The source code is available (under the Apache license) from my GitHub library:
  [http://github.com/jamescurran/JavascriptHelper](http://github.com/jamescurran/JavascriptHelper)

