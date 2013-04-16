---
layout: default
title: DEV102's Programming Job Interview Challenge #6 
categories: code c# .net dotnet csharp
tags: code c# .net dotnet csharp
---

  <p>Another week, another C# interview question from the good folk's at Dev102.com -- Although I use the term "good folks" advisedly, as this week they did not even acknowledge the solution I posted for last weeks puzzle (which was both correct, and, I believe, the first blog post about it).</p>

<p>Anyway, time to move on to <a href="http://www.dev102.com/2008/06/02/a-programming-job-interview-challenge-6-c-games/">this week's question</a>. </p>

<blockquote>
<p>Look at the following Code segment written in C#:</p>
<div>
<div style="border-style:none;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;">
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   1:</span> ArrayList a = <span>new</span> ArrayList();</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   2:</span> ArrayList b = <span>new</span> ArrayList();</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   3:</span></pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   4:</span> a.Add(1);</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   5:</span> b.Add(1);</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   6:</span> a.Add(2);</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   7:</span> b.Add(2.0);</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   8:</span></pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>   9:</span> Console.WriteLine((a[0] == b[0]));</pre>
<pre style="border-style:none;margin:0em;padding:0px;overflow:visible;font-size:8pt;width:100%;color:black;line-height:12pt;"><span>  10:</span> Console.WriteLine((a[1] == b[1]));</pre>
</div>
</div>
What will be typed into the console? And <b><span style="text-decoration:underline;">WHY?</span></b></blockquote>

<p> </p>

<p> This one is fairly trivial (They seem to be alternating between difficult and easy questions).  And, as requested, I formulated my answer before typing it into VS (actually, I copy'n'pasted in SnippetCompiler), but the compiler DID confirm the answer I'd already theorized.</p>
<p>This answer is short enough that we can use the cool "white-on-white; select to see it" trick--- However, RSS feed (and apparently the theme of this blog) seem to ignore the color style, so it's probably visible to you below.. </p>

<p> </p>
<p> </p>
<p> </p>
<div style="color:white;">
<p>ArrayList is deep-down, just an object[].  To store an valuetype, like an int or float, in an ArrayList, that value would first have to be boxed.  Each valuetype is boxed separately, in distinct objects, even if they do happen to have the same value. When we get to the WriteLines, we are just performing (object) == (object) (actually, Object.ReferenceEquals(object1, object2); )  ReferenceEquals knows nothing about unboxing.  It just asks, "Are these two references pointing to the exact same object?".  For any two boxed objects, regardless of their value, the answer would be "No".  Hence, both lines print "False".</p>
</div>
<p> </p>
<p> </p>
<p> </p>

<p> (select the blank space above)</p>
<br />