---
layout: post
title: C# Code:Adding Skip First to Foreach
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---

  <p>A couple years back, I made a <a href="http://groups.google.com/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/412fd19c65ea81de">proposal online</a> for a new feature in C#.  It gathered some interest in the newsgroup.  I later emailed it to someone on the C# team at Microsoft (I believe it was <a href="http://blogs.msdn.com/ericgu/default.aspx">Eric Gunnerson</a> but I'd really have to look it up), who emailed back a very nice response saying basically, "We thought about something like this, but decided again it, because it can be done with the Iterators we're adding to C# v2.0".</p> <p>So, I figured, three years later, perhaps it's time I actually did implement them using iterator.</p> <p>The idea was to allow developers to use foreach to iterator over a collection, in the case where the first or last item in the collection needed to be handled differently.   My first crack at it works like this:</p> <p> </p><pre class="csharpcode">int[] ary = new int[ 6 ] { 1, 2, 3, 4, 5, 6 };
<span class="kwrd">foreach</span> (int a <span class="kwrd">in</span> new SkipFirst&lt;int&gt;(ary))
{
    Console.Write(a);  // Prints 23456
}<br />
The code for it looks like this:</pre>
<p> </p>
<div class="csharpcode"><pre class="alt">public class SkipFirst&lt;T&gt; : IEnumerable&lt;T&gt;</pre><pre>{</pre><pre class="alt">    <span class="kwrd">private</span> IEnumerable&lt;T&gt; mEnum;</pre><pre>    public SkipFirst(IEnumerable&lt;T&gt; enm)</pre><pre class="alt">    {</pre><pre>        mEnum = enm;</pre><pre class="alt">    }</pre><pre>    <span class="rem">#region IEnumerable&lt;T&gt; Members</span></pre><pre class="alt"> </pre><pre>    public IEnumerator&lt;T&gt; GetEnumerator()</pre><pre class="alt">    {</pre><pre>        IEnumerator&lt;T&gt; iter = mEnum.GetEnumerator();</pre><pre class="alt">        <span class="kwrd">if</span> (iter.MoveNext())</pre><pre>        {</pre><pre class="alt">            <span class="kwrd">while</span> (iter.MoveNext())</pre><pre>            {</pre><pre class="alt">                yield <span class="kwrd">return</span> iter.Current;</pre><pre>            }</pre><pre class="alt">        }</pre><pre>    }</pre><pre class="alt"> </pre><pre>    <span class="rem">#endregion</span></pre><pre class="alt"> </pre><pre>    <span class="rem">#region IEnumerable Members</span></pre><pre class="alt"> </pre><pre>    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()</pre><pre class="alt">    {</pre><pre>        <span class="kwrd">return</span> GetEnumerator();</pre><pre class="alt">    }</pre><pre> </pre><pre class="alt">    <span class="rem">#endregion</span></pre><pre>}</pre></div>
<p> </p>
<p>On the whole, quite simple minded.  The while() loop is the essence of a standard IEnumerator.  I basically just added a MoveNext() first.   Handling SkipLast is a little bit trickier:</p><pre class="csharpcode">public class SkipLast&lt;T&gt; : IEnumerable&lt;T&gt;
{
    <span class="kwrd">private</span> IEnumerable&lt;T&gt; mEnum;
    public SkipLast(IEnumerable&lt;T&gt; enm)
    {
        mEnum = enm;
    }
    <span class="rem">#region IEnumerable&lt;T&gt; Members</span>

    public IEnumerator&lt;T&gt; GetEnumerator()
    {
        IEnumerator&lt;T&gt; iter = mEnum.GetEnumerator();
        <span class="kwrd">if</span> (iter.MoveNext())
        {
            T curr = iter.Current;
            <span class="kwrd">while</span> (iter.MoveNext())
            {
                yield <span class="kwrd">return</span> curr;
                curr = iter.Current;
            }
        }

    }

    <span class="rem">#endregion</span>

    <span class="rem">#region IEnumerable Members</span>

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        <span class="kwrd">return</span> GetEnumerator();
    }

    <span class="rem">#endregion</span>
}
</pre>
<p> </p>
<p>Here's a must keep of copy of the Current item, and return the copy only after we've successfully done the MoveNext(). If you can't MoveNext, then the copy we've saved in the Last that we want to skip.</p>
<p> </p>
<p>However, the problem with these is that they have a rather ugly syntax : You have to include "new" and specify the type. But, there's a way out:  If we can create a function which returns on object of our IEnumerator class, we can have the compiler figure out the types:</p>
<p> </p><pre class="csharpcode">static public class Skip
{
    public class SkipFirst&lt;T&gt; : IEnumerable&lt;T&gt;
    {  /* As above */  }
    public class SkipLast&lt;T&gt; : IEnumerable&lt;T&gt;
    {  /* As above */  }

    static public SkipFirst&lt;T&gt; First&lt;T&gt;(IEnumerable&lt;T&gt; enm)
    {
        <span class="kwrd">return</span> new SkipFirst&lt;T&gt;(enm);
    }
    static public SkipLast&lt;T&gt; Last&lt;T&gt;(IEnumerable&lt;T&gt; enm)
    {
        <span class="kwrd">return</span> new SkipLast&lt;T&gt;(enm);
    }
}</pre>
<p> </p>Now all we have to write is this: 
<p> </p><pre class="csharpcode">int[] ary = new int[ 6 ] { 1, 2, 3, 4, 5, 6 };
<span class="kwrd">foreach</span> (int a <span class="kwrd">in</span> Skip.First(Skip.Last(ary)))
{
    Console.Write(a); // writes <span class="str">"2345"</span>
}
</pre>
<p>Somewhat enhanced source code is available <a href="http://honestillusion.com/files/folders/c-sharp/entry4396.aspx">here</a>.</p><a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/05/c-code-adding-skip-first-to-foreach.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/05/c-code-adding-skip-first-to-foreach.aspx" border="0" /></a>