---
layout: post
title: A ViewComponent extension for Castle MonoRail, Part II
categories: code c# .net programming dotnet csharp castle monorail
tags: code c# .net programming dotnet csharp castle monorail
---

  <P>This was intended to be a two-part article.  It was just after I published <A href="http://honestillusion.com/blogs/blog_0/archive/2009/08/24/a-viewcomponent-extension-for-castle-monorail.aspx" target="_blank">the original article,</A> I noticed that I'd left out a large part of ViewComponentEx. We continue.....</P>
<HR />
<PRE class="c#"><FONT size="4">        protected bool RenderOptionalSection(string section) 
        protected bool RenderOptionalSection(string section, string defaultText)</FONT> </PRE>
<P>Renders the named section of a block component - if the section is present.  If not, it just silently returns.   The second overload lets you provide some text to be rendered, if that section isn't given:</P>
<BLOCKQUOTE>
<P><FONT size="3">RenderOptionalSection("tablestart", "&lt;table&gt;")</FONT> </P></BLOCKQUOTE>
<P>Returns true if this section was rendered; false, if the section was present.  This seems like a very simple method (and it is), but if your component has a number of different sections for styling (such as the SmartGridViewComponent, which has 18!), this can do wonders to streamline your code.</P>
<HR />
<PRE class="c#"><FONT size="4">        void RenderComponent&lt;VC&gt;(params string[] componentParams) where VC : ViewComponentEx, new();
        void RenderComponent&lt;VC&gt;(IDictionary componentParams) where VC : ViewComponentEx, new();
        void RenderComponent(ViewComponentEx component, params string[] componentParams); 
        void RenderComponent(ViewComponentEx component, IDictionary componentParams); </FONT></PRE>
<P>This implement, with a slightly different syntax, a technique originally devised by Joey Beninghove.  The idea is to make a ViewComponent which is composite of several other VCs.  The basic syntax is </P><PRE class="c#"><FONT size="4"> RenderComponent&lt;LinkSubmitButtonComponent&gt;("linkText=Search",
             string.Format("formToSubmit={0}", searchFormName));</FONT></PRE>
<P>However, the various overloads allow using an already exist component object, and/or an already built dictionary of options.</P>
<HR />

<P>Also include in the source file is the class ViewComponentUsingSiteMap which, like ViewComponentEx, is an abstract base use for creating ViewComponents, but I'll hold off discussing that until I ready to talk about the VCs the derive from it.</P>