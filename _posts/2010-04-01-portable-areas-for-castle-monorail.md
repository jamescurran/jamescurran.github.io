---
layout: default
title: Portable Areas for Castle Monorail
categories: code c# .net programming dotnet csharp castle monorail
tags: code c# .net programming dotnet csharp castle monorail
---

  
<p>Recently I had read a blogger comparing Castle Monorail with ASP.NET MVC.  He chose ASP.NET mainly because it supported Portable Areas while Monorail did not.  As a supporter of Monorail, I was very offended by this, and decided to correct the problem.  The first step… finding out exactly what a “portable area” was.</p>
<p>A Portable Area is a piece of functionality, with all pieces bundled up into a single assembly, which can be just “dropped into” an existing website.  Think of a forum or a wiki.  The idea is basically, I could just add wiki.dll to my site, and suddenly <a href="http://www.mysite.com/wiki">www.mysite.com/wiki</a> just works.</p>
<p>Now, having a controller and its actions together in a single assembly is a basic design principle of Monorail (and essentially all of MVC design), so that part was done before I even began.  The rest was a bit trickier.</p>
<p>I would have to store – and retrieve at the right times, the view template files, and all other associated files – images, style sheets, script files, etc.</p>
<p>Storing the files was in itself trivial -- .NET has a means built right it. The file just need to be added to the project as embedded resources.  In Visual Studio, on the file’s property panel, set the “Build  Action” to “Embedded Resource”.</p>
<p>Now that the files were in the assembly, we had to get them out of it. For views, which I had assumed were going to be the most trouble, this turned out to be simple, with the plumbing for it was already built into the Monorail framework.  Apparently, someone had planned for portable areas, but never followed through.  View templates are loaded by a class aptly called the FileAssemblyViewSourceLoader.  It looks in the “Views” folder for the template, and failing there, looks among the embedded resources of the assemblies in its collection.  However, nothing in the framework ever added an assembly to that collection, so this feature has always laid fallow.</p>
<p>The final feature was getting it to load a random file from the resources.  Whenever I need to do something with a name not known in advance, a DefaultAction seems like the way to go.</p>
<p>With that, all the pieces were in place, I just had a wee bit of code to tie them all together.  Defining a base class which a portable area could be derived from seemed the best approach.</p>
<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">class</span> PortableAreaController : SmartDispatcherController</pre>
<p>{</p>
<p>The first step was to override the Initialize() method, so that I could add this assembly to the list searched by FileAssemblyViewSourceLoader.  The only trick here is we must make sure that the assembly is only added once, regardless of how many times the controller is initialized.    And, while we are at it, we’ll also get the assembly name and a list of the resources and save them for later.</p>
<pre class="csharpcode">
  <span class="kwrd">private</span> <span class="kwrd">string</span>[] resourceNames;
<span class="kwrd">private</span> <span class="kwrd">string</span> asmName;

<span class="kwrd">public</span> <span class="kwrd">override</span> <span class="kwrd">void</span> Initialize()
{
    <span class="kwrd">base</span>.Initialize();
    var asm = <span class="kwrd">this</span>.GetType().Assembly;
    resourceNames = asm.GetManifestResourceNames();
    asmName = asm.GetName().Name;
    var asminfo = <span class="kwrd">new</span> AssemblySourceInfo(asm, asmName.ToLower());
    <span class="kwrd">if</span> (!<span class="kwrd">this</span>.Context.Services.ViewSourceLoader.AssemblySources.Cast&lt;AssemblySourceInfo&gt;()<br />                            .Any(asi=&gt;asi.AssemblyName==asminfo.AssemblyName))
        <span class="kwrd">this</span>.Context.Services.ViewSourceLoader.AddAssemblySource(asminfo);
}</pre>
<p>Next is the DefaultAction.  The idea here is that you request somefile.jpg.rails, and we pull somefile.jpg out of the resources, and stream it to the browser.  In Monorail, a DefaultAction is a method which is called when no other method in the controller matches the action.  In our DefaultAction, we’ll generate a resource name from the assembly name and the Action name.  The Action is basically, the name of the “file” requested without the “.rails” extension, so if we ask for file “http://mysite.com/portable/myimage.gif.rails”, then the pseudo-file we are requesting is “myimage.gif.rails” which makes the Action “myimage.gif” which just happens to be the file we really want.  The only tricky part here is the GetContentTypeFromExt() function.  The problem is that there is a very simple way to do this --- which only works under Windows.  Now, while the vast majority of web servers running Monorail are Windows based, Monorail is designed to also run under Mono (Linux).   I couldn’t find a good portable way to handle this, so I just punted (check the source for dirty secrets).</p>
<p>[DefaultAction] 
  <br /><span class="kwrd">public</span> <span class="kwrd">void</span> DefaultAction() 

  <br />{ 

  <br />    <span class="kwrd">string</span> filename = asmName + <span class="str">"."</span> + Action; 

  <br />    var resourceName = resourceNames.FirstOrDefault(rn=&gt; rn.Equals(filename,StringComparison.InvariantCultureIgnoreCase)); 

  <br />    <span class="kwrd">if</span> (resourceName!= <span class="kwrd">null</span>) 

  <br />    { 

  <br />        <span class="kwrd">string</span> ext = Path.GetExtension(filename); 

  <br />        <span class="kwrd">this</span>.Response.ContentType = GetContentTypeFromExt(ext); 

  <br />

  <br />        Stream contents = <span class="kwrd">this</span>.GetType().Assembly.GetManifestResourceStream(resourceName); 

  <br />        <span class="kwrd">this</span>.Response.BinaryWrite(contents); 

  <br />        CancelView(); 

  <br />    } 

  <br />} 

  <br /></p>
<p>This method is the controller’s Default action by virtue of the [DefaultAction] attribute.  The name is arbitrary – DefaultAction just kept things simple.</p>
<p>And with that, everything you need to write a portable area in Monorail is neatly contained in a simple base class.  Everything we’ve just gone over, you can now completely ignore.   </p>
<p>Next, we discuss how you use this base class to write your own portable area.</p>