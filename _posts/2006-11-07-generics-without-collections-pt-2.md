---
layout: default
title: Generics without Collections, pt. 2
---

  <p>My <a title="Generics without Collections" href="http://honestillusion.com/blogs/blog_0/archive/2006/10/02/Generics-without-Collections.aspx">previous article</a> on this subject dealt with creating a lazy-loaded data type.  But, if you think about it, that's realy just a collection, with just one item.  I promised you use of generics without collections, so let's move this to the next step, using the type to affect the behavior of the code, without ever storing an object of that type.</p> <p>You've probably written some code like this:</p> <div class="csharpcode"><pre class="alt">    <span class="kwrd">foreach</span> (Control ctrl <span class="kwrd">in</span> <span class="kwrd">this</span>.Controls)</pre><pre>    {</pre><pre class="alt">        ComboBox cb = ctrl <span class="kwrd">as</span> ComboBox;</pre><pre>        <span class="kwrd">if</span> (cb != <span class="kwrd">null</span>)</pre><pre class="alt">        {</pre><pre>             MessageBox.Show(<span class="str">"CheckBox "</span> + (chb.Checked ? <span class="str">"IS"</span> : <span class="str">"is NOT"</span>) + <span class="str">" checked"</span>);                </pre><pre class="alt"> </pre><pre>        }</pre><pre class="alt">    }</pre></div>
<p>This gives a list of all ComboBoxes in the curret control set.  To handle that, we've got to go through all the controls and filter out the ones that are not ComboBoxes.  Wouldn't it be great if we could handle this filtering automatically?</p>
<div class="csharpcode"><pre class="alt">    <span class="kwrd">foreach</span> (CheckBox chb <span class="kwrd">in</span> ControlFilter.Only&lt;CheckBox&gt;(<span class="kwrd">this</span>.Controls))</pre><pre>    {</pre><pre class="alt">        MessageBox.Show(<span class="str">"CheckBox "</span> + (chb.Checked ? <span class="str">"IS"</span> : <span class="str">"is NOT"</span>) + <span class="str">" checked"</span>);</pre><pre>    }</pre></div>
<p>We start by defining the static class to hold this method, and the method itself. You'll note that nowhere in this block do we use the type parameter directly. </p>
<div class="csharpcode"><pre class="alt"><span class="kwrd">using</span> System.Windows.Forms;<span class="kwrd">static</span> <span class="kwrd">class</span> ControlFilter</pre><pre>{</pre><pre class="alt">   <span class="kwrd">public</span> <span class="kwrd">static</span> IEnumerable&lt;T&gt; Only&lt;T&gt;(Control.ControlCollection coll) <span class="kwrd">where</span> T:Control</pre><pre>   {</pre></div>
<p>We need to return an object here, one which implements IEnumerable&lt;T&gt;, so let's create one, and pass all the work on to it:</p>
<div class="csharpcode"><pre class="alt">    <span class="kwrd">return</span> <span class="kwrd">new</span> ControlFilter_impl&lt;T&gt;(coll);</pre><pre>   }</pre></div>
<p>Since ControlFIlter_impl has no meaning outside of ControlFilter, while just define it as an internal class inside ControlFilter.</p>
<div class="csharpcode"><pre class="alt">   <span class="kwrd">class</span> ControlFilter_impl&lt;T&gt; : IEnumerable&lt;T&gt; <span class="kwrd">where</span> T : Control</pre><pre>   {</pre><pre class="alt">       <span class="kwrd">private</span> Control.ControlCollection m_Coll;</pre><pre>       <span class="kwrd">public</span> ControlFilter_impl(Control.ControlCollection coll)</pre><pre class="alt">       {</pre><pre>          m_Coll = coll;</pre><pre class="alt">       }</pre></div>
<p>So far, all we've done is store a reference to the collection we plan on enumerating -- and we still haven't actually used type T for anything, but now we do that in the next step.</p>
<div class="csharpcode"><pre class="alt"><span class="kwrd">public</span> IEnumerator&lt;T&gt; GetEnumerator()</pre><pre>{</pre><pre class="alt">    <span class="kwrd">foreach</span> (Control ctrl <span class="kwrd">in</span> m_Coll)</pre><pre>    {</pre><pre class="alt">        T ctrlT = ctrl <span class="kwrd">as</span> T;</pre><pre>        <span class="kwrd">if</span> (ctrlT != <span class="kwrd">null</span>)</pre><pre class="alt">            <span class="kwrd">yield</span> <span class="kwrd">return</span> ctrlT;</pre><pre>    }</pre><pre class="alt">}</pre></div>Here, we use the "yield return" statement to create an iterator block.  It is essentially the same algorithm as we started with, but here we finally use the passed-in type to filter the controls as they go by.   That is the important message here.  We are using the passed in type to modify what the algorithm does, instead of to just hold a bunch of them. 
<p>The rest is just boiler plate to keep the compiler happy.</p>
<div class="csharpcode"><pre class="alt">    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()</pre><pre>    {</pre><pre class="alt">        <span class="kwrd">return</span> <span class="kwrd">this</span>.GetEnumerator();</pre><pre>    }</pre><pre class="alt">}</pre></div><a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/07/Generics-without-Collections-_2800_pt-2_2900_.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/07/Generics-without-Collections-_2800_pt-2_2900_.aspx" border="0" /></a>