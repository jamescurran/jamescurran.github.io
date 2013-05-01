---
layout: post
title: More Fun with C# Iterators:Take, Skip, TakeWhile, SkipWhile
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---
As I was reading <a href="http://gbarnett.org/archive/2007/03/08/linq-standard-query-operators-part-3.aspx">this article by Granville Barnett</a> on some of the new operators available on LINQ queries, I thought, "That's all well and good, but for the time being, we're living in a .Net 2.0 world.  I wonder if I could emulate those with just generics &amp; iterators "  As it turned out, it was quite easy.

First up is Take:  Given a collection, we want to return the subset which is just the first N items.  To handle this, we just return items while counting down to zero. When we reach zero, we stop. 

<script src="https://gist.github.com/jamescurran/5472376.js">    </script>

Next, is Skip.  This is essentially the reverse of Take: Given a collection, we want to return the subset which is everything after the first N items.  And handling Skip is essentially the reverse of Take as well:  We still count down to zero as we iterator through the collection, but we don't return anything until we reach zero.  (Of course, after we reach zero, we have to stop decrementing the count, or the if() will fail when skip equal -1.)

<script src="https://gist.github.com/jamescurran/5472384.js">   </script>

Now, once we have Skip, SkipFirst ([which we discussed before]({% post_url 2007-02-05-c-code-adding-skip-first-to-foreach%})) just becomes an instance of that. 

	static public IEnumerable<T> SkipFirst<T>(IEnumerable<T> enm)
	{
		return Skip(enm, 1);
	}

There is no simple way to expand SkipLast, which was also discussed in that previous article, beyond just one item, so we'll leave it's implementation.

A bit more advanced are TakeWhile and SkipWhile.  And, unlike the LINQ versions, we can't use lambda expression, so we'll have to make do with delegates.

The essence of TakeWhile is, given a collection, we return items until we reach one which fails a given predicate.  Note that we stop at the first failure we reach (even if there may be later item which pass the predicate test)

<script src="https://gist.github.com/jamescurran/5472393.js">    </script>


Final, this brings us to SkipWhile, which is the inverse of TakeWhile:  Given a collection, we skip items until one fails a given predicate, and then we return the rest (even though some of the rest may fail the predicate)
<script src="https://gist.github.com/jamescurran/5472422.js">   </script>

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/03/09/more-fun-with-c-iterators-take-skip-takewhile-skipwhile.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/03/09/more-fun-with-c-iterators-take-skip-takewhile-skipwhile.aspx" border="0" /></a>
