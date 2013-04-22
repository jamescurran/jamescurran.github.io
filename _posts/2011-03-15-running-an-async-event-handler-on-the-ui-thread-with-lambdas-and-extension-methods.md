---
layout: post
title: Running An Async Event Handler on the UI thread (with lambdas and extension methods!)
categories: code c# .net programming dotnet csharp codeproject
tags: code c# .net programming dotnet csharp codeproject
---

  
<p>So, it’s been a freakishly long time since my last post here.  </p>
<p>I’ve been trying to get better… I even wrote out a list of topics I wanted to write about.  So, let’s start talking about them.</p>
<p>As we do more and more work in our applications asynchronously, we’re confronted with the problem of how to communicate the status of the background thread to our users.  Firing an event seems like the best way, except anything the user can see has to be done by the UI thread, which is specifically not the thread the background task is running on. </p>
<p>Winforms offers a means of redirecting execution onto the UI thread, the Form.Invoke method, but using it makes life difficult.   This is particular true when you throw lambdas into the mix.  Lambdas made it quite easy to write a simple event handler:</p>
<pre style="border-bottom:#cecece 1px solid;border-left:#cecece 1px solid;padding-bottom:5px;background-color:#fbfbfb;min-height:40px;padding-left:5px;width:650px;padding-right:5px;overflow:auto;border-top:#cecece 1px solid;border-right:#cecece 1px solid;padding-top:5px;">
  <pre style="background-color:#fbfbfb;margin:0em;width:100%;font-family:consolas,'Courier New',courier,monospace;font-size:12px;">  backgroundAction.StepCompleted +=
         (s, ea) =&gt; {statusMsg.Text = String.Format("<span style="color:#8b0000;">Step #{0} Completed</span>", ea.StepNum);}</pre>
</pre>
<p>But when we add is thread marshaling, it gets messy:</p>
<pre style="border-bottom:#cecece 1px solid;border-left:#cecece 1px solid;padding-bottom:5px;background-color:#fbfbfb;min-height:40px;padding-left:5px;width:958px;padding-right:5px;height:52px;overflow:auto;border-top:#cecece 1px solid;border-right:#cecece 1px solid;padding-top:5px;">
  <pre style="background-color:#fbfbfb;margin:0em;width:100%;font-family:consolas,'Courier New',courier,monospace;font-size:12px;">  backgroundAction.StepCompleted +=
</pre>
  <pre style="background-color:#fbfbfb;margin:0em;width:100%;font-family:consolas,'Courier New',courier,monospace;font-size:12px;">    (s, ea) =&gt;{<span style="color:#0000ff;">this</span>.Invoke((s1,ea1)=&gt;{statusMsg.Text = String.Format("<span style="color:#8b0000;">Step #{0} Completed</span>", ea.StepNum);}, s,ea);</pre>
</pre>
<p>And that’s with always marshaling it.  If the event could be fired from both a background thread or from the main thread depending on the context, then you should check the InvokeRequired property, if call the action directly if it’s not needed.  But complicates our lives, making us split the action out into a named function:</p>
<pre style="border-bottom:#cecece 1px solid;border-left:#cecece 1px solid;padding-bottom:5px;background-color:#fbfbfb;min-height:40px;padding-left:5px;width:650px;padding-right:5px;overflow:auto;border-top:#cecece 1px solid;border-right:#cecece 1px solid;padding-top:5px;">
  <pre style="background-color:#fbfbfb;margin:0em;width:100%;font-family:consolas,'Courier New',courier,monospace;font-size:12px;">backgroundTask.StepCompleted += ((s, ea) =&gt;
{
  <span style="color:#0000ff;">if</span> (<span style="color:#0000ff;">this</span>.InvokeRequired)
    <span style="color:#0000ff;">this</span>.Invoke(UpdateStatus, s, ea);
  <span style="color:#0000ff;">else</span>
    UpdateStatus(s,ea);
});
<span style="color:#008000;">// ...</span>
<span style="color:#0000ff;">void</span> UpdateStatus(<span style="color:#0000ff;">object</span> sender, MessageEventArgs ea)
{
  statusMsg.Text = String.Format("<span style="color:#8b0000;">Step #{0} Completed</span>", ea.StepNo);
}
</pre>
</pre>
<p>Yech!  We have a lambda, but it we’re using it for the boiler-plate code and that we will probably have to repeat.  And the real method we want to perform is in a separate function.</p>
<p>What we need is a handle utility function which will take a method reference, or, better yet, a lambda, and package it up as we need it. Then we could write it like: </p>
<div class="csharpcode">
  <pre class="alt">backgroundTask.StepCompleted += </pre>

  <pre>   ToUIThread&lt;StepEventArgs&gt;((s, ea) =&gt; statusMsg.Text = String.Format(<span class="str">"Step #{0} Completed"</span>, ea.StepNo));</pre>
</div>
<p>The tricky part about this is that it must take a function as a parameter, and <em>return</em> a function. Plus, the compiler can’t figure out the type of the second parameter by itself, so we have to give it some help.  And, we we’ll need a non-generic version, for event which are defined as EventHandle instead of EventHandler&lt;TEventArgs&gt;.   Make it an extension method on Form, and we’ve got:</p>
<div class="csharpcode">
  <pre class="alt"><span class="kwrd">using</span> System;</pre>

  <pre><span class="kwrd">using</span> System.Threading;</pre>

  <pre class="alt"><span class="kwrd">using</span> System.Threading.Tasks;</pre>

  <pre><span class="kwrd">using</span> System.Windows.Forms;</pre>

  <pre class="alt"> </pre>

  <pre><span class="kwrd">public</span> <span class="kwrd">static</span> <span class="kwrd">class</span> FormExt</pre>

  <pre class="alt">{</pre>

  <pre>    <span class="kwrd">static</span> <span class="kwrd">public</span> EventHandler&lt;TEventArgs&gt; ToUIThread&lt;TEventArgs&gt;(<span class="kwrd">this</span> Form frm, EventHandler&lt;TEventArgs&gt; handler)</pre>

  <pre class="alt">                <span class="kwrd">where</span> TEventArgs : System.EventArgs</pre>

  <pre>    {</pre>

  <pre class="alt">        <span class="kwrd">return</span> ((sender, e) =&gt;</pre>

  <pre>        {</pre>

  <pre class="alt">            <span class="kwrd">if</span> (frm.InvokeRequired)</pre>

  <pre>                frm.Invoke(handler, sender, e);</pre>

  <pre class="alt">            <span class="kwrd">else</span></pre>

  <pre>                handler(sender, e);</pre>

  <pre class="alt">        });</pre>

  <pre>    }</pre>

  <pre class="alt"> </pre>

  <pre>    <span class="kwrd">static</span> <span class="kwrd">public</span> EventHandler ToUIThread(<span class="kwrd">this</span> Form frm, EventHandler handler)</pre>

  <pre class="alt">    {</pre>

  <pre>        <span class="kwrd">return</span> ((sender, e) =&gt;</pre>

  <pre class="alt">        {</pre>

  <pre>            <span class="kwrd">if</span> (frm.InvokeRequired)</pre>

  <pre class="alt">                frm.Invoke(handler, sender, e);</pre>

  <pre>            <span class="kwrd">else</span></pre>

  <pre class="alt">                handler(sender, e);</pre>

  <pre>        });</pre>

  <pre class="alt">    }</pre>

  <pre>}</pre>

  <pre class="alt"> </pre>
</div>
<a href="http://dotnetshoutout.com/Honest-Illusion-Running-An-Async-Event-Handler-on-the-UI-thread-with-lambdas-and-extension-methods">
  <img alt="Shout it" src="http://dotnetshoutout.com/image.axd url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F03%2F15%2Frunning-an-async-event-handler-on-the-ui-thread-with-lambdas-and-extension-methods.aspx" style="border:0px;" />
</a>