---
layout: post
title: Code Tuneup : DataConvert
categories: csharp dotnet code-tuneup
---

(Ya'know, I think I'll make "Code Tuneup" a regular feature on Honest Illusion -- If a site that goes months without a posting can be said to have "regular features")

CODE Magazine, one of the last hard-core programming magazines left, just put out it's May-June 2013 issue.   One of the article is Paul. D Sheriff's  [Creating Collections of Entity Objects](http://www.code-magazine.com/article.aspx?quickid=1305031&page=1) (Note: unless you're a subscriber, you'll only be able to see the first 1/3rd of the article online.)

Now, while most of the article is quite good, Paul starts with some basic routines and build to more advanced one, but he doesn't take it quite far enough.

It's seems he's a bit timid when using generics, using them for some parts of a method, but not others, not unleashing their full potential .  Let's see what we can do about that.

Our first sample is this:
        

	public static T ConvertTo<T>(object value, object defaultValue)
        {
            if (value.Equals(DBNull.Value))
                return (T)defaultValue;
            else
                return (T)value;
        }

Background: In the article, we are reading data from a database, and putting it into strongly typed class.  This method is part of the low-level code to convert a most likely boxed value into an hard number.   It would be used like:

	foreach (DataRow dr in dt.Rows)
	{
		entity = new Product();

		entity.IntroductionDate =
			DataConvert.ConvertTo<DateTime>(dr["IntroductionDate"], default(DateTime));
					 
		entity.Cost =
			DataConvert.ConvertTo<decimal>(dr["Cost"],  default(decimal));         


