---
layout: post
title: A ViewComponent extension for Castle MonoRail
categories: code c# .net programming dotnet csharp castle monorail
tags: code c# .net programming dotnet csharp castle monorail
---

  I've been rewriting my website, njtheater.com, (very slowly) as a Castle MonoRail application.  Along the way, I've written a number of ViewComponents and other elements.  Many of these were of general use, so I've added them to the CastleContib project, and documented them in the using.castleproject.org wiki.
  
  Two problem there: First, some of the items I wrote don't fit into an exist category in CastleContrib (There's one for ViewComponents, which I've stick a filter into, but putting a Controller base class there seemed wrong).  Second, CastleContrib &amp; using.castleproject.org seem to be somewhat of a black hole.  No one seems to look there for information about the Castle Project (which is kind of a shame, since that's exactly it's purpose). 
  
On the other hand, blogs posts about Castle are turning up everywhere.  We've even now got an [aggregated blog feed specific to Castle](http://pipes.yahoo.com/pipes/pipe.run?_id=bGjr2c1s3hGi5qx20EypaA&_render=rss&limit=200).  So, I figured, I'll start using my blog to talk about what I've written.
  
In fact, one article I discovered on that aggregator was Andy Pike's "Integrating Gravatar with Castle MonoRail" inwhich he discusses a Helper object for Monorail which creates Gravatars for user's email addresses.  It was written last January.  The only thing is, I've written (and added to CastleContrib) a Gravatar component three months earlier.  That was going to be the topic of my first MonoRail blog post (and I will be my second), but first, I figure I should talk about the base class I use for all my ViewComponents, which I've given the rather imaginative name of ViewComponentEx.

ViewComponentEx derives from ViewComponent, and can be used as a "drop-in" replacement for it, as the base class for your ViewComponents.  It provides a number of simple methods to help building ViewComponents.

**void ConfirmSectionPresent(string section);**

>Throws an exception if the given section is not present.

**string GetSectionText(string section);**

>Get the text of a section as a string.

**string GetBodyText();**

>Get the text of the body of a block component (without section)</p>

**void RenderTextFormat(string format, params object[] args);**

>Renders the text, formatted. Just like String.Format() 

**string GetParamValue(string key, string defaultValue);**

**bool GetParamValue(string key, bool defaultValue);**

**E GetParamValue(string key, E defaultValue) where E : struct;**

>Gets a parameter value, with a default. Overloaded to handle string, boolean, or Enum value.

**Castle.Core.Logging.ILogger Logger { get; set; }**

>A property for the system Logger. Automatically wired by Windsor, if active and a Logger is defined in the container. Default to NullLogger, otherwise.

**string MakeUniqueId(string prefix);**

>Makes an unique id. The given prefix is prepended to the generated number. The ID isn't actually guaranteed to be unique (which would require using all 32 digits of the guid). But this produce ids sufficiently distinctive to generate multiple controls on a page.

<p>Code available here: <a href="http://honestillusion.com/files/folders/castle/entry7880.aspx" target="_blank">ViewComponentex.cs</a></p>

