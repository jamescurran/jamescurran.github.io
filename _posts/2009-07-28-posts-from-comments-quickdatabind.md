---
layout: post
title: Posts from Comments:QuickDataBind
categories: code c# .net programming dotnet csharp
tags: code c# .net programming dotnet csharp
---

You may have noticed that I don't write on this blog much.  But the thing is I *do* write a lot on the inter-webs about technical matters --- I just don't to it here.  Usually, I find something interesting on someone else's blog, and then write an improvement in the comments.  So, my work goes to helping other people's pagerank.  I figure this has got to stop... To this end, I start a series where I turn comments I made on other blogs into posts on this one....
  
To start us off, a few days ago, [Samer wrote about an extension method](http://geekswithblogs.net/samerpaul/archive/2009/07/22/listview-extension-i-thought-irsquod-sharehellip.aspx) he created for ListView: 

	public static ListView QuickDataBind(this ListView myListView, object myDataSource)
    {
        myListView.DataSource = myDataSource;
        myListView.DataBind();
        return myListView;
    }

Now, this is all well and good.  but why are we limiting ourselves to just ListViews   Many ASP.NET WebControl take a datasource and use that idiom.  Why not make an generic extension method to handle all of them?


	public static T QuickDataBind(this T myDataBoundControl, object myDataSource) 
        where T: BaseDataBoundControl
	{
        myDataBoundControl.DataSource = myDataSource;
        myDataBoundControl.DataBind();
        return myDataBoundControl;
	}
	
It's still called exactly the same way: 

       myGridView.QuickDataBind(myDS);
	   
but now it can be used on ListViews, GridView, DropDownLists DataGrids, Repeaters or anything else that uses a DataSOurce. 

