---
layout: default
title: JavascriptHelper–Managing JS files for ASP.NET MVC
categories: code c# .net javascript programming dotnet csharp aspnetmvc codeproject
tags: code c# .net javascript programming dotnet csharp aspnetmvc codeproject
---

  <p>After working several years with the Castle Monorail MVC framework, I decided to try ASP.NET MVC to see if it had caught up to Monorail. The transition seemed to go rather smoothly, but one area where I was surprised to find how clumsily it was handled, was the management of JavaScript files. Basically, if some part of a page, say a helper or partial page, needed a particular JS file, you had one of two choices.</p>
  <p>The first option is to have the part itself would write the script tag. This allows the part to operate as a “black box” – just drop it in and it works – But it means that there will be script tags loading file scattered throughout the page, and that the part needs to know your folder structure where you keep your JavaScript files. And it needs to know if you want the file loaded from you website or from a CDS like Googleapi.com. And, since there’s a good chance it will depend on jQuery, you have to make sure that jquery.js is loaded first, at the top of the page, despite “best Practices” which say script files should be loaded at the bottom of the page. Then, let’s say, two different partial views on the same page use the same JS file, you need a way of making sure it’s only included once. Plus, there’s a good chance it will also depend on its own CSS file being loaded, which doubles the problems above. Microsoft (well, PluralSite’s training videos on Microsoft’s site) recommends putting the script tags in a @section named Scripts, and rendering that in the layout, which helps but only addresses some of the problems)</p>
  <p>Alternately, you can break the black box, and manually add the needed script &amp; CSS link tags in your layout. This allows you to group the files tag together in the proper places, CSS at the top, JS at the bottom. But, you must know all the JS files all component of the page need, including all dependencies they have. And if you are putting these in a layout file, then you’ll need to put all the JS files needed for all pages anywhere on the site.</p>
  <p>Wouldn’t it be great if there was a way to automatically figure out just the files we need for a certain page, and include just those, without us having to do a lot of thinking about it. Isn’t that the type of thing we invented computers for?</p>
  <p>Monorail also lacked such a manager, but Monorail has neither the major corporate sponsor nor the large user community base of ASP.NET MVC, where I figured <i>someone</i> would have written one. I wrote a manager like this for Monorail, so I guess that someone is going to be me… which leads us to the <b>JavascriptHelper.</b></p>
  <p><b>Goals:</b></p>  <p>My goals for the JavascriptHelper were the following:</p>
  <ul>   
  <li>To have all needed JS files grouped together, in a single spot I specify (presumably at the bottom of the layout) </li>
  <li>To be able to specify a needed JS file in a view, partial view, or helper. </li>    <li>To have all JS files that file depends on included automatically </li>
  <li>To have files included only once, regardless of how many pieces requested them. </li>
  <li>To be able to specify a block of JS code in a view, partial view or helper and have all the code blocks bundled together. </li>
  <li>To be able to say if a code block should be included one per page, or repeatedly. </li>
  <li>To be able to associate a needed CSS file, and have it similarly included where I specify in the layout. </li>    <li>To be able to specific a needed file by a simple name. </li>
  <li>To be able to easily update the file a particular name refers to (in case where the actual filename includes the version number, like jquery-1.5.1.js </li>
  <li>To be able to use one version of a file during development/debugging and easily switch to another (presumably mini-ified) for production. </li>
  <li>To be able to use a local version of a file during development/debugging (when I’m often off-line), and easily switch to Google’s CDS for production. </li>
  <li>To be able to gather a collection a JS files into a single, combined file (mini-fied on the fly) (OK, the JavascriptHelper doesn’t actually do that, but implementing it would require rewriting only one method --- and adding a controller that handles the actual bundling &amp; mini-fying. We’ll get to that eventually) </li>
  </ul>
  <p><b>Usage</b></p>
  <p>Let’s say for example that I have a partial view that uses the jQuery UI slider control (which I’ll specify some unique id value). Plus it has JS code which needs to be called to initialize it. Say this is in the form of a method and a call to that method using that id. Now, let’s say that we want several sliders on that page, and use that partial view several times, so we’ll only the UI scripts and the method definition once, but we’ll need each separate call to the function. To handle this, you’d need to add to you partial view, the following:</p>
  
<div class="csharpcode">
  <pre class="alt">Script.Std(<span class="str">"slider"</span>)</pre>

  <pre>Script.AddScript(<span class="str">"MySlider"</span>, <span class="str">"function HideSlider(id) { $(id).Hide();}"</span>)</pre>

  <pre class="alt">Script.AddScript(<span class="str">"HideSlider('#"</span>+ myId + <span class="str">"');"</span>)</pre>
  
</div>

<p><font face="Consolas"></font></p>

<p>“Script.Std()” says that “slider” is one of the standard JavaScript files which we have defined what it’s traits and dependencies are. Actually, that could be a comma-separated list of them, so you state everything you need in one line. I originally envisioned only a few “standard” file – jQuery, jQuery UI, et al – but soon realized assigning a keyword for every script file used made life easier. In fact, it will even accept “self” to load a script &amp; CSS file based on the name of the view, i.e., id Script.Std(“self”) is used in /Home/Index, then it will load /Scripts/Views/Home/Index.js and /Content/Css/Home/index.css (if they exist on the file system). How a file is pre-defined, as well as where we get a Script object, will be discussed in the next section. </p>

<p>The first “Script.AddScript()” call defines the function that we need. The second AddScript called calls that function, using the id specific to that instance of the partial view. So, if our page has three instances of this partial view, we’ll need the function definition only once, but the call to it three times. This is handled by given a name to the snippet that need not be repeated. All blocks with the same name are rendered only once (actually, block with the same name as an existing block are ignored, so make sure that you only use a particular name for one script block. )</p>

<p>Then in the Layout, we just add the lines:</p>

<pre class="csharpcode"><span class="kwrd">&lt;</span><span class="html">html</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">head</span><span class="kwrd">&gt;</span>
@Script.InsertCss();
<span class="kwrd">&lt;/</span><span class="html">head</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">body</span><span class="kwrd">&gt;</span>

/* other content for the page */

@Script.InsertScripts();
<span class="kwrd">&lt;/</span><span class="html">body</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">html</span><span class="kwrd">&gt;</span></pre>

<p>This will produce an output of :</p>

<pre class="csharpcode"><span class="kwrd">&lt;h</span><span class="html">tml</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">head</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">link</span> <span class="attr">rel</span><span class="kwrd">="stylesheet"</span> <span class="attr">type</span><span class="kwrd">="text/css"</span> <span class="attr">href</span><span class="kwrd">="/content/css/ui.base.css"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">link</span> <span class="attr">rel</span><span class="kwrd">="stylesheet"</span> <span class="attr">type</span><span class="kwrd">="text/css"</span> <span class="attr">href</span><span class="kwrd">="/content/css/ui.core.css"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">link</span> <span class="attr">rel</span><span class="kwrd">="stylesheet"</span> <span class="attr">type</span><span class="kwrd">="text/css"</span> <span class="attr">href</span><span class="kwrd">="/content/css/ui.slider.css"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;/</span><span class="html">head</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">body</span><span class="kwrd">&gt;</span>
/* other content for the page */
<span class="kwrd">&lt;</span><span class="html">script</span> <span class="attr">type</span><span class="kwrd">="text/javascript"</span> <span class="attr">src</span><span class="kwrd">="/Scripts/jquery-1.6.1.min.js"</span><span class="kwrd">&gt;&lt;/</span><span class="html">script</span><span class="kwrd">&gt;</span>
&lt;script type=<span class="str">"text/javascript"</span> src=<span class="str">"/Scripts/ui/jquery.ui.core.js"</span>&gt;&lt;/script&gt;
&lt;script type=<span class="str">"text/javascript"</span> src=<span class="str">"/Scripts/ui/jquery.ui.widget.js"</span>&gt;&lt;/script&gt;
&lt;script type=<span class="str">"text/javascript"</span> src=<span class="str">"/Scripts/ui/jquery.ui.mouse.js"</span>&gt;&lt;/script&gt;
&lt;script type=<span class="str">"text/javascript"</span> src=<span class="str">"/Scripts/ui/jquery.ui.slider.js"</span>&gt;&lt;/script&gt;
&lt;script type=<span class="str">"text/javascript"</span>&gt;
<span class="rem">//&lt;![CDATA[</span>
<span class="kwrd">function</span> HideSlider(id) { $(id).Hide();}
HideSlider(<span class="str">'#sl12321'</span>);
HideSlider(<span class="str">'#sl14315'</span>);
HideSlider(<span class="str">'#sl68953'</span>);
<span class="rem">//]]&gt;</span>
<span class="kwrd">&lt;/</span><span class="html">script</span><span class="kwrd">&gt;&lt;/</span><span class="html">body</span><span class="kwrd">&gt;</span>
<span class="kwrd">&lt;</span><span class="html">html</span><span class="kwrd">&gt;</span></pre>

<p><b>Setup:</b></p>

<p>Creating a Script object.</p>

<p>The “Script” object needs to be created differently depending on where it is being used. (You can, of course, call it anything you like. I use “Script” with a capital S, so that it corresponds to @Html).</p>

<p>When used in a view, create it like this:</p>

<pre class="csharpcode">@{ var Script = StateTheaterMvc.Component.JavascriptHelper.Create(<span class="kwrd">this</span>); }</pre>

<p>When used inside a @helper method or an HtmlHelper extension method:</p>

<pre class="csharpcode">var script = StateTheaterMvc.Component.JavascriptHelper.Create(WebPageContext.Current);</pre>

<p><b>Configuration XML file</b></p>

<p>Finally, we reach the part which is at the heart of the JavascriptHelper, the jslibraries.xml file. It’s a bit complex, but the default I’ve set up is probably good enough, with perhaps only a few minor tweaks, and once fully configured to your taste, you could easily standardize on that for your whole enterprise. It is, however, simple enough that a NuGet package could be updated it when being added to a project. And the good news is that I’ve also create a XSD schema file for it, so you can have IntelliSense to help you along.</p>

<p>The root element is &lt;libraries&gt; and it has three optional attributes:</p>

<p>“localjspath=” which gives the relative location of the folder where you put your js files. The default is “~/Scripts” which is the ASP.NET MVC default.</p>

<p>“selfJsPath=” which gives the relative location of the folder under which you put your view-specific js files. They follow the same structure as view files, so if you request the js file for /Home/Index, the view file would be /Views/Home/Index.cshtml, and the js file used what be /Scripts/Views/Home/Index.js. Defaults to “~/Script/Views”</p>

<p>“useDebugScripts=” when set to “true” forces use of debug versions of js files when available. Defaults to false. Debug scripts are always used when a debugger is attached.</p>

<pre class="csharpcode"><span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="jquery"</span> <span class="attr">version</span><span class="kwrd">="1.6.2"</span> <span class="attr">useGoogle</span><span class="kwrd">="false"</span> 
  <span class="attr">pathname</span><span class="kwrd">="jquery-1.6.1.min.js"</span> <span class="attr">debugPath</span><span class="kwrd">="jquery-1.6.1.js"</span> <span class="kwrd">/&gt;</span></pre>

<p>Within the &lt;libraries&gt; element, there are a collection of &lt;library&gt; elements – one for each standard javascript file you’ll be using-- which have two required and several optional attributes:</p>

<p>“name=” which gives a unique name for a particular javascript file. It is required and is used by the helper and throughout the xml file to refer to that file.</p>

<p>“pathname=” which gives the path, relative to the localjspath described above, and filename of this file. This can also be a fully-qualified URL if it’s a remote file. It is required. (unless useGoogle</p>

<p>“debugPath=” which, like the pathname attribute, give the relative path to a javascript file to be used when debugging. This is optional and defaults to the value given for the pathname. The idea here is the you set the debugPath to the full, commented version, and the pathname to the mini-fied version, and the helper figures out which to use.</p>

<p>“dependsOn” which is a list of comma separated name of file, which must be loaded before this one. This is the most powerful feature, but also the most confusing, so we’ll expand upon it in a moment. It is optional and defaults to no dependencies</p>

<p>“alias=” which is a list of comma separated alternate names this file could be identified as. I added this because I could never remember if it’s “accordion” or “accordian”. This is optional and defaults to no aliases.</p>

<p>“css=” which gives the name of the css element (see below) associated with this file.</p>

<p>“useGoogle=” which, when set to true, will generate a request to the googleapi.com CDN to load the file. Optional, defaults to false. If true, “version” attribute is required.</p>

<p>“version=” is used only when “useGoogle” is true. Gives the version of the file to load from Google.</p>

<p>Also with the &lt;libraries&gt; element (at the same level as the &lt;library&gt; elements), there is the &lt;css&gt; group.</p>

<p>The &lt;css&gt; element has only one attribute:</p>

<p>“localcsspaath” which gives the relative location of the folder where you put your CSS files. The default is “~/Content” which is the ASP.NET MVC default.</p>

<p>Within the &lt;css&gt; element are multiple &lt;sheet&gt; element, one for each css file associated with a js file. They have two required attributes:</p>

<p>“name=” Unique name used to identified the particular css file. Matches name given in the “css” attribute in the &lt;library&gt; element above. Can be the same as the name used for the js file in the &lt;library&gt; element.</p>

<p>“pathname=” which gives the path, relative to the localcsspath described above, and filename of this file.</p>

<p><b>Example:</b></p>

<pre class="csharpcode"><span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="jquery"</span> <span class="attr">version</span><span class="kwrd">="1.6.1"</span> <span class="attr">useGoogle</span><span class="kwrd">="false"</span> <span class="attr">pathname</span><span class="kwrd">="jquery-1.6.1.min.js"</span>
     <span class="attr">debugPath</span><span class="kwrd">="jquery-1.6.1.js"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="uicore"</span> <span class="attr">dependsOn</span><span class="kwrd">="jquery"</span> <span class="attr">pathname</span><span class="kwrd">="ui/jquery.ui.core.js"</span><span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="uiwidget"</span> <span class="attr">dependsOn</span><span class="kwrd">="uicore"</span> <span class="attr">pathname</span><span class="kwrd">="ui/jquery.ui.widget.js"</span><span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="mouse"</span> <span class="attr">dependsOn</span><span class="kwrd">="uiwidget"</span> <span class="attr">pathname</span><span class="kwrd">="ui/jquery.ui.mouse.js"</span><span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="datepicker"</span> <span class="attr">dependsOn</span><span class="kwrd">="uiwidget"</span>
    <span class="attr">pathname</span><span class="kwrd">="ui/jquery.ui.datepicker.js"</span> <span class="attr">css</span><span class="kwrd">="ui"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;</span><span class="html">library</span> <span class="attr">name</span><span class="kwrd">="slider"</span> <span class="attr">dependsOn</span><span class="kwrd">="uiwidget,mouse"</span> 
    <span class="attr">pathname</span><span class="kwrd">="ui/jquery.ui.slider.js"</span> <span class="attr">css</span><span class="kwrd">="ui"</span> <span class="kwrd">/&gt;</span></pre>

<p>Starting with the first element, we specified a JavaScript file, which we’ll be calling “jquery”.   When we ask for “jquery”, we’ll normally get “jquery-1.6.1.min.js” , unless we’re debugging, in which case, it will load “jquery-1.6.1.js”.  It will load these from the website, but, if, as I put it into production, I flip the useGoogle file to true, then it will load it from <a href="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.js">http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.js</a>  </p>

<p>(Note that the “name” value is used to the URL here.  This is the only place where the actual name used is significant.  Otherwise it can just be any unique string.  This is wrong, and will be fixed in some future release).</p>

<p>Next we want to define the complete componentized version of jQuery UI, so that we can load just the parts we want. First, we declare the UI core as depending on jQuery.  Then we declare widget component as depending on the core.  And the mouse component as depending on the widgets.</p>

<p>Then we can the individual components themselves.  “datepicker” as depending on the widgets; “slider” as depending on the widget &amp; the mouse components.  Since the mouse already depends on widget, it was really necessary to specify both here, but there’s no harm.  So, if you request to use “slider” on your page, then the helper makes sure that jQuery, the UI core, UI Widgets, mouse &amp; slider components are all included, in the proper order.  (Note, there is no checking for circular dependencies, so be careful how you specify the dependsOn attribute)</p>

<p>Both are also associated with the “ui” css file, which brings us to that section:</p>

<pre class="csharpcode"><span class="kwrd">&lt;</span><span class="html">css</span> <span class="attr">localcsspath</span><span class="kwrd">="~/content/css/Views"</span><span class="kwrd">&gt;</span>
   <span class="kwrd">&lt;</span><span class="html">sheet</span> <span class="attr">name</span><span class="kwrd">="ui"</span> <span class="attr">pathname</span><span class="kwrd">="ui/themes/Redmond/jquery-ui-1.8.13.custom.css"</span> <span class="kwrd">/&gt;</span>
   <span class="kwrd">&lt;</span><span class="html">sheet</span> <span class="attr">name</span><span class="kwrd">="cluetip"</span> <span class="attr">pathname</span><span class="kwrd">="jquery.cluetip.css"</span> <span class="kwrd">/&gt;</span>
<span class="kwrd">&lt;/</span><span class="html">css</span><span class="kwrd">&gt;</span></pre>

<p>Both of the jQuery UI components above give “ui” as their CSS file (the theme roller doesn’t create individual CSS files) so if either (or both) is used, that file is included.  CSS files don’t have a separate dependency tree, but all associated CSS files for every dependent js file up the tree are included, so if you wanted to use the separate jQuery UI CSS files, you’d associate  “ui.core.css” with uicode”, “ui.base.css” with “uiwidget” and each component with its own CSS file, and all the needed files will be included.</p>

<p><b>API</b></p>

<p>The API is fairly simple:</p>

<p><font face="Consolas">Std(string)</font> - accepts a comma-separated list of script file ids.</p>

<p><font face="Consolas">AddScript(string id, string script)</font> - accepts a block of script as text. Multiple calls with the same id are rendered only once.</p>

<p><font face="Consolas">AddScript(string script) </font>- accepts a block of script as text. Each call is rendered.</p>

<p>All script from either AddScript methods is rendered in a block at the script insertion point.</p>

<p><font face="Consolas">AddOnReadyScript(string script) </font>- accepts a block of script as text. Appended to script run on page ready. All on rendered, wrapped in a jQuery document ready event function, at the point of InsertOnReady()</p>

<p><font face="Consolas">InsertScripts() </font>-- Renders all script files &amp; script blocks. </p>

<p><font face="Consolas">InsertOnReady()</font> -- Renders all script block intended for startup, wrapped in a jQuery on ready function.</p>

<p><font face="Consolas">InsertCss() </font>-- Renders all css files</p>

<p><b>What’s the next step?</b></p>

<p>Now that I’ve made this public, it’s really just a beta. I think there’s still a bit more to be done before it’s ready to be “Production-Ready v1.0”. And I need some feedback….</p>

<ul>
  <li>First of all, how exactly should it be packaged? I’ve been just added to source file to my project, which is simple, but not very elegant. The alternative would be to create an assembly for it, but it’s just one file, so that seem like overkill. I’d really like to create a NuGet package for this, but that question needs to be settled first. </li>

  <li>Or do we think bigger? Microsoft has open-sourced the MVC framework, and is <a target="_blank" href="http://haacked.com/archive/2012/03/29/asp-net-mvc-now-accepting-pull-requests.aspx">now accepting pull-requests</a>.  Should be deeply embedded into the MVC eco-system? </li>

  <li>Also, how is the API? Are the method names sufficiently intuitive? </li>

  <li>Is an XML file the proper way to store the dependency information? </li>

  <li>Is there any feature that really needs to be added? </li>
</ul>

<p><b>The code:</b></p>

<p>The source code is available (under the Apache license) from my GitHub library:</p>

<p><a href="http://github.com/jamescurran/JavascriptHelper">http://github.com/jamescurran/JavascriptHelper</a></p>
<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2012%2f03%2f29%2fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx"><img border="0" alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%253a%252f%252fhonestillusion.com%252fblogs%252fblog_0%252farchive%252f2012%252f03%252f29%252fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx" /></a><div class="wlWriterHeaderFooter" style="margin:0px;padding:0px 0px 0px 0px;"><div class="shoutIt"><a href="http://dotnetshoutout.com/Submit?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2012%2f03%2f29%2fjavascripthelper-managing-js-files-for-asp-net-mvc.aspx&amp;title=JavascriptHelper%e2%80%93Managing+JS+files+for+ASP.NET+MVC"><img alt="Shout it" style="border:0px;" src="http://dotnetshoutout.com/image.axd?url=http://honestillusion.com/blogs/blog_0/archive/2012/03/29/javascripthelper-managing-js-files-for-asp-net-mvc.aspx" /></a></div></div>