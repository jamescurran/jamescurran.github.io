---
layout: post
title: Implementing A Circular Iterator
categories: code c# .net programming
tags: code c# .net programming
---

  Many years ago, I wrote an article entitled "Implement A Circular Iterator" for *The VisualC++ Developer's Journal*.  (Unfortunately, VCDJ is now out of business, and it's successor, *[Visual Studio Magazine](http://www.ftponline.com/vsm/)* doesn't maintain an online archive of articles from that magazine.  Fortunately, I kept a [copy](http://www.noveltheory.com/iterators/Iterator_N0.htm) of it)
 
The essence of the C++ code is that given a templated collection, it will give you an iterator to would loop through the collection over &amp; over.
 
A bit more recently, some one wrote C# variant of this, and published it on [CodeProject](http://www.codeproject.com/csharp/circularlist.asp).  However, that one was a CircularList, which would be derived from a standard List.  In the [comments](http://www.codeproject.com/csharp/circularlist.asp?msg=1519678&mode=all&userid=2094#xx1519678xx), I created an IEnumerator class.  It could be used with any class the implemented IEnumerable, like this
 
 	static void Main(string[] args)
	{
		 int[] a = new int[] {1,2,3,4,5,6,7};
	 
		 foreach(int i in a)
			Console.WriteLine("{0}", i);
	 
		 Console.WriteLine("--------------------------");
		 int cnt = 0;
		 foreach(int i in new CircularEnumerator(a))
		 {
			 Console.Write("{0}", i);
			 ++cnt;
			 if (cnt == 30)
				 break;
		 }
	}



would print "12345671234567123456712".  Note the "break" in the above.  It's vitally important, because the foreach will never exit on it own.

But, that used the non-generic IEnumerator interface, and manually implement all the parts of the interface.  I figured that using an C# iterator it would be easier.  And, as it turns out, it was:

	static class Util
	{
	 public static IEnumerable<T> Circular<T>(this IEnumerable<T> coll)
	 {
		while(true)
		{
			foreach(T t in coll)
				yield return t;
		}
	 }
	}

This would be used exactly like the previous version, with a single line changed:

      foreach(int a in Util.Circular(ary))
      
---
**Update:** As I revisit these old posts, I tweak a bit.  Here I've made it an extension method.   Now it can be called like this:
      
      foreach(int a in ary.Circular())

This kinda makes the next paragraph irrelevant.

---

However, this bring up an important point.  Here I've put in into a static class named "Util".  Now, it would be a natural to put it into the same class as my [SkipFirst &amp; SkipLast enumerators]({% post_url 2007-02-05-c-code-adding-skip-first-to-foreach%}) I wrote about recently.  However, I put those into a static class named "Skip" which won't be appropriate for this.  However (part II), when I [wrote utility functions for enums]({% post_url 2006-11-20-generics-without-collections-pt-3 %}) I put them in a static class called "Enumr".  Now, do you (that's the collectively, blog-reading "You" --  i.e. answer in the comments) think it would be OK, to put both function dealing with enums *and* functions dealing with IEnumerators in a class called "Enumr"?


<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/28/implementing-a-circular-iterator.aspx"><img src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2007/02/28/implementing-a-circular-iterator.aspx" border="0" alt="kick it on DotNetKicks.com" /></a>
