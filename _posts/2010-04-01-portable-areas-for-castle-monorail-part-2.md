---
layout: post
title: Portable Areas for Castle Monorail, Part 2
categories: code c# .net programming dotnet csharp castle monorail
tags: code c# .net programming dotnet csharp castle monorail
---

In our last episode, we discussed the PortableAreaController base class, which makes it simple to create a portable area using Monorail.  In this installment, we put that class to use.

For the purposes of this example, the controller isn’t going to do anything useful – I’m not even going to bother with code in the actions.  The different actions will display different views so you can see the effect, and I’m hoping you’ll just trust that you can put real code in the action methods.

<PRE class="csharpcode">[Layout(<SPAN class="str">"default"</SPAN>,<SPAN class="str">"patest"</SPAN>)]
<SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">class</SPAN> PATest : PortableAreaController
{
    <SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">void</SPAN> Page1()
    {         }
    <SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">void</SPAN> Page2()
    {         }
    <SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">void</SPAN> Page3()
    {         }
}</PRE>


That’s our minimalistic example.  If it looks a lot like an ordinary Monorail controller, that’s the idea.    “PATest” is the name of the controller, which in the case of a Portable Area, is the name of the entire feature, so it’s be something like CoolWiki or MyForum. 

The Action methods are identical to what you’d have in a normal controller.  In fact, everything in the portable controller is just like it would be if this were a intrinsic part of a website.  

The rest is just about putting the pieces together in the assembly. 
[![PortableAreaSolution](/images/PortableAreaSolution_thumb_6E5AFF84.png)](/image/PortableAreaSolution_7652A1E6.png)

Individual files  (images, css files etc) should be placed in the root level of the project.  When referencing then in a view, just added “.rails” to the end (`<img src=”${siteRoot}/PATest/autolist.png.rails” />`)

View templates should be in a folder named after the controller, and layouts in a “layouts” folder --- just as if the root level was the website’s “Views” folder.  You don;t have to do anything special to get Monorail to find them – that is handled for you by the base class.

All those files (not the CS files obviously) should have their Build Action set to Embedded Resource.

Note that when a view template is needed, Monorail will first look for a physical file in the website’s Views files, and failing there, search in the assembly.  This allows the end user to override the embedded view templates with their own.   In the example I’ve uploaded to the CastleContrib SVN repository, I’ve locally overridden Page2.

The same holds for layouts, which I’ve put to use in the example.  I assumed that most sites will have a default layout for the site (which I further assumed is called “default”).  The controller uses that layout and adds it’s own child layout.  (The project should also define a simple default layout as just “${childContent}” just in case the website it’s being used in doesn’t define one.)

The PortableAreaController base class has been added to the ViewComponent project in the CastleContrib SVN repository. (Yes, I know it’s not a ViewComponent, but I didn’t want to create a whole new branch -- particularly not for just one file-- and ViewComponents were the closest available existing project).  The PATest example shown here is a separate project under that branch, with the resulting Castle.MonoRail.PortableAreaExample.dll assembly added to the ViewComponent.TestSite project.  Note that that assembly is the *only* file added to the website to enable this functionality, and the only other change was to add a reference to it into the web.config.
