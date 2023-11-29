---
layout: post
title: The Oregon Trail Project - Part 3 - Data
tags: code programming retro history opensource 
---

Oregon Trail Project
--------------------
 - Part 0 : [Intro](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)
 - Part 1 : [Markdown to Spectre Console ](https://honestillusion.com/blog/2023/11/26/oregon-trail-project-1-text/)
 - Part 2 : [Dates](https://honestillusion.com/blog/2023/11/27/oregon-trail-project-2-date/) 
 - Part 3 : Data  (this one)
  
 [Virtual Coffee's](https://virtualcoffee.io/) Blogging Challenge has just a couple days left, and we're still quite a ways short of the 100K words goals, so lets see how many I can get out before time runs out...

One of the four principles of Object-Oriented Programming is *Encapsulation*, also known as (well, closely related to) *Data Hiding*.  Basically, bundling all the data used by one section of the application together, and hiding it away from the rest. 

Programs written for computer languages like the HP TIME-SHARED BASIC used for this version of Oregon Trail are pretty much why that tenet was added.

In Basic compilers of that era (actually, most were interpreters) **ALL** variables were global.  (In C# these days it's actually impossible to have an actual global, and even simulating one is difficult --- and very much frowned on). And variable names could only be a single letter, or a single letter followed by a single digit.  Numeric variable were all floating point (no true integers). String variables were available, indicated by a trailing dollar sign (e.g. `A$`).

This led to such a mess in most programs, that the Oregon Trail source code helpfully include a map in the comments at the end.

    6470  REM ***IDENTIFICATION OF VARIABLES IN THE PROGRAM***
    6480  REM A = AMOUNT SPENT ON ANIMALS
    6490  REM B = AMOUNT SPENT ON AMMUNITION
    6500  REM B1 = ACTUAL RESPONSE TIME FOR INPUTTING "BANG"
    6510  REM B3 = CLOCK TIME START OF INPUTTING "BANG"
    6520  REM C = AMOUNT SPENT ON CLOTHING
    6530  REM C1 = FLAG FOR INSUFFICIENT CLOTHING IN COLD WEATHER
    6540  REM C$ = YES/NO RESPONSE TO QUESTIONS

(that continues on for another 25 lines)

So, one of our primary tasks as we port the code is fixing all that.  I'll give each variable a real name, and limit it's scope to just the locality where it's used.

The first thing done when playing the game, is that you are given some money ($700), and you allocate that to supplies you will need along the trip (Oxen, Food, Clothes, Bullets, and Miscellany plus remaining Cash).  These fluctuate over the course of the game, and their level determine if you win or lose, so I figure they belong in an class of their own. 

	public class Resources : IFormattable
	{
		public int Oxen { get; set; } // 200-300
		public int Food { get; set; } // 0 -
		public int Bullets { get; set; } // 0-
		public int Clothes { get; set; } // 0-
		public int Misc { get; set; } // 0-


Each turn begins by running through this block of code:

    1750  IF F >= 0 THEN 1770
    1760  F=0
    1770  IF B >= 0 THEN 1790
    1780  B=0
    1790  IF C >= 0 THEN 1810
    1800  C=0
    1810  IF M1 >= 0 THEN 1830
    1820  M1=0
    1850  F=INT(F)
    1860  B=INT(B)
    1870  C=INT(C)
    1880  M1=INT(M1)
    1890  T=INT(T)
    1900  M=INT(M)

If it's not obvious to you, that ensuring that the levels of Food (`F`), Bullets (`B`), Clothing (`C`) and Miscellany (`M1`) are at least zero, and then truncates each of them as well as the leftover Cash (`T`) and distance travels in Miles (`M`) to whole numbers.  Since C# allows us to use true integers, we don't need the second part, but the first section needs to be moved to a member method of our Resources class:

		public void Clean()
		{
			if (Oxen < 0) Oxen = 0;
			if (Food < 0) Food = 0;
			if (Bullets < 0) Bullets = 0;
			if (Clothes < 0) Clothes = 0;
			if (Misc < 0) Misc = 0;
		}


Now, before we move on, I must tell a story from programming assignments past.  In the early 1990s, I was working for a company that was very much mainframe-centered, and very reluctant to change. They had started introducing PCs, and had a PC development team, but you can tell they weren't happy about it.  Their IT department had just approved Windows 2.0 for use on office machines, despite Windows 3.0 having been out for over a year.

We (myself and another new hire) were supporting one application and developing another application in C.  The first app was written by a few of the existing developers, where it was their first C program after years of working in Fortran for a mainframe.  It was a paragon of monolithic spaghetti code.  At one spot, I found a 100-line `if()` block, inside a 200-line `for()` loop, inside a 300-line `while` loop, inside a 400-line `case` statement, inside a 1000-line `switch` block.  The idea of a simple function to do one small task was completely alien to them -- partly because any code "entry-point" (which in Fortran would be the program start, but in C was a function), require *literally* **seven pages** of documentation.

The point of my reliving nightmares of the past, is this: The project leader on writing the new application was one of those hold-overs from Fortran.  But, he'd been trying to keep up with software development trends, and knew global variables were bad, and he wanted to minimize them as much as possible in the new code -- but he didn't quite get it.  

In his design, each section of the application would define a `struct` to hold all the data needed for that section.  These structs would hold dozens of unrelated variables, which were essentially just a bunch of globals for that module.  But, at least they were localized.

But it gets worse.  All of these structs were allocated at program startup, and pointers to them were stored in another struct, which we called the `SAAB`, which was officially the "Structured A{something} Allocation Block", but was really just named after my car. The we had one global variable, `pSAAB`, which held a pointer the that struct.

The project leader was overjoyed.  He'd had reduced the application to just a single global variable.

But that was nonsense.  The program had hundred of global variables, just like it's predecessor; we had just renamed them to something long & complicated.  But whether you call something `WidgetThingee` or `pSAAB -> pWidgetBlock -> pThingeePtr`, if it's globally accessible, it's a global variable.

Which brings us back to the present (where we are dealing with code even older than we were in the above).  We now have to create the classes used by the rest of the code, and in the original, everything was a global, so every variable was used all over the place.  We have to resist the urge to create a class that just holds every variable, and pass that object around like a global.

The heart of the game are 16 "events" (Wagon breaks down, bandits attack, etc) which could happen to you along the trail. In our final design, each of those will get its own class (actually, a whole module, but that's getting ahead of ourselves), so we'll need a game context object which can be passed to and from any of the Event classes, holding all the information that class would need about the status of the game, but no more, lest we fall into the `pSAAB` problem of yore.

After a bit of analysis, I came up with this:

    public class GameContext
    {
        public Resources Resources { get; set; }
        public int TurnCounter { get; set; }

        public bool ClearedSouthPass { get; set; }       //  REM F1 = FLAG FOR CLEARING SOUTH PASS
        public bool ClearedBlueMountains { get; set; }   //  REM F2 = FLAG FOR CLEARING BLUE MOUNTAINS
        public bool  ClearSouthPassSettingMileage  { get; set;}   //  REM M9 = FLAG FOR CLEARING SOUTH PASS IN SETTING MILEAGE

        public bool AreInjured  { get; set;}   //  REM K8 = FLAG FOR INJURY
        public bool AreIll  { get; set;}       //  REM S4 = FLAG FOR ILLNESS
        public bool HadBlizzard  { get; set;}  //  REM L1 = FLAG FOR BLIZZARD
    }

And we start on those Event modules (using [MEF](https://learn.microsoft.com/en-us/dotnet/framework/mef/) !)  next time.
