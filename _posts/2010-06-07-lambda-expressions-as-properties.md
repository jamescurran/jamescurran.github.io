---
layout: post
title: Lambda Expressions as Properties
categories: code c# .net programming dotnet csharp codeproject
tags: code c# .net programming dotnet csharp codeproject
---
Peter recently caused a bit of a stir with his article “[Sometimes an enum is not the best idea](http://codebetter.com/blogs/peter.van.ooijen/archive/2010/05/30/sometimes-an-enum-is-not-the-best-idea.aspx)”.  In it, he had a very specific problem: When an enum is passed to a method as an Object, and that method converts it to a usable value by calling `ToString()`, you get that enum’s name and not it’s value.  And he gave a gave very specific solution to that problem.  Peter wanted an simple ad hoc solution that he could just drop into his code.  Peter’s idea was basically a quick fix to replace a bad design with something slightly less bad.   The problem arose when readers inferred that it was intended as a far more general solution, and pointed out different, more architectural solutions.  Bickering started in the comments.

I think the main problem is that Peter just want a quick fix to his existing design, and viewed a proper design as overkill for his simple needs.  However, since those simple needs were out of scope of his original purpose for the  article, he never got into specifics.  However, knowing the specifics would be key to knowing how complex the design needs to be.  This was the essence of Peter’s follow-up [article](http://codebetter.com/blogs/peter.van.ooijen/archive/2010/06/02/ask-first.aspx) (which he posted as I was in the middle of writing this)

So, let’s try looking at this problem to see if we can come up with a better design that still meets the goal of being simple.

In Peter’s design, he has a Suppliers class (which he occasionally refers to as the Company class), which has several instances --- one for each supplier.  There is also an enum, PublicIds with one entry for each supplier.  PublicIds was used in two places:  In switch statements, and as a parameter in an ADO.NET statement:

<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">void</span> DoWithCompany(PublicIds company)
{
    <span class="kwrd">switch</span> (company)
    {
      <span class="kwrd">case</span> PublicIds.Company1:
      <span class="rem">// Do something with Company 1</span>
      <span class="kwrd">break</span>;
      <span class="kwrd">case</span> PublicIds.MyCompany:
      <span class="rem">// Do something with My Company  </span>
      <span class="kwrd">break</span>;
      <span class="kwrd">case</span> PublicIds.ThatOtherCompany:
      <span class="rem">// Do something else</span>
      <span class="kwrd">break</span>;
    }
}
<p>cmdExists.Parameters.AddWithValue("@idSupplier", Supplier.PublicIds.MyCompany);</p>
</pre>


The second use was the real motivator of Peter’s article – The first was what caused all the hoopla on the blog.

Commenters proposed some general ideas, basically out of Object-Oriented Design 101 – Creating a new subclass for  each of the Suppliers, with overridden methods.  Peter fought back --- the code in the “do something” blocks is not part of Supplier, but is in other classes.  Subclasses would requiring moving non-related code to where it doesn’t belong.

Since Peter still hasn’t provided any real-world examples of his problem (which is reasonable, as they might involve proprietary material or just have too many dependencies to post meaningful excerpts), lets try to imagine some scenarios that might fit his description (namely that DoWithCompany is an example of  a method that “is a member of several quite different other classes and comes in many flavors”).

Ok, so let’s say we have a Order class, with a ProcessOrderToSupplier(Supplier sup) method.   Let’s further say that most of Order processing is the same for all suppliers, but some suppliers offer a discount (or surcharge) under some conditions.  Hence we need custom processing per supplier to handle the discount, but moving order handling into the Supplier class would clearly be wrong.

So, let’s try three different discounting schemes, and come up with three means of handling them in ways that are scalable, maintainable &amp; understandable.

 #1 – A flat discount rate on all items.
    That’s simple.  A discount rate property (getter only) in Supplier.  It could be an abstract property which is overridden in derived classes (one for each Supplier), or just set in Supplier’s constructor.  (Could be set to zero for those suppliers not offering a discount).
    
 #2 -  Several suppliers offering (free shipping on orders over $100).
       The “free shipping blah-blah-blah” part is handled entirely in the Order class.  The Supplier class just needs a way to say whether of not a particular supplier offers it.  This could be handled by a  Property as described above, or an attribute on the derived class, or by a marker interface (a marker interface has no members, and is just used in statements like:
<pre class="csharpcode">
  <span class="kwrd">if</span> (sup <span class="kwrd">is</span> IOffersFreeShipping)
     ProcessFreeShipping();</pre>

 #3 – One supplier offers 10% off total orders over $100. Another offers 15% off individual items over $25. And a third offers a $1 shipping surcharge on items over 20 lb.

Basically,  every supplier offers something different, and each is complex.  We’d think we’d need derived classes with overridden methods here to keep this object-oriented.  But that is exactly “the first step to multiple inheritance or an unmaintainable set of interfaces” which Peter was afraid of.

  *(My laptop has an interesting way of telling me that I haven’t saved in a while --- the battery falls out.  I’ve now – too late -- discovered Windows Live Writer’s auto-save feature.  Now let’s see if I can recreate what I just lost……).*

And besides that, creating a subclass specifically for one instance of a class just feels wrong --- particularly when we would be creating multiple subclass, one for each of several instances.  But how can be package instances of distinct functionality in individual instances of an object.

By using **Lambda Expressions as Properties**. 
<p>The idea is pretty simple.  We define a property, which, instead of returning some scalar value, returns a lambda expression (or pretty much any form of anonymous function).  The property works just like a instance method, except we can assign and change it at run-time.   If you’ve done any work in JavaScript, you’ve probably seen very much the same thing. 

Let’s look at some code.  We start by defining the Supplier class like this:</p>
<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">class</span> Supplier 
{
    <span class="kwrd">public</span> PublicIds Id {get; <span class="kwrd">private</span> set;}
    <span class="kwrd">public</span> <span class="kwrd">int</span> IdCode {get { <span class="kwrd">return</span> (<span class="kwrd">int</span>) Id;}}
    <span class="kwrd">public</span> Action&lt;OrderItem&gt; PerItem {get; <span class="kwrd">private</span> set;}
    
    <span class="kwrd">public</span> Supplier(PublicIds id, Action&lt;OrderItem&gt; perItem)
    {
        Id = id;
        PerItem = perItem;
    }
    
    <span class="kwrd">public</span> Supplier(PublicIds id) : <span class="kwrd">this</span>(id, oi=&gt; oi=oi)
    {}
}</pre>
<p>Note that the constructor takes a Action delegate, and stores it in the PerItem property.</p>
<p>Next, define our instances like this:</p>
<pre class="csharpcode">Supplier Company1 = <span class="kwrd">new</span> Supplier(PublicIds.Company1);
Supplier MyCompany = <span class="kwrd">new</span> Supplier(PublicIds.MyCompany, oi=&gt; { <span class="kwrd">if</span> (oi.Price &gt; 25m) oi.Discount = oi.Price * 0.15m;});
Supplier ThatOtherCompany = <span class="kwrd">new</span> Supplier(PublicIds.ThatOtherCompany, oi=&gt; {<span class="kwrd">if</span> (oi.Weight &gt;=20) oi.Shipping = 1m;});</pre>
<br />
<pre class="csharpcode">List&lt;OrderItems&gt; items = <span class="kwrd">new</span> List&lt;OrderItems&gt;();    
<span class="kwrd">void</span> ProcessOrderToSupplier(Supplier sup)
{
    <span class="rem">// :</span>
    items.ForEach(sup.PerItem);
    <span class="rem">//:</span>
}</pre>
<p>“Ok“, you say. “We’ve got the distinct behavior without subclasses – but we still have order processing code defined outside of the Order class, and to make matters worse, it moved to the instance definition in a really messy way.”

No Problem, we just go back to out (new) old friend: Lambda Expression as Properties:

This time, we’ll create static properties in the Order class:
<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">class</span> Order
{
    List&lt;OrderItem&gt; items = <span class="kwrd">new</span> List&lt;OrderItem&gt;();    
    <span class="kwrd">void</span> ProcessOrderToSupplier(Supplier sup)
    {
        <span class="rem">// :</span>
        items.ForEach(sup.PerItem);
        <span class="rem">//:</span>
    }    
    
    <span class="kwrd">public</span> <span class="kwrd">static</span>  Action&lt;OrderItem&gt; Discount15on25
    {
        get
        {
            <span class="kwrd">return</span> oi =&gt; 
                { 
                    <span class="kwrd">if</span> (oi.Price &gt; 25m) 
                        oi.Discount = oi.Price * 0.15m;
                };
        }
    }
    
    <span class="kwrd">public</span> <span class="kwrd">static</span>  Action&lt;OrderItem&gt; Shipping1Over20
    {
        get
        {
            <span class="kwrd">return</span> oi=&gt; 
                {
                    <span class="kwrd">if</span> (oi.Weight &gt;=20) 
                        oi.Shipping = 1m;
                };
        }
    }
}</pre>
And we can now define out Supplier instances like this:

<pre class="csharpcode">Supplier Company1 = <span class="kwrd">new</span> Supplier(PublicIds.Company1);
Supplier MyCompany = <span class="kwrd">new</span> Supplier(PublicIds.MyCompany, Order.Discount15on25);
Supplier ThatOtherCompany = <span class="kwrd">new</span> Supplier(PublicIds.ThatOtherCompany, Order.Shipping1Over20);</pre>
So, what have we accomplished?

* We’ve distinct behavior amongst the different suppliers, without resorting to subclasses or switch/case blocks 
* We’ve all the Order processing code in the Order class.

* We’ve neat &amp; tidy Supplier instance definitions, which nevertheless indicates the special features of that supplier. 

  <li>And we have those special features as part of the Supplier object. </li>
</ul>
<p>But, wait.  You’re probably saying that those properties we’ve defined on Order are rather specific to one particular trait.  Would it be nice to be able to have one general algorithm which is customizable?  That can be done too, by just extending the general principle, which involves using a method instead of a property, but the basic idea is still the same:  instead of returning a value, we return a function:</p>
<pre class="csharpcode">
  <span class="kwrd">public</span> <span class="kwrd">static</span>  Action&lt;OrderItem&gt; ShippingByWeight(<span class="kwrd">int</span> pounds, <span class="kwrd">decimal</span> cost)
{
    <span class="kwrd">return</span> oi=&gt; 
        {
            <span class="kwrd">if</span> (oi.Weight &gt;=pounds) 
                oi.Shipping = cost;
        };
}
<p>// :</p>
<p>Supplier ThatOtherCompany = <span class="kwrd">new</span> Supplier(PublicIds.ThatOtherCompany, Order.ShippingByWeight(20,1m); </p>
</pre>
>
<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2010%2f06%2f07%2flambda-expressions-as-properties.aspx">
  <img border="0" alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2010%2f06%2f07%2flambda-expressions-as-properties.aspx" />
</a>