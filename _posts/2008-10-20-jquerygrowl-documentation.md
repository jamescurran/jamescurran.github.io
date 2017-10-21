---
layout: post
title: jQuery.growl Documentation
tags: code javascript programming 
---

Right now, I'm in the midst of a long-running project to rewrite my other website, [NJTheater.com](http://www.njtheater.com).  In the process, I've discovered jQuery, the hot new javascript library that all the kids are using today.   One of it's key selling points is it's well designed plugin system, which has led to a host of add-ons being written for it. 

Recently, I stumbled upon one such plugin, [jQuery Growl Plugin](http://www.fragmentedcode.com/jquery-growl) by [David Higgins](http://www.fragmentedcode.com/).  Apparently, Growl is a MacOS application, so the Applist readers should by now figured out what it does.  For the Windows/Linux folk, it displays a little popup alert box, sort-of like the Messenger "toaster" popup, except they come down from the top.  They slide down, stay for a few moments, and then fade out.  If another is displayed while the first is still visible, they stack.  [Demos here](http://projects.zoulcreations.com/jquery/growl/).  Now, while the demos looked rather cool, library itself does suffer from the main problem that affects most open source code -- the documentation just sucks.  In fact, it goes beyond mere suckage; at one point, you get the feeling the author is just mocking you.

But no sense in just complaining, or insulting a person who has contributed to the community.  The best thing to do in this case is for one to contribute himself.  And so, here's my documentation for the plugin

##jQuery.growl

The official calling syntax is:

	$.growl(title, message, image, priority);
	
All four parameters are strings, and all have defaults, so you only need to pass the ones you are using.  However, the first two default to an empty string, so it'll be rather boring unless you specify them.  The third and fourth parameters have reasonable defaults, but, well, we'll get to that in a minute.

You see, the important thing to realize here is that the HTML displayed is template driven.  So, while the first parameter is called "title", that merely means that it'll be used to replace the string "%title%" in the template.  Similarly, the value of the "message" parameter replaces "%message%" in the template; "image" replaces "%image%", and you guessed it, "priority" replaces "%priority%".

This is important to know, because, while there is a default template, which uses %title% as the title and %message% as the message, you can define your own template and in that template, you can use the four parameters for whatever you what.  (Templates are defined at the global level, which in this context mean "for the page").

Here we start getting into the bizarre part:  The replaceable keywords "%image%" and "%priority%" do not appear in the default template at all. Unless you define your own template, the values you pass for them will never be seen. Of course, if you do define your own template, there's nothing requiring that you use"%image%' as an image or "%priority%" as a priority.  The only thing holding them to their preordained role is their defaults: the image parameter defaults to ''growl.jpg", and priority defaults to "normal". (So the parameters aren't used out of the box having meaningful defaults, while the two that are used, have useless defaults).

The default template is rather minimalist, but functional:

<pre class="xml">&lt;div class="notice"&gt;
&lt;h3 style="margin-top: 15px"&gt;%title%&lt;/h3&gt;
&lt;p&gt;%message%&lt;/p&gt;
&lt;/div&gt;</pre>

An example of a more elaborate template would be:

<pre class="xml">&lt;div&gt;
  &lt;div style="float: right; background-image: url(normalTop.png); position: relative; width: 259px; height: 16px; margin: 0pt;"&gt;&lt;/div&gt;
  &lt;div style="float: right; background-image: url(normalBackground.png); position: relative; display: block; color: #ffffff; font-family: Arial; font-size: 12px; line-height: 14px; width: 259px; margin: 0pt;"&gt;
    &lt;img style="margin: 14px; margin-top: 0px; float: left;" src="%image%" /&gt;
    &lt;h3 style="margin: 0pt; margin-left: 77px; padding-bottom: 10px; font-size: 13px;"&gt;%title%&lt;/h3&gt;
    &lt;p style="margin: 0pt 14px; margin-left: 77px; font-size: 12px;"&gt;%message%&lt;/p&gt;
  &lt;/div&gt;
  &lt;div style="float: right; background-image: url(normalBottom.png); position: relative; width: 259px; height: 16px; margin-bottom: 10px;"&gt;&lt;/div&gt;
&lt;/div&gt;';</pre>

(That one came from jQuery.growl's author, and we still haven't found a use for the priority parameter!)

The template is changed by setting the $.growl.settings.noticeTemplate field.

	$.growl.settings.noticeTemplate = '&lt;div class="%priority%"&gt;&lt;div class="%priority%-heading"&gt;%title%&lt;/div&gt;&lt;div class="%priority%-message"&gt;%message%&lt;/div&gt;&lt;/div&gt;'</p>

The other setting that change be changed the same way are:

<table cellspacing="0" cellpadding="2">
    <tr>
      <td>
        <p align="center"><strong>Property</strong></p>
      </td>

      <td>
        <p align="center"><strong>Description </strong></p>
      </td>

      <td>
        <p align="center"><strong>Default</strong></p>
      </td>

      <td>
        <p align="center"><strong>Type</strong></p>
      </td>
    </tr>

    <tr>
      <td>dockTemplate</td>

      <td>Element in which the notices are created.</td>

      <td>'&lt;div&gt;&lt;/div&gt;'</td>

      <td>string </td>
    </tr>

    <tr>
      <td>dockCss</td>

      <td>Style elements applied on dock, generally used to specify it's position.</td>

      <td>Fixed in the upper right corner of the browser window</td>

      <td>object whose properties are feed to a css() method call.</td>
    </tr>

    <tr>
      <td>noticeTemplate</td>

      <td>Template for notice.</td>

      <td>(see above)</td>

      <td>string</td>
    </tr>

    <tr>
      <td>noticeCss</td>

      <td>Style elements applied on notice.</td>

      <td>White on Green at 3/4 opacity.</td>

      <td>object whose properties are fed to a css() method.</td>
    </tr>

    <tr>
      <td>noticeFadeTimeout</td>

      <td>How fast the notice fades out.</td>

      <td>'slow'</td>

      <td>String|Number, suitable for use in an animate() method call.</td>
    </tr>

    <tr>
      <td>displayTimeout</td>

      <td>Total time the notice displayed.</td>

      <td>3500 milliseconds</td>

      <td>number</td>
    </tr>

    <tr>
      <td>defaultImage</td>

      <td>Value used for %image% when not specified in the call.</td>

      <td>growl.jpg</td>

      <td>string</td>
    </tr>

    <tr>
      <td>defaultStylesheet</td>

      <td>Gives the name of a stylesheet , which, if specified, is automatically loaded.</td>

      <td>none</td>

      <td>string.</td>
    </tr>
  </table>

<p> </p>

<p>The dock needs a bit more explanation.  It's basically where the notices are drawn, and there's probably little reason to change it from it's default of a vanilla div.  Note that whatever it is, it will have the attributes "id=growlDock" and "class=growl" added to it.  </p>

<p>If you want to change the look of the dock, and want more control of it than stuffing some html into a property, you can just define an element with an id=growlDock, and $.growl will use that.</p>

<p>However the dock is defined, the style elements defined in the dockCss property are then added to it, and it's append to the &lt;body&gt; of the page.</p>

<p> </p>
<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2008%2f10%2f20%2fjquery-growl-documentation.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2008%2f10%2f20%2fjquery-growl-documentation.aspx" border="0" /></a>