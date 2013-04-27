---
layout: post
title: Generics without Collections, Pt. 3
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---
Over the weekend, I attended the third [NJ Code Camp](http://www.njcodecamp.org/).  And since the moderators asked nicely, I presented this series as a talk.  Overall, the lecture didn't go well. (I was too nervous and talk too quickly.  Oddly, on one of the evaluation sheets, someone complained that I was talking too loudly, which I first is the first time in my life that was ever said about me)

Also, I think I was a victim of scheduling a bit.  My presentation was during the first shift, and was on an advanced topic.  The other generics talk that day (by Kevin Goff), which was much more of an introduction to generics, was given during the last shift.  So, if you didn't know generics well, you'd be completely confused by my talk.  But, if you'd seen Kevin's first, and then saw mine, it would have made more sense.

Anyway, for the presentation I came up with a few more examples, which I'll be posting to the blog in the upcoming days (between Rev. Billy updates --- Remember the day after Thanksgiving is [Buy Nothing Day](http://en.wikipedia.org/wiki/Buy_Nothing_Day) )

One of the attendees to the presentation mentioned that he could use generics to simplify parsing enums, and another said that he'd already done it.  It took me only a few moments to write the code, so I figured I'd share it with you:</p> <p>Presently, to parse a string into a enum value, you have to write it like this:</p>

	enum ABC {Able, Baker, Charlie};
	string str = "Charlie";
	ABC c = (ABC) Enum.Parse(typeof(ABC), str);


No big deal, but it's a bit ugly, and you have to specify the enum name twice, and if you type a mismatch, you get a run-time error.    Let's see how we can make this a bit prettier &amp; safer with generics.

	static class Enumr
	{
		public T Parse<T>(string value)
		{
			return (T) Enum.Parse(typeof(T), str);
		}
	}

Which would be used like this:

	enum ABC {Able, Baker, Charlie};
	string str = "Charlie";
	ABC c = Enum.Parse<ABC>(str);

This way is a bit easier to read &amp; type, and a mismatch specifying the enum names will cause a compile-time error.

Oddly, just this morning I found a link to a <a href="http://www.codekeep.net/snippets/5fd04f07-a8cc-445c-9fbe-a076cb133afd.aspx">page</a> giving a very similar method to convert a int to an enum, namely

	public static E ConvertIntToEnum<E>(int value)
	{
		return (E)System.Enum.ToObject(typeof(E), value);
	}

But that method is completely unecessary. You can convert an int to and enum (and an enum to an int), by simple casting: 

	ABC a = ABC.Able;
	int ai = (int) a;
	ABC aa = (ABC) ai;


<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/20/Generics-without-Collections_2C00_-Pt.-3.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2006/11/20/Generics-without-Collections_2C00_-Pt.-3.aspx" border="0" /></a>
>