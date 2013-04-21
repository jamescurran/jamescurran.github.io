---
layout: post
title: Posts from Comments:QuickDataBind
categories: code c# .net programming dotnet csharp
tags: code c# .net programming dotnet csharp
---

  <p>You may have noticed that I don’t write on this blog much.  But the thing is I <em>do</em> write a lot on the inter-webs about technical matters --- I just don’t to it here.  Usually, I find something interesting on someone else’s blog, and then write an improvement in the comments.  So, my work goes to helping other people’s pagerank.  I figure this has got to stop… To this end, I start a series where I turn comments I made on other blogs into posts on this one….</p>  <p> </p>  <p>To start us off, a few days ago, <a href="http://geekswithblogs.net/samerpaul/archive/2009/07/22/listview-extension-i-thought-irsquod-sharehellip.aspx" target="_blank">Samer wrote about an extension method</a> he created for ListView: </p>  <pre class="c#">public static ListView QuickDataBind(this ListView myListView, object myDataSource)
    {
        myListView.DataSource = myDataSource;
        myListView.DataBind();
        return myListView;
    }</pre>

<p>Now, this is all well and good.  but why are we limiting ourselves to just ListViews?  Many ASP.NET WebControl take a datasource and use that idiom.  Why not make an generic extension method to handle all of them?</p>

<pre class="c#">public static T QuickDataBind(this T myDataBoundControl, object myDataSource) 
        where T: BaseDataBoundControl
{
        myDataBoundControl.DataSource = myDataSource;
        myDataBoundControl.DataBind();
        return myDataBoundControl;
}</pre>
It's still called exactly the same well: 

<pre class="c#">       myGridView.QuickDataBind(myDS);</pre>
but now it can be used on ListViews, GridView, DropDownLists DataGrids, Repeaters or anything else that uses a DataSOurce. 

