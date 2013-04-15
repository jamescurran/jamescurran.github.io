---
layout: default
title: Automating Retry on Exception
---

  <p>Every so often, you run across some action,  which just fails, where the best response it to just try it again.  This is particularly true when dealing with an external source, like a database or web service, which can have network or other temporary problems, which would have cleared up when you repeat the call seconds later.   Often, these actions fail by throwing an exception which makes the process of trying the call again rather cumbersome.  Having to deal with this a number of times in one application, I decided to wrap the full procedure up in a method, which I share here with you:</p>  <p>OK, this has very weird semantics.  A call will look something like this:</p>  <div class="csharpcode">   <pre class="alt">var msg = Util.Retry(5, 1,</pre>

  <pre>            ( ) =&gt; <span class="str">"Error"</span>,</pre>

  <pre class="alt">            AppLog.Log,   <span class="rem">// assuming there is an AppLog.Log(Exception ex) method.</span></pre>

  <pre>            ( ) =&gt;</pre>

  <pre class="alt">            {</pre>

  <pre>                    <span class="rem">// Simulation of a function which fails &amp; succeeds at random times</span></pre>

  <pre class="alt">                    Console.WriteLine(DateTime.Now.ToLongTimeString());</pre>

  <pre>                    <span class="kwrd">if</span> ((DateTime.Now.Second &amp; 7) != 7)</pre>

  <pre class="alt">                    <span class="kwrd">throw</span> <span class="kwrd">new</span> Exception(<span class="str">"Bad"</span>);</pre>

  <pre>                    <span class="kwrd">return</span> DateTime.Now.ToString();</pre>

  <pre class="alt">            });</pre>
</div>

<p>The first parameter is the maximum num of time to retry the call before giving up.  </p>

<p>The second parameter is the time, in seconds, to wait between retries.</p>

<p>The third parameter is a function (or lambda expression) taking no parameters, and returning the value to return in case of failure. It done as a function to avoid evaluating it unless needed, in case it was a heavy-weight object, or had some side-effect.</p>

<p>The  fourth parameter (made optional by way of an overload) is a function (or lambda expression) taking an exception as a parameter, and returning nothing.  Can be used to log the final exception on it gives up retrying.</p>

<p>The last parameter is a function (or lambda expression) taking no parameters, which performs the actual work which may need to be retried. 
  <br /></p>

<p>The code looks like this:</p>

<div class="csharpcode">
  <pre class="alt"><span class="kwrd">static</span> <span class="kwrd">public</span> T Retry&lt;T&gt;(<span class="kwrd">int</span> retries, <span class="kwrd">int</span> secsDelay, Func&lt;T&gt; errorReturn,   Action&lt;Exception&gt; onError,  Func&lt;T&gt; code)</pre>

  <pre>{</pre>

  <pre class="alt">    Exception ex = <span class="kwrd">null</span>;</pre>

  <pre>    <span class="kwrd">do</span></pre>

  <pre class="alt">    {</pre>

  <pre>        <span class="kwrd">try</span></pre>

  <pre class="alt">        {</pre>

  <pre>            <span class="kwrd">return</span> code();</pre>

  <pre class="alt">        }</pre>

  <pre>        <span class="kwrd">catch</span> (Exception dde)</pre>

  <pre class="alt">        {</pre>

  <pre>            ex = dde;</pre>

  <pre class="alt">            retries--;</pre>

  <pre>            Thread.Sleep(secsDelay * 1000);</pre>

  <pre class="alt">        }</pre>

  <pre>    } <span class="kwrd">while</span> (retries &gt; 0);</pre>

  <pre class="alt">    onError(ex);</pre>

  <pre>    <span class="kwrd">return</span> errorReturn();</pre>

  <pre class="alt">}</pre>
</div>

<p>The full source code (with comments and everything!) is <a target="_blank" href="http://honestillusion.com/files/folders/c-sharp/entry8103.aspx">here</a>:</p>

<a href="http://dotnetshoutout.com/Honest-Illusion-Automating-Retry-on-Exception"><img alt="Shout it" style="border:0px;" src="http://dotnetshoutout.com/image.axd?url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F03%2F21%2Fautomating-retry-on-exception.aspx" /></a>