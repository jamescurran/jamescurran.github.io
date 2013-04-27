---
layout: post
title: Generics without Collections
categories: code c# .net programming generics-without-collections
tags: code c# .net programming generics-without-collections
---
Having moved to C# after years of being a C++ programmer, I was quite happy when generics were added to C#. I could finally do some of the things I was doing before with C++'s templates. 

However, I noticed something about how C# programmers were using generics --- In fact, it was exactly the way C++ programmer were using templates when they were first introduced. 

One of the first books on C++ I read (circa 1991) gave this simple purpose for templates : "Allows creation of type-safe collection classes"  (unfortunately, I cannot find that book now, so I can't give a proper citation or a verbatim quote)

The problem is that while collection classes are the most obvious use of templates &amp; generics, they are only a small part of what can be done with them.   In fact, with the creation &amp; adoption of the Standard Template Library (now essentially Chapters 23, 24 &amp; 25 in the ISO C++ Standard), type-safe collection classes in C++ became, for the most part, a "solved-problem" --- one should never have to worry about writing a template for that again. A similarly with the System.Collections.Generic namespace (augmented perhaps by <a href="http://www.wintellect.com/Weblogs/PowerCollections10ForNET20RTM.aspx">Wintellects PowerCollection library</a>) one doesn't have to think about using Generics for a collection.   

So, what else are Generics good for?   Well, anytime you want to tie some functionality to a particular type, but don't know what that type is. Let's explorer some possibilities. Let's say you have Entity-Object model, where you have an object directly tied to an row in a database -- To borrow an example from a past job, let's say you have a Product class referencing a Product table.  One field in there is ManufacturerID -- a foreign key to the Manufacturer table, which naturally has a Manufacturer class. To find the name of the Manufacturer of a particular product, you'd have to take the ManufacturerID, and create a Manufacturer object using that ID (which queries the database).   Better would be to make Manufacturer a field in the Product object, so you can just say "myProduct.Manufacturer.Name.  You'd still need to do the work it read that object in, but it would be abstracted. 

*But,* you don't want to create that inner object (or, more accurately, you don't want to perform the SQL query needed to create that object), unless you actually use that object.  This is particularly true you had a number of foreign keys in the original object.  Hence you'd want to create a delayed loaded (aka lazy-loaded) object.   Essentially, we want to have a boolean associated for each object we don't want to load immediately.  In generic C# terms, this becomes: 

	using System;
	 
	struct DelayedLoad<T>
	{
		  private T    m_Value;
		  private bool m_Valid;
 
Since this is a struct, C# won't let us have a default constructor.  Fortunately, the implied behavior is exactly what we want.  The ctor would look like this:
 
	//    public DelayedLoad()
	//    {
	//          m_Valid = false;
	//    }
 
Next, we need to know if it's valid, and to set it's value. These are quite straightforward:
 
      public bool IsValid
      {
            get
            {
                  return m_Valid;
            }
      }
      public void Set(T value)
      {
            m_Value = value;
            m_Valid = true;
      }


 Now, we arrive at the important part, the getter:
 
      public T Get()
      {
            if (!this.IsValid)
                  throw new ArgumentException("Object needs to be loaded before use");
                  
            return this.m_Value;
      }
 
 Of course, instead of throwing an exception, you'd probably want something to automatically load the object there -- and since this is a generic (small "G" in this case) class, it would need to be a delegate, passed into the ctor.  And, if this were an article on delegates, I'd explain how to do it.  But, as this is an article on Generics (big G this time), that part is being left as an exercise for the student.  (If the student doesn't fell like exercising, I might write it up in the future).
But, we're not quite done. We want our class to be used in the place of it's inner class, so we need a bit more plumbing:
 
      public static implicit operator T(DelayedLoad<T> t)
      {
            return t.Get();
      }
      
      public override string ToString()
      {
            return this.Get().ToString();
      }
      
 With that implemented, the following works as expected:
 
<script src="https://gist.github.com/jamescurran/5471525.js">   </script>

<a href="http://www.dotnetkicks.com/kick/?url=http://honestillusion.com/blogs/blog_0/archive/2006/10/02/Generics-without-Collections.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http://honestillusion.com/blogs/blog_0/archive/2006/10/02/Generics-without-Collections.aspx" border="0" /></a>