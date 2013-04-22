---
layout: post
title: JavascriptHelper-Managing JS files for ASP.NET MVC
categories: code c# .net javascript programming dotnet csharp aspnetmvc codeproject
tags: code c# .net javascript programming dotnet csharp aspnetmvc codeproject
---

After working several years with the Castle Monorail MVC framework, I decided to try ASP.NET MVC to see if it had caught up to Monorail. The transition seemed to go rather smoothly, but one area where I was surprised to find how clumsily it was handled, was the management of JavaScript files. Basically, if some part of a page, say a helper or partial page, needed a particular JS file, you had one of two choices.

The first option is to have the part itself would write the script tag. This allows the part to operate as a "black box" - just drop it in and it works - But it means that there will be script tags loading file scattered throughout the page, and that the part needs to know your folder structure where you keep your JavaScript files. And it needs to know if you want the file loaded from you website or from a CDS like Googleapi.com. And, since there's a good chance it will depend on jQuery, you have to make sure that jquery.js is loaded first, at the top of the page, despite "best Practices" which say script files should be loaded at the bottom of the page. Then, let's say, two different partial views on the same page use the same JS file, you need a way of making sure it's only included once. Plus, there's a good chance it will also depend on its own CSS file being loaded, which doubles the problems above. Microsoft (well, PluralSite's training videos on Microsoft's site) recommends putting the script tags in a @section named Scripts, and rendering that in the layout, which helps but only addresses some of the problems)

Alternately, you can break the black box, and manually add the needed script &amp; CSS link tags in your layout. This allows you to group the files tag together in the proper places, CSS at the top, JS at the bottom. But, you must know all the JS files all component of the page need, including all dependencies they have. And if you are putting these in a layout file, then you'll need to put all the JS files needed for all pages anywhere on the site.
  
Wouldn't it be great if there was a way to automatically figure out just the files we need for a certain page, and include just those, without us having to do a lot of thinking about it. Isn't that the type of thing we invented computers for 

Monorail also lacked such a manager, but Monorail has neither the major corporate sponsor nor the large user community base of ASP.NET MVC, where I figured *someone* would have written one. I wrote a manager like this for Monorail, so I guess that someone is going to be me... which leads us to the **JavascriptHelper.**

###Goals:

