---
layout: post
title: A ViewComponent extension for Castle MonoRail
categories: code c# .net programming dotnet csharp castle monorail
tags: code c# .net programming dotnet csharp castle monorail
---

  <p>I’ve been rewriting my website, njtheater.com, (very slowly) as a Castle MonoRail application.  Along the way, I’ve written a number of ViewComponent and other elements.  Many of these were of general use, so I’ve added them to the CastleContib project, and documented them in the using.castleproject.org wiki.</p>  <p>Two problem there: First, some of the items I wrote don’t fit into an exist category in CastleContrib (There’s one for ViewComponents, which I’ve stick a filter into, but putting a Controller base class there seemed wrong).  Second, CastleContrib &amp; using.castleproject.org seem to be somewhat of a black hole.  No one seems to look there for information about the Castle Project (which is kind of a shame, since that’s exactly it’s purpose).  </p>  <p>On the other hand, blogs posts about Castle are turning up everywhere.  We’ve even now got an <a href="http://pipes.yahoo.com/pipes/pipe.run?_id=bGjr2c1s3hGi5qx20EypaA&amp;_render=rss&amp;limit=200" target="_blank">aggregated blog feed specific to Castle</a>.  So, I figured, I start using my blog to talk about what I’ve written.</p>  <p>In fact, one article I discovered on that aggregator was Andy Pike’s “Integrating Gravatar with Castle MonoRail” inwhich he discusses a Helper object for Monorail which creates Gravatars for use’s email addresses.  It was written last January.  The only thing is, I’ve written (and added to CastleContrib) a Gravatar component three months earlier.  That was going to be the topic of my first MonoRail blog post (and I will be my second), but first, I figure I should talk about the base class I once for all my ViewComponents, which I’ve given the rather imaginative name of ViewComponentEx.</p>  <p>ViewComponentEx derives from ViewComponent, and can be used as a “drop-in” replacement for it, as the base class for your ViewComponents.  It provides a number of simple methods to help building ViewComponents.</p>  <pre class="c#"><font size="4">void ConfirmSectionPresent(string section);</font></pre>

<blockquote>
  <p>Throws an exception if the given section is not present. </p>
</blockquote>

<pre class="c#"><font size="4">string GetSectionText(string section);</font></pre>

<blockquote>
  <p>Get the text of a section as a string.</p>
</blockquote>

<pre class="c#"><font size="4">string GetBodyText();</font></pre>

<blockquote>
  <p>Get the text of the body of a block component (without section)</p>
</blockquote>

<pre class="c#"><font size="4">void RenderTextFormat(string format, params object[] args);</font></pre>

<blockquote>
  <p>Renders the text, formatted. Just like String.Format() </p>
</blockquote>

<pre class="c#"><font size="4">string GetParamValue(string key, string defaultValue);</font></pre>

<pre class="c#"><font size="4">bool GetParamValue(string key, bool defaultValue);</font></pre>

<pre class="c#"><font size="4">E GetParamValue(string key, E defaultValue) where E : struct;</font></pre>

<blockquote>
  <p>Gets a parameter value, with a default. Overloaded to handle string, boolean, or Enum value. </p>
</blockquote>

<pre class="c#"><font size="4">Castle.Core.Logging.ILogger Logger { get; set; }</font></pre>

<blockquote>
  <p>A property for the system Logger. Automatically wired by Windsor, if active and a Logger is defined in the container. Default to NullLogger, otherwise.</p>
</blockquote>

<pre class="c#"><font size="4">string MakeUniqueId(string prefix);</font></pre>

<blockquote>
  <p>Makes an unique id. The given prefix is prepended to the generated number. The ID isn't actually guaranteed to be unique (which would require using all 32 digits of the guid). But this produce ids sufficiently distinctive to generate multiple controls on a page.</p>
</blockquote>

<p> </p>

<p>Code available here: <a href="http://honestillusion.com/files/folders/castle/entry7880.aspx" target="_blank">ViewComponentex.cs</a></p>