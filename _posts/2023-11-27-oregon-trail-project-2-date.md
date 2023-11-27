---
layout: post
title: The Oregon Trail Project - Part 2 - Dates
tags: code programming retro history opensource dates
---

Oregon Trail Project
--------------------
 - Part 0 : [Intro](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)
 - Part 1 : [Markdown to Spectre Console ](https://honestillusion.com/blog/2023/11/26/oregon-trail-project-1-text/)
 - Part 2 : Dates (this one)
  
 After spending nearly three weeks getting installment #1 out, I figured I should get the next one out quickly, especially since [Virtual Coffee's](https://virtualcoffee.io/) Blogging Challenge have only a few days left.

Fortunately, the next topic is one I can crank out fast: Fixing how the BASIC code handled dates.

It goes like this: given an integer number of weeks, return a string of the date that many weeks from the start of the trek (27-Mar-1847), in the form *MONDAY MARCH 29 1847*.

In the original, it took  quite a bit on rather monotonous code to handle this:

    1250  D3=D3+1
    1260  PRINT 
    1270  PRINT "MONDAY ";
    1280  IF D3>10 THEN 1300
    1290  GOTO D3 OF 1310,1330,1350,1370,1390,1410,1430,1450,1470,1490
    1300  GOTO D3-10 OF 1510,1530,1550,1570,1590,1610,1630,1650,1670,1690
    1310  PRINT "APRIL 12 ";
    1320  GOTO 1720
    1330  PRINT "APRIL 26 ";
    1340  GOTO 1720
    1350  PRINT "MAY 10 ";
    1360  GOTO 1720
    1370  PRINT "MAY 24 ";

That goes on for 50 lines --- in a program that's only 695 lines total --- that's over 7%.

Fortunately, .NET build date handling and format right into the framework.  All of that can be replaced by:

	public static string CurrentDate(int fortnights, int extraDays = 0) 
        => new DateTime(1847, 3, 29)
            .AddDays(fortnights * 14 + extraDays)
            .ToString("dddd MMMM d yyyy");

