---
layout: post
title: Let's talk about some code (Javascript)
categories: code javascript programming
tags: code javascript programming
---

  
<p>Ya'know, I'd always intended this blog to be one of them cool ".Net Blogs", with just a occasional entry about my life.  Ok, so two years later, I've got the occasion random life entry thing down, but in all the time, not one thing about programming.  Let's try correcting that.</p>
<p>Lately, I've been reading  [amazon:type=Blended:search=0975240269:results=1], which is otherwise a fine book on Javscript technique. (<a href="http://www.sitepoint.com/books/jsant1/">website</a>)  However, every now &amp; then, the authors just get a bit caught up in "clever" code.  For example, this snippet from pg. 178:</p>
<p>
    <font face="Courier New">function dofade(steps, img, value, targetvisibility, otype)<br />{<br />  value+=(targetvisibility ? 1 : -1) / steps;<br />  if (targetvisibility ? value &gt; 1 : value &lt; 0)<br />        value = targetvisibility ? 1 : 0;<br /> setfade(img, value, otype);<br /> if (targetvisibility ? value &lt; 1 : value &gt; 0)<br /> {<br />    setTimeout(function()<br />    {<br />      dofade(step, img, value, targetvisibility, otype);<br />    }, 1000/ fps);<br />  }<br />}</font>
  </p>
<p>Now, the important thing to note here, is that nearly every line is affected by the value of targetvisibility, and we test for that value 4 times in just that short function.  Now, let's consider if we just tested for it once:</p>
<p>
    <font face="Courier New">function dofade(steps, img, value, targetvisibility, otype)<br />{<br />  if (targetvisibility)<br />  {<br />    value += 1 / steps;<br />    if (value &gt; 1)<br />        value = 1;<br />    setfade(img, value, otype);<br />    if (value &lt; 1)<br />    {<br />     setTimeout(function()<br />     {<br />       dofade(step, img, value, true, otype);<br />     }, 1000/ fps);<br />    }<br />  }<br />  else<br />  {<br />    value += -1 / steps;<br />    if (value &lt; 0)<br />        value = 0;<br />    setfade(img, value, otype);<br />    if (value &gt; 0)<br />    {<br />     setTimeout(function()<br />     {<br />      dofade(step, img, value, false, otype);<br />     }, 1000/ fps);<br />    }<br />  }<br />}</font>
  </p>
<p>So, now while, it's a bit longer, it's a whole bunch easier to read &amp; understand. But let's take this a step further.  How do we initially call this function?  Again, from pg. 178:</p>
<p>
    <font face="Courier New">if (dir == 'out') {dofade(steps, img, 1, false, otype);}<br />else {dofade(steps, img, 1, true, otype);}<br /></font>
  </p>
<p>The significance of this is that now, nowhere is the targetvisibility parameter not set explicitly. In which case, there's no need for it:</p>
<p>
    <font face="Courier New">if (dir == 'out') {dofadeOut(steps, img, 1, otype);}<br />else {dofadeIn(steps, img, 1, otype);}</font>
    <br />
  </p>
<p>
    <font face="Courier New">function dofadeIn(steps, img, value, otype)<br />  {<br />    value += 1 / steps;<br />    if (value &gt; 1)<br />        value = 1;<br />    setfade(img, value, otype);<br />    if (value &lt; 1)<br />    {<br />     setTimeout(function()<br />     {<br />       dofadeIn(step, img, value, otype);<br />     }, 1000/ fps);<br />    }<br />  }</font>
  </p>
<p>
    <font face="Courier New">function dofadeOut(steps, img, value, otype)<br />  {<br />    value += -1 / steps;<br />    if (value &lt; 0)<br />        value = 0;<br />    setfade(img, value, otype);<br />    if (value &gt; 0)<br />    {<br />     setTimeout(function()<br />     {<br />      dofadeOut(steps, img, value, otype);<br />     }, 1000/ fps);<br />    }<br />  }</font>
  </p>
<p> </p>