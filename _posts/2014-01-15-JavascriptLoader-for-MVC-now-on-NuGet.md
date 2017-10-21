---
layout: post
title: JavascriptLoader for MVC now on NuGet
tags: code c# .net javascript programming dotnet csharp aspnetmvc codeproject
---

When I last wrote of JavascriptLoader, I was still calling it `JavascriptHelper` and to use it, you had to include a C# source file in your project.

Well, it's grown a bit since then.  First, the new, more appropriate name, and now it's easily installable via NuGet (under the id "`JavascriptLoader`" but as a first-time installer, you'll need to get "`JavascriptLoader.Initial`" also)   

Plus there are a few new features, and I suspect there are people hearing about `JavascriptLoader` for the first time, so let's recap.

One of the biggest hassles using ASP.NET MVC is managing Javascript files.  With jQuery & Knockout heavily used, and both with a large collection of add-ons (in separate JS files) and with each add-on often coming with it's own needed CSS file, things get a bit messy. Then since many embed the version name in the file's name, updating your source code when a new version comes up is a large hunt-and-replace project. Then throw in bundling & compression, and you pretty much have to include every js file on every page (kind-of defeating the purpose of compressing it in the first place), and things get really ugly.

Wouldn't it be great if there was some labor-saving device that could make that simple?

Using JavascriptLoader is quite easy.  

At the top of your layout, add:

	@{
	    var JScript = NovelTheory.Components.JavascriptLoader.Create(this);
	    JScript.Include("jquery, moderizr");
	}

Then, where you want the CSS files, remove the line:
`@Styles.Render("~/Content/css")`

and add:  
`@JScript.InsertCss()`

Where you want the JS files, remove the lines 
 
	@Scripts.Render("~/bundles/jquery")
	@RenderSection("scripts", required: false)

add:

	@JScript.InsertScripts()

Then in you views, add (for example)

	@{
	    var JScript = NovelTheory.Components.JavascriptLoader.Create(this);
	    JScript.Std("menu, slider, dialog, self");
	}

"`self`" refers to a JS file following the same naming conventions as the view template, under the Scripts folder
e.g., `/Scripts/View/Home/Index.js`

The other names ("`menu`", "`slider`" etc) refer to JS files defined in the jslibraries.xml in the website root.

The `~/jslibraries.xml` file needs to be manually updated for the JS files you are using, but once you get it the way you like, you can copy it from project to project.  (A basic one, covering the files used in Wizard-created projects is included).

JavascriptLoader will automatically include the .js file, and any .js file it's dependent on,  so if you need Knockout Mapping, you ask for "`mapping`", and it gives you `ko.mapping-latest.js` *and* also `knockout-3.0.0.js`.  And when v3.1 comes out, just update it once in the jslibraries file, and the new version is used everywhere. (I look forward to a world where authors include updating the jslibraries file as part of their NuGet package.)

Plus, if any of these files you asked for needs a CSS file, it's included too (because you always forget to add them, right?)

What to always use the full versions of scripts during development, then switch to the minified versions for production?  Change one setting in the jslibraries.xml file.  How 'bout turning on MVC's bundling & minification -- yep, change just one setting in the xml file.


More detailed (if a bit outdated) information is available on the [CodeProject article](http://www.codeproject.com/Articles/395143/JavascriptHelper-Managing-JS-files-for-ASP-NET-MVC) I wrote on it.


### The code:
  
  The source code is available (under the Apache license) from my GitHub library:
  [http://github.com/jamescurran/JavascriptHelper](http://github.com/jamescurran/JavascriptHelper)   
