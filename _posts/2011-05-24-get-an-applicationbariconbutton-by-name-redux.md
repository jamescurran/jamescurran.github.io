---
layout: default
title: Get an ApplicationBarIconButton by name (Redux)
---

  
<p>Yesterday, I read a blog post on <a href="http://www.ariankulp.com/get-an-applicationbariconbutton-by-name" target="_blank">getting an ApplicationBarIconButton by name</a>.  The author made a couple minor errors in the code, which I was going to leave a comment about, except his comment page is broken. So, another excuse to write something for my own blog.</p>
<p>The basics of the article are that in Windows Phone 7 coding, when referencing the ApplicationBar buttons, you never get an direct reference – you have to look the one you want up by name – and the author provided some code:</p>
<pre class="csharpcode">
  <span class="kwrd">private</span> ApplicationBarIconButton GetAppBarIconButton(<span class="kwrd">string</span> name)
{
    <span class="kwrd">foreach</span> (var b <span class="kwrd">in</span> ApplicationBar.Buttons)
        <span class="kwrd">if</span> (((ApplicationBarIconButton)b).Text == name)
            <span class="kwrd">return</span> (ApplicationBarIconButton)b;

    <span class="kwrd">return</span> <span class="kwrd">null</span>;
}</pre>
<p>That’s rather ugly code for a simple function.  What really bothered me was that he didn’t just cast the object – he casted it <em>twice  </em>pointlessly.</p>
<p>The author states that this is a bit uglier than you’d expect (and non-LINQ-able) because ApplicationBar.Button returns an IList, and because “the collection of buttons are of type <em>Object</em>, so you need to cast them.”   </p>
<p>Neither of those statements are exactly accurate.  The objects in the collection really are of type ApplicationBarIconButton.  They just appear to be Object types due to the effect of the IList.  Why exactly that property returns an IList instead of the more precise IList&lt;ApplicationBarIconButton&gt; is a mystery known only to the devs at Microsoft. </p>
<p>So, are we stuck with this?  No, we can improve that code.  “var” is often a very useful keyword, but you must understand what it does.  It says “declare this variable of the type of the object presented to initialize it” – which is expressly what we do <strong>not </strong>want here.  The IList is presenting the objects in the Buttons collections as Objects; but we know that they are ApplicationBarIconButtons and we want they treated like that.</p>
<pre class="csharpcode">
  <span class="kwrd">private</span> ApplicationBarIconButton GetAppBarIconButton(<span class="kwrd">string</span> name)
{
    <span class="kwrd">foreach</span> (ApplicationBarIconButton b <span class="kwrd">in</span> ApplicationBar.Buttons)
        <span class="kwrd">if</span> (b.Text == name)
            <span class="kwrd">return</span> b;

    <span class="kwrd">return</span> <span class="kwrd">null</span>;
}</pre>
<p>Much cleaner, huh?  But, let’s return to the author’s original point.  He wanted to use LINQ, but was blocked by the ILIST.   However, Microsoft realized that’s often a problem, and wrote a way around it:  The <strong>Cast&lt;T&gt;()</strong> method.   It takes a non-generic IList, and a type, and transforms it into an generic IList&lt;T&gt;.  With that, the LINQ version is trivial:</p>
<pre class="csharpcode">
  <span class="kwrd">private</span> ApplicationBarIconButton GetAppBarIconButton(<span class="kwrd">string</span> name)
{
  <span class="kwrd">return</span> ApplicationBar.Buttons.Cast&lt;ApplicationBarIconButton&gt;().FirstOrDefault(b=&gt;b.Text == name);
}</pre>