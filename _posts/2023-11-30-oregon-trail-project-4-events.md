---
layout: post
title: The Oregon Trail Project - Part 4 - Trail Events
tags: code programming retro history opensource 
---

Oregon Trail Project
--------------------
 - Part 0 : [Intro](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)
 - Part 1 : [Markdown to Spectre Console ](https://honestillusion.com/blog/2023/11/26/oregon-trail-project-1-text/)
 - Part 2 : [Dates](https://honestillusion.com/blog/2023/11/27/oregon-trail-project-2-date/) 
 - Part 3 : [Data](https://honestillusion.com/blog/2023/11/29/oregon-trail-project-3-data/)
 - Part 4 : Trail Events  (this one)
  
 [Virtual Coffee's](https://virtualcoffee.io/) Blogging Challenge is on it's last day! And we're still quite a ways short of the 100K words goals, so we're gonna work this down to the last minute....

Ya'know, I think it about time that we actually get to some real *code* for this project.  An important element of gameplay are the occurrences that happen to you along th trail, which the source code refers to as "*events*", but which I'll call "*trail events*", to avoid confusion with delegates and the `event` keyword.

There are sixteen different things that could happen to you each turn, such as "WAGON BREAKS DOWN--LOSE TIME AND SUPPLIES FIXING IT" and "OX INJURES LEG---SLOWS YOU DOWN REST OF TRIP".  The selection of which trail event happens on any given turn is handled by this bit of code:

    3550  LET D1=0
    3560  RESTORE
    3570  R1=100*RND(0)
    3580  LET D1=D1+1
    3590  IF D1=16 THEN 4670
    3600  READ D
    3610  IF R1>D THEN 3580
    3620  DATA 6,11,13,15,17,22,32,35,37,42,44,54,64,69,95
    3630  IF D1>10 THEN 3650
    3640  GOTO D1 OF 3660,3700,3740,3790,3820,3850,3880,3960,4130,4190
    3650  GOTO D1-10 OF 4220,4290,4340,4560,4610,4670

If you're not familiar with Basic code, let me explain.  `RND(0)` return a random floating point value between 0 and 1, so R1 is between 0 and 100.  The `DATA` value on line 3620 correspond to the percentage chance for each trail event being selected. Each time the `READ` is executed, the next `DATA` value is assigned to the variable `D`.  `RESTORE` reset the `DATA` so the next `READ` would start again with 6.

So, first pass thru, `D1` is 1, `D` is 6, and `R1` is a random number [0..100). If that random number is less than or equal to 6, the `GOTO .. OF` lines will direct execution to the first trail event.  If it is greater than 6, it tries again with `D1` as 2, `D` as 11.

It tries this up to 16 times, with the value of `D` increasing each time until one hits.  On the 16th iteration, if the random number less than 100, is also less than 95, we go to trail event at line 4670 (line 3650).  On the next time thru the loop, we go directly to line 4670 ("HELPFUL INDIANS SHOW YOU WHERE TO FIND MORE FOOD").

So, the percentage chance of any particular trail event happening is the difference between it's corresponding value in the `DATA` line, and the value below it. Which means we can just use the difference, and the order doesn't matter. This makes the next step possible.

I want to isolate each of those trail event into individual class (and then organize them using .NET's [Managed Extensibility Framework (MEF)](https://learn.microsoft.com/en-us/dotnet/framework/mef/)).  This is major over-engineering, but that pretty much describes this entire project, so we're not going to let that stop is.  

A typical trail event looks like this:

    4560  PRINT "HAIL STORM---SUPPLIES DAMAGED"
    4570  M=M-5-RND(0)*10
    4580  B=B-200
    4590  M1=M1-4-RND(0)*3
    4600  GOTO 4710

To translate this into C# with MEF, first we define an Interface to be used by each of the trail events:

    public interface ITrailEvent
    {
        string Occasion(GameContext context);
    }

(I couldn't stand calling the method `Invoke`, which led to a trip to a thesaurus, hence `Occasion`.  That interface may change as this project develops. I think particularly the return value will change.)

This will make the code:

    public string Occasion(GameContext context)
    {
        context.Miles += Random.Shared.Next(-5, 5);
        context.Resources.Bullets -= 200;
        context.Resources.Misc -= Random.Shared.Next(1, 4);

        return "Hail Storm---Supplies damaged";
    }

Next, we have to mark this as part of our MEF system.  This involve adding a `[Export]` attribute giving the interface it implements.  Later we'll ask MEF to gather up all classes that export that interface.  But, also want to know what percent of time that trail event should occur -- before we actually load it.  For that we can use the `[ExportMetadata]` attribute.

    [Export(typeof(ITrailEvent))]
    [ExportMetadata("Weight", 10)]
    public class HailStorm : ITrailEvent
    {
        ...
    }

MEF will create an object on the fly with the metadata, but we'll need to define an  interface that represents it:

    public interface ITrailEventData
    {
        int  Weight { get; }
    }
    
Ok, we've get the piece there, now we have to work on loading and invoking them.  First we'll need a place to store them:

    [ImportMany]
    List<Lazy<ITrailEvent, ITrailEventData>> trailEventsImport;

And then you need to load it up.  Here we just take all trail event defined in the same assembly as the `GameContext` class, which is where I'll all the trail events defined in the original game.  But the advantage here is that we can add another `catalog.Catalogs.Add( )` line, pointing to a folder, and it will scan all the assemblies in the folder for more components, so we can extend this with many events, without ever having to recompile the game.

    var catalog = new AggregateCatalog();
    catalog.Catalogs.Add(new AssemblyCatalog(typeof(GameContext).Assembly));
    var container = new CompositionContainer(catalog);
    container.ComposeParts(this);

Next build a new list, replacing the individual weights, with a running total, so each gets it own piece of the total range.

    var trailEvents = new List<(int weight, ITrailEvent)>(trailEventsImport.Count);
    int totalWeight = 0;
    foreach (var item in trailEventsImport)
    {
        totalWeight += item.Metadata.Weight;
        trailEvents.Add(ValueTuple.Create(totalWeight, item.Value));
    }

Then to randomly select one, at the same frequency at defined in the original code:

    var val = Random.Shared.Next(totalWeight);
    var todaysEvent = trailEvents
        .SkipWhile(te => te.weight < val)
        .Select(te => te.evtTrail)
        .FirstOrDefault();

Then we just call that

    GameContext context = new GameContext();
    // :
    // :
    todaysEvent.Occasion(context);






