---
layout: post
title: Portable Areas for Castle Monorail
tags: code c# .net programming dotnet csharp castle monorail
---

Recently I had read a blogger comparing Castle Monorail with ASP.NET MVC.  He chose ASP.NET mainly because it supported Portable Areas while Monorail did not.  As a supporter of Monorail, I was very offended by this, and decided to correct the problem.  The first step... finding out exactly what a "portable area" was.

A Portable Area is a piece of functionality, with all pieces bundled up into a single assembly, which can be just "dropped into" an existing website.  Think of a forum or a wiki.  The idea is basically, I could just add wiki.dll to my site, and suddenly `www.mysite.com/wiki` just works.

Now, having a controller and its actions together in a single assembly is a basic design principle of Monorail (and essentially all of MVC design), so that part was done before I even began.  The rest was a bit trickier.

I would have to store - and retrieve at the right times, the view template files, and all other associated files - images, style sheets, script files, etc.

Storing the files was in itself trivial -- .NET has a means built right it. The file just need to be added to the project as embedded resources.  In Visual Studio, on the file's property panel, set the "Build Action" to "Embedded Resource".

Now that the files were in the assembly, we had to get them out of it. For views, which I had assumed were going to be the most trouble, this turned out to be simple, with the plumbing for it was already built into the Monorail framework.  Apparently, someone had planned for portable areas, but never followed through.  View templates are loaded by a class aptly called the FileAssemblyViewSourceLoader.  It looks in the "Views" folder for the template, and failing there, looks among the embedded resources of the assemblies in its collection.  However, nothing in the framework ever added an assembly to that collection, so this feature has always laid fallow.

The final feature was getting it to load a random file from the resources.  Whenever I need to do something with a name not known in advance, a DefaultAction seems like the way to go.

With that, all the pieces were in place, I just had a wee bit of code to tie them all together.  Defining a base class which a portable area could be derived from seemed the best approach.

<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">class</span> PortableAreaController : SmartDispatcherController</pre>
<p>{</p>

The first step was to override the Initialize() method, so that I could add this assembly to the list searched by FileAssemblyViewSourceLoader.  The only trick here is we must make sure that the assembly is only added once, regardless of how many times the controller is initialized.    And, while we are at it, we'll also get the assembly name and a list of the resources and save them for later.

<script src="https://gist.github.com/jamescurran/5493755.js">    </script>

Next is the DefaultAction.  The idea here is that you request somefile.jpg.rails, and we pull somefile.jpg out of the resources, and stream it to the browser.  In Monorail, a DefaultAction is a method which is called when no other method in the controller matches the action.  In our DefaultAction, we'll generate a resource name from the assembly name and the Action name.  The Action is basically, the name of the "file" requested without the ".rails" extension, so if we ask for file "`http://mysite.com/portable/myimage.gif.rails`", then the pseudo-file we are requesting is "myimage.gif.rails" which makes the Action "myimage.gif" which just happens to be the file we really want.  The only tricky part here is the GetContentTypeFromExt() function.  The problem is that there is a very simple way to do this --- which only works under Windows.  Now, while the vast majority of web servers running Monorail are Windows based, Monorail is designed to also run under Mono (Linux).   I couldn't find a good portable way to handle this, so I just punted (check the source for dirty secrets).

<script src="https://gist.github.com/jamescurran/5493750.js">    </script>

This method is the controller's Default action by virtue of the `[DefaultAction]` attribute.  The name is arbitrary - DefaultAction just kept things simple.

And with that, everything you need to write a portable area in Monorail is neatly contained in a simple base class.  Everything we've just gone over, you can now completely ignore.   

Next, we discuss how you use this base class to write your own portable area.
