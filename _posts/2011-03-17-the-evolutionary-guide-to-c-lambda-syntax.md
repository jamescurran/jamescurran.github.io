---
layout: default
title: The Evolutionary Guide to C# Lambda Syntax
---

  
<p>Originally (.NET V1.1), we had to explicitly create a Delegate object to wrap a method reference to use it as a callback method, and that method had to named.</p>
<div class="csharpcode">   <pre class="alt">button1.Click += <span class="kwrd">new</span> EventHandler(button1_Click);</pre>

  <pre><span class="rem">// :</span></pre>

  <pre class="alt"><span class="rem">// :</span></pre>

  <pre><span class="kwrd">void</span> button1_Click(<span class="kwrd">object</span> sender, EventArgs e)</pre>

  <pre class="alt">{</pre>

  <pre>    <span class="rem">DoStuff();</span></pre>

  <pre class="alt">}</pre>

  <pre> </pre>
</div>
<p>With .NET v2.0, The C# compiler got smart enough to realize that when I used a method reference in code, I needed it wrapped up as a delegate, so it would silently to that for me.</p>
<pre class="csharpcode">    button1.Click += button1_Click;</pre>
<p>Better still, C#2 added anonymous methods, which could be written inline.</p>
<pre class="csharpcode">    button1.Click += <span class="kwrd">delegate</span> (<span class="kwrd">object</span> s, EventArgs ea) { DoStuff();}</pre>
<p>Then, C#3, we got lambdas, which were basically anonymous methods with a cleaned up syntax.</p>
<pre class="csharpcode">    button1.Click += (<span class="kwrd">object</span> s, EventArgs ea) =&gt; { DoStuff(); };</pre>
<p>But, at the same time, the compiler got brighter about figuring things out for itself.  For example,  the Button Click event took a delegate to a method which had an object and an EventArgs parameter.  Giving it anything else is a compile-time error.  So, since we all agree that those are the parameters, why is it necessary for us to stand that out loud.  Why not just let the compile assume it.</p>
<pre class="csharpcode">    button1.Click += (s, ea) =&gt; { DoStuff(); };</pre>
<p>From there, we have just a few more refinements, for special (but common) cases, but for these we can no longer use Button Click as the destination of our method reference, so from here on out, we’ll be use Enumerator.Where on an int array.  The important point here is that the lambda we will be writing takes an int, and return a bool.  Within that environment, our last syntax would look like this:</p>
<div class="csharpcode">
  <pre class="alt">    <span class="kwrd">int</span>[] x = <span class="kwrd">new</span> <span class="kwrd">int</span>[] {1,2,3,4};</pre>

  <pre>    var y = x.Where((x)=&gt;{<span class="kwrd">return</span> x % 2 == 0;}).ToList();</pre>
</div>
<p>But, if we have just one parameter, the compiler can figure out where it starts and ends, so we don’t need the parenthesis. </p>
<pre class="csharpcode">    var y = x.Where( x =&gt; { <span class="kwrd">return</span> x % 2 == 0;}).ToList();</pre>
<p>Finally, if all the function does is return a value (which is all a true lambda function is supposed to do), we can eliminate the curly braces and even the <strong>return</strong> :</p>
<pre class="csharpcode">    var y = x.Where( x =&gt;  x % 2 == 0).ToList();</pre>
<p>And that’s really all you need to know to write a lambda function.</p>
<a href="http://dotnetshoutout.com/Honest-Illusion-The-Evolutionary-Guide-to-C-Lambda-Syntax">
  <img alt="Shout it" src="http://dotnetshoutout.com/image.axd?url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F03%2F17%2Fthe-evolutionary-guide-to-c-lambda-syntax.aspx" style="border:0px;" />
</a>