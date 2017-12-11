---
layout: post
title: An Exercise in Refactoring.
tags: code programming csharp
---
This post is the eleventh installment of the [2017 C# Advent Calendar](https://crosscuttingconcerns.com/The-First-C-Advent-Calendar) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

This article is based on a video on [DimeCasts.Net](http://dimecasts.net/) on the [Specification Design Pattern](http://dimecasts.net/Content/WatchEpisode/139).  DimeCasts was a great source of short videos on .NET and other development topics.  Unfortunately, they are no longer accepting new videos (and haven't been for a while). But the videos are still available and many of them, on general topics, like design patterns, are still timely.  So, if you got 10 minutes to spare, you might want to check out the Specification Pattern video cited above, before continuing. 

For the impatient, I'll give a quick recap.  Using the example of a online retailer trying to determine of an order could be rush-shipped, the author (Tim Barcz) proposes the criteria of a) must be shipped to the US, b) the order total is over $100, c) all items in the order are in stock, and d) none of the items contain hazardous material.

Without the specification pattern, coding that would look something like this:

<script src="https://gist.github.com/jamescurran/44d36426ff70d28f0410e91db4acabe3.js"> </script>

**_With_** the Specification pattern, that can be reduced to just this:

            var spec = new RushOrderSpecification();
            return spec.IsSatisfiedBy(this);

With `RushOrderSpecification` looking like this:
<script src="https://gist.github.com/jamescurran/b37a86130c2032310be1cbe9e59815d1.js"> </script>

The video goes on describing the `Specification<T>` framework, and the `DomesticOrderSpecification`,  `HighValueOrderSpecification`, `HazardousMaterialSpecification`, and `IsInStockSpecification` classes which do the actual work.

Which is the problem.  While the code in the `Order` class in reduced to two lines, you have to write *five* classes to accomplish that, and those five classes hold a lot of redundant code.

So, let's see if we can improve this.

(I posted the original code, and all my revisions to my github, https://github.com/jamescurran/Specification-Pattern with a check-in after each step, so you can compare the changes, by viewing the history)

Each of those classes takes the form of this:
<script src="https://gist.github.com/jamescurran/71fbf5cfa91cbb90f297d98829225544.js"> </script>
Note that there is just one really relevant line (the return statement), and the rest is all boilerplate.  And if you were to write a specification class where that line is more than one line, you are probably doing too much in it, and it should be broken up into multiple specifications.

Our first step will be to replace those classes with objects, which the relevant test injected as a lambda expression, so that :

    readonly HighValueOrderSpecification highValueSpec =
      new HighValueOrderSpecification();
    
becomes

    readonly Specification<Order> highValueSpec =
      Specification<Order>.With(candidate => candidate.OrderTotal > 100);

 To handle that, we create one new class to the framework:
 <script src="https://gist.github.com/jamescurran/8b4751faea1250416d6a6ef864c76da6.js"> </script>
 
 And add a static method to the Specifiction<> base class to simplify creating new specifications:
 
    public static SpecificationBuilder<TEntity> With(Func<TEntity, bool> conditional)
    {
        return new SpecificationBuilder<TEntity>(conditional);
    }

This allows us to forgot the `SpecificationBuilder` class even exists, and we can just work with `Specification<>`.


OK, that's all well & good... *but*, `IsSatisfiedBy` is in passive voice.  In coding, as in speech, one should prefer active voice. So, instead of 

            return spec.IsSatisfiedBy(this);

It's better to say

            return this.Satisfies(spec);
            
The code for `Satisfies()` would be the same for every code, so it would be simpliest to move it into a base class, *but* the derived classes would otherwise be unrelated, so requiring them to use a common base class would be a poor design choice (For one thing, it would prevent them from being derived from any other class, limiting the design)

An alternative would be it put the code into a extension method.  One small problem here.  Since the `this` object won't have a common base class (see above), we'd have to attach the extension method on `object` which would have the `Satisfies()` method appearing in the Intellisense for every object, even those which it's not appropriate.  To limit that, we'll create an `ISpecificationTarget<>` interface for those classes.  Forcing such a class to implement an interface (especially one like this, which will have no members, and so, no implementation), is a far less limiting design decision than a base class.

So we add:

    public interface ISpecificationTarget<TEntity>
    {
        // that's right, nothing here.
    }
    
Add a quick addition to the `Order` class:

    public class Order : ISpecificationTarget<Order>
    {   
        // Unchanged
    }
    

And finally, our extension method:

<script src="https://gist.github.com/jamescurran/45205344b3c73735a0418696eebec1cc.js"> </script>

So, now, we can write IsRush() as:

    public bool IsRush()
    {
        var spec = new RushOrderSpecification();
        return this.Satisifies(spec);
    }
    
    
OK, homestretch now.  We did all this based on the example of one needed specification ("Is this a rush order?"") made up of four criteria.  But in a real application we'd have many of each, with some of the specifications being used over and over. We have the top level specification made of member variables for each sub-specification.   Once we moved the specifications from classes to objects, that means if two different top-level specifications used the same criteria -- say "Does this order earn customer loyalty point?" also needed `HighValueOrderSpecification`. -- we would need to define it separately in our `LoyalPointsSpecification`, and we'd need to define it exactly the same way as we did in `RushOrderSpecification`.  We really need to define one common definition.

So, let's make each component specification public properties of a static class, like this:
<script src="https://gist.github.com/jamescurran/ce36e52f5132bee77596c6bef494031b.js"> </script>

And now we can make Order.IsRush() just:

    public bool IsRush()
    {
        return this.Satisifies(OrderSpec.RushOrder);
    }

PLus, if we then throw in a static using statement, 

    using static SpecificationPattern.Specifications.OrderSpec;

we can reduce that to 

    public bool IsRush()
    {
        return this.Satisifies(RushOrder);
    }
    
Which gives us the peak in clarity and readability.