My goals for the JavascriptHelper were the following:

  * To have all needed JS files grouped together, in a single spot I specify (presumably at the bottom of the layout) 
 * To be able to specify a needed JS file in a view, partial view, or helper.
 * To have all JS files that file depends on included automatically.
 * To have files included only once, regardless of how many pieces requested them.
 * To be able to specify a block of JS code in a view, partial view or helper and have all the code blocks bundled together.
 * To be able to say if a code block should be included one per page, or repeatedly.
 * To be able to associate a needed CSS file, and have it similarly included where I specify in the layout. 
 * To be able to specific a needed file by a simple name.
 * To be able to easily update the file a particular name refers to (in case where the actual filename includes the version number, like jquery-1.5.1.js
 * To be able to use one version of a file during development/debugging and easily switch to another (presumably mini-ified) for production.
 * To be able to use a local version of a file during development/debugging (when I'm often off-line), and easily switch to Google's CDS for production.
 * To be able to gather a collection a JS files into a single, combined file (mini-fied on the fly) (OK, the JavascriptHelper doesn't actually do that, but implementing it would require rewriting only one method --- and adding a controller that handles the actual bundling &amp; mini-fying. We'll get to that eventually)
 
 
 ###Usage:
 
Let's say for example that I have a partial view that uses the jQuery UI slider control (which I'll specify some unique id value). Plus it has JS code which needs to be called to initialize it. Say this is in the form of a method and a call to that method using that id. Now, let's say that we want several sliders on that page, and use that partial view several times, so we'll only the UI scripts and the method definition once, but we'll need each separate call to the function. To handle this, you'd need to add to you partial view, the following:

    Script.Std("slider")
    Script.AddScript("MySlider", "function HideSlider(id) { $(id).Hide();}")
    Script.AddScript("HideSlider('#"+ myId + "');")

`"Script.Std()"` says that "slider" is one of the standard JavaScript files which we have defined what it's traits and dependencies are. Actually, that could be a comma-separated list of them, so you state everything you need in one line. I originally envisioned only a few "standard" file - jQuery, jQuery UI, et al - but soon realized assigning a keyword for every script file used made life easier. In fact, it will even accept "self" to load a script &amp; CSS file based on the name of the view, i.e., id Script.Std("self") is used in /Home/Index, then it will load /Scripts/Views/Home/Index.js and /Content/Css/Home/index.css (if they exist on the file system). How a file is pre-defined, as well as where we get a Script object, will be discussed in the next section. 

The first `"Script.AddScript()"` call defines the function that we need. The second AddScript called calls that function, using the id specific to that instance of the partial view. So, if our page has three instances of this partial view, we'll need the function definition only once, but the call to it three times. This is handled by given a name to the snippet that need not be repeated. All blocks with the same name are rendered only once (actually, block with the same name as an existing block are ignored, so make sure that you only use a particular name for one script block. )

Then in the Layout, we just add the lines:
<script src="https://gist.github.com/jamescurran/5437560.js"></script>
This will produce an output of :

<script src="https://gist.github.com/jamescurran/5437573.js"></script>

###Setup:

Creating a Script object.

The "Script" object needs to be created differently depending on where it is being used. (You can, of course, call it anything you like. I use "Script" with a capital S, so that it corresponds to @Html).  (**Update:**  *With the release of MVC 4, Microsoft added their own, completely different pre-defined object called "Script", so I've switched to calling mine "JScript"*)

####When used in a view, create it like this:


    @{ var Script = StateTheaterMvc.Component.JavascriptHelper.Create(this; }

####When used inside a @helper method or an HtmlHelper extension method:

    var script = StateTheaterMvc.Component.JavascriptHelper.Create(WebPageContext.Current);

####Configuration XML file

Finally, we reach the part which is at the heart of the JavascriptHelper, the jslibraries.xml file. It's a bit complex, but the default I've set up is probably good enough, with perhaps only a few minor tweaks, and once fully configured to your taste, you could easily standardize on that for your whole enterprise. It is, however, simple enough that a NuGet package could be updated it when being added to a project. And the good news is that I've also create a XSD schema file for it, so you can have IntelliSense to help you along.

The root element is &lt;libraries&gt; and it has three optional attributes:

"`localjspath=`" which gives the relative location of the folder where you put your js files. The default is "~/Scripts" which is the ASP.NET MVC default.

"`selfJsPath=`" which gives the relative location of the folder under which you put your view-specific js files. They follow the same structure as view files, so if you request the js file for /Home/Index, the view file would be /Views/Home/Index.cshtml, and the js file used what be /Scripts/Views/Home/Index.js. Defaults to "~/Script/Views"

"`useDebugScripts=`" when set to "true" forces use of debug versions of js files when available. Defaults to false. Debug scripts are always used when a debugger is attached.

    <library name="jquery" version="1.6.2" useGoogle="false" 
  pathname="jquery-1.6.1.min.js" debugPath="jquery-1.6.1.js" />

Within the `&lt;libraries&gt;` element, there are a collection of `&lt;library&gt;` elements - one for each standard JavaScript file you'll be using-- which have two required and several optional attributes:</p>

"`name=`" which gives a unique name for a particular JavaScript file. It is required and is used by the helper and throughout the xml file to refer to that file.</p>

"`pathname=`" which gives the path, relative to the` localjspath` described above, and filename of this file. This can also be a fully-qualified URL if it's a remote file. It is required. (unless useGoogle.

"`debugPath=`" which, like the pathname attribute, give the relative path to a javascript file to be used when debugging. This is optional and defaults to the value given for the pathname. The idea here is the you set the debugPath to the full, commented version, and the pathname to the mini-fied version, and the helper figures out which to use.

"`dependsOn`" which is a list of comma separated name of file, which must be loaded before this one. This is the most powerful feature, but also the most confusing, so we'll expand upon it in a moment. It is optional and defaults to no dependencies.

"`alias=`" which is a list of comma separated alternate names this file could be identified as. I added this because I could never remember if it's "accordion" or "accordian". This is optional and defaults to no aliases.

"`css=`" which gives the name of the css element (see below) associated with this file.

"`useGoogle=`" which, when set to true, will generate a request to the googleapi.com CDN to load the file. Optional, defaults to false. If true, "version" attribute is required.

"`version=`" is used only when "useGoogle" is true. Gives the version of the file to load from Google.

Also with the &lt;libraries&gt; element (at the same level as the &lt;library&gt; elements), there is the &lt;css&gt; group.

The &lt;css&gt; element has only one attribute:

"`localcsspaath`" which gives the relative location of the folder where you put your CSS files. The default is "~/Content" which is the ASP.NET MVC default.

Within the &lt;css&gt; element are multiple &lt;sheet&gt; element, one for each css file associated with a js file. They have two required attributes:

"`name=`" Unique name used to identified the particular css file. Matches name given in the "css" attribute in the &lt;library&gt; element above. Can be the same as the name used for the js file in the &lt;library&gt; element.

"pathname=" which gives the path, relative to the localcsspath described above, and filename of this file.

###Example:

<script src="https://gist.github.com/jamescurran/5437587.js"></script>

Starting with the first element, we specified a JavaScript file, which we'll be calling "jquery".   When we ask for "jquery", we'll normally get "jquery-1.6.1.min.js" , unless we're debugging, in which case, it will load "jquery-1.6.1.js".  It will load these from the website, but, if, as I put it into production, I flip the useGoogle file to true, then it will load it from [http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.js](http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.js)

(Note that the "name" value is used to the URL here.  This is the only place where the actual name used is significant.  Otherwise it can just be any unique string.  This is wrong, and will be fixed in some future release).

Next we want to define the complete componentized version of jQuery UI, so that we can load just the parts we want. First, we declare the UI core as depending on jQuery.  Then we declare widget component as depending on the core.  And the mouse component as depending on the widgets.

Then we can the individual components themselves.  "datepicker" as depending on the widgets; "slider" as depending on the widget &amp; the mouse components.  Since the mouse already depends on widget, it was really necessary to specify both here, but there's no harm.  So, if you request to use "slider" on your page, then the helper makes sure that jQuery, the UI core, UI Widgets, mouse &amp; slider components are all included, in the proper order.  (Note, there is no checking for circular dependencies, so be careful how you specify the dependsOn attribute)

Both are also associated with the "ui" css file, which brings us to that section:

    <css localcsspath="~/content/css/Views">
       <sheet name="ui" pathname="ui/themes/Redmond/jquery-ui-1.8.13.custom.css" />
       <sheet name="cluetip" pathname="jquery.cluetip.css" />
    </css>

Both of the jQuery UI components above give "ui" as their CSS file (the theme roller doesn't create individual CSS files) so if either (or both) is used, that file is included.  CSS files don't have a separate dependency tree, but all associated CSS files for every dependent js file up the tree are included, so if you wanted to use the separate jQuery UI CSS files, you'd associate  "ui.core.css" with uicode", "ui.base.css" with "uiwidget" and each component with its own CSS file, and all the needed files will be included.

###API

The API is fairly simple:

`Std(string)` - accepts a comma-separated list of script file ids.</p>

`AddScript(string id, string script)` - accepts a block of script as text. Multiple calls with the same id are rendered only once.

`AddScript(string script)` - accepts a block of script as text. Each call is rendered.</p>

All script from either AddScript methods is rendered in a block at the script insertion point.

`AddOnReadyScript(string script)`- accepts a block of script as text. Appended to script run on page ready. All on rendered, wrapped in a jQuery document ready event function, at the point of InsertOnReady(

`InsertScripts()`-- Renders all script files &amp; script blocks.

`InsertOnReady()` -- Renders all script block intended for startup, wrapped in a jQuery on ready function.

`InsertCss()`-- Renders all css files

###What's the next step 

Now that I've made this public, it's really just a beta. I think there's still a bit more to be done before it's ready to be "Production-Ready v1.0". And I need some feedback....


 * First of all, how exactly should it be packaged  I've been just added to source file to my project, which is simple, but not very elegant. The alternative would be to create an assembly for it, but it's just one file, so that seem like overkill. I'd really like to create a NuGet package for this, but that question needs to be settled first.

 * Or do we think bigger  Microsoft has open-sourced the MVC framework, and is [now accepting pull-requests](http://haacked.com/archive/2012/03/29/asp-net-mvc-now-accepting-pull-requests.aspx).  Should be deeply embedded into the MVC eco-system  

 * Also, how is the API  Are the method names sufficiently intuitive 

* Is an XML file the proper way to store the dependency information 

* Is there any feature that really needs to be added 

###The code:

The source code is available (under the Apache license) from my GitHub library:

[http://github.com/jamescurran/JavascriptHelper](http://github.com/jamescurran/JavascriptHelper)

<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2012%2f03%2f29%2fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx">

<img border="0" alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%253a%252f%252fhonestillusion.com%252fblogs%252fblog_0%252farchive%252f2012%252f03%252f29%252fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx" /></a>


<div class="wlWriterHeaderFooter" style="margin:0px;padding:0px 0px 0px 0px;"><div class="shoutIt"><a href="http://dotnetshoutout.com/Submit?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2012%2f03%2f29%2fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx&amp;title=JavascriptHelper%e2%80%93Managing+JS+files+for+ASP.NET+MVC"><img alt="Shout it" style="border:0px;" src="http://dotnetshoutout.com/image.axd?url=http://honestillusion.com/blogs/blog_0/archive/2012/03/29/javascripthelper-managing-js-files-for-asp-net-mvc.aspx" /></a></div></div>