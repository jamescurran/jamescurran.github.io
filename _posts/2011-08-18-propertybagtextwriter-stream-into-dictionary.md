---
layout: default
title: PropertyBagTextWriter (Stream into Dictionary)
categories: code c# .net programming dotnet csharp castle monorail codeproject
tags: code c# .net programming dotnet csharp castle monorail codeproject
---

  <p>It’s been too long since I posted since .NET code, and I’ve been itching to.  (Actually, I really want to write more about politics, but I figured if I don’t show some code soon, I’m gonna lost my techy audience)  Fortunately, I’ve got a backlog of things I’ve been meaning to write about.  Today’s is the PropertyBagTextWriter.</p>
  <p>The original purpose of this for a particular use in combination with Castle Monorail and Linq-2-Sql, but it has been made general purpose, so you may find a use for it in other environments.  </p>
  <p>Now, when Linq-2-Sql was still in beta, the DataContext object had a property which held, as a string, the SQL generated from the Linq query.  As I was writing a Monorail website, I often assigned that property to a value in the PorpertyBag (which is just a IDictionary, not even a IDictionary&lt;K,V&gt;  -- <font face="Courier New"><i>PropertyBag[“SQL”] = db.Log;</i></font>), and write it in  an HTML comment on the webpage, so I could see I was getting what I expected.  However, the designer eventually realized that a string property wasn’t good enough, as the Linq query could produce several SQL statement, some of which would be based on the response from the earlier ones.  So, they replaced it with a property which can be set to a TextWriter and have the SQL output written there.  So, to use it the way I was before, I needed a TextWriter-ish object, which would set it’s output to a value in a Dictionary  (<font face="Courier New"><i>db.Log = new PropertyBagTextWriter(“SQL”, PropertyBag);</i></font> )   The important point here is that it’s self-contained.  Once we set the property to the PropertyBagTextWriter object,  we should never have to interact with it again.  The value should just appear in the dictionary when it’s ready.</p>  <p>The code itself is fairly straightforward.  Start by deriving a new class from StringWriter, which is usually the best way to create a customized TextWriter.  That way, it’s handle the details of gathering and formatting the data from the stream, and all we have to deal with is the string at the end.</p>  <pre class="csharpcode">
  <span class="kwrd">class</span> ProperyBagTextWriter : StringWriter
    {</pre>

<p>Next, we’re going to need to know the dictionary the output will be stored in,  and the key, so we accept those in the constructor, and hold on to them for later:</p>

<pre class="csharpcode"><span class="kwrd">public</span> ProperyBagTextWriter(<span class="kwrd">string</span> key, IDictionary bag)
{
    <span class="kwrd">this</span>.key = key;
    <span class="kwrd">this</span>.bag = bag;
}
<span class="kwrd">string</span> key;
IDictionary bag;</pre>

<p>Then the key point:   When we get a Flush() call, we save the text we gathered so far into the dictionary under that key:</p>

<pre class="csharpcode"><span class="kwrd">public</span> <span class="kwrd">override</span> <span class="kwrd">void</span> Flush()
{
    <span class="kwrd">base</span>.Flush();
    bag[key] = <span class="kwrd">base</span>.ToString();
}</pre>

<p>However, since we can’t count on the Flush always being called when we need it, we’ll force a flush at other times, like during the Dispose() and after writing a line:</p>

<pre class="csharpcode"><span class="kwrd">protected</span> <span class="kwrd">override</span> <span class="kwrd">void</span> Dispose(<span class="kwrd">bool</span> disposing)
{
    <span class="kwrd">base</span>.Dispose(disposing);
    <span class="kwrd">if</span> (disposing)
        Flush();
}</pre>

<pre class="csharpcode"><span class="kwrd">public</span> <span class="kwrd">override</span> <span class="kwrd">void</span> Write(<span class="kwrd">char</span>[] buffer, <span class="kwrd">int</span> index, <span class="kwrd">int</span> count)
{
    <span class="kwrd">base</span>.Write(buffer, index, count);
    Flush();
}</pre>

<p> </p>

<p>That all there is to it.  Besides the Linq2Sql log, I’ve also used it for the output from a XSLT transformation.</p>

<p> </p>

<p>Source Code: I’ve decided to get with the times, and create (well, actually “use”… I created it a while ago), a GitHub account.  So, you can find this class, code from my future posts, and when I get around to it, code from my older post, at <a href="http://github.com/jamescurran/HonestIllusion">http://github.com/jamescurran/HonestIllusion</a></p>