---
layout: post
title: Thoughts on eXtreme Programming
tags: random-thoughts code programming
---

I recently started a new job.  (Leaving the old one wasn't my idea, but that's a story for another time).   This company is a computer equipment manufacturer, and will be releasing a new model in the coming months.  Which means all their drivers and other supporting software needs to be updated, largely to take advantage of the new features the new product has to offer. 
  
They've created a new team for this task. (That's related to the reason why I was hired, although I'm not actually on that team).  Since they are planning an aggressive release schedule for this product, the boss able to persuade upper management to try "eXtreme Programming"  (XP) for the team.
  
As I said, I'm not on that team, but I did attend a department-wide presentation on techniques being used by the team.  Which gives my an opportunity to give my opinion on a matter without having any first-hand experience (and what kind of a blogger would I be if I let that stop me?)
 
The first principle discussed was Test Driven Development (TDD) -- basically before you write any block on code, you are expected to have a test written for it, and run that test on your code after it is written (and after anything else is written or changed).  This is unquestionably a good thing, however, not entirely for the obvious reasons. 
 
Basically, it's very difficult to write a unit test (particular in test frameworks, like nUnit which is the industry standard), which says "launch this program, click that button, and see what text appears in this box".   It's far easier to write a test which says "Instantiate this object, call that method, and see what string is returned."  This pretty much forces you to move your business logic out of your GUI code and into separate objects--- Which is a good thing in itself.  Hence the major benefit of TDD is not the effect of it, but the side-effect of it.
 
Further, TDD is a fully formed concept in it's own right. The fact that it's been annexed by XP doesn't really change the fact that it can and does live on it's own as a development methodology.  You can't really judge the benefits of XP by the benefits of TDD, because you can separate them if you want.
 
Which bring us to the features of XP which are intrinsic to it.  The best known is paired-programming, where two programmers work together, basically one typing with the other looking over this shoulder - and we'll get to that in a minute, but first I want to talk about one of the more basic principles.
 
Essentially, when a developer is given the task to make a change to the existing code, he is expected to make the smallest change possible to accomplish the task, and not to make larger design changes, even if he sees that the old architecture cannot handle the features of the application it is evolving into.
 
The reason given for this is "Why waste time planning for some future feature, when 90% of the time, that new feature will never be implemented."
 
That was the first red-flag I spotted. These theoretic developers were misjudging what new features their users were going to want 90% of the time?   That seemed remarkably high. My ability to view a set of change requests and guess what requests will come next is far greater than 10% -- more in the 60-70% range.
 
This is when the light-bulb over my head lit up.  I now think I understand the guiding principle of XP:
 
##"XP assumes that all programmers are idiots"
 
Put in that light, the rest all makes complete sense.  The rule of always making the smallest change translates to "We know you are going to screw up the code, so touch as little of it as possible".
 
Similarly, developers aren't supposed to plan for new features, because they aren't bright enough to understand the architecture of the app, nor conceive what the end users wants.
 
This seemed to be confirmed to me by another statistic quoted in the presentation:  That using paired-programming, defects in code are reduced by 20%. Wait a minute --- you have two people looking over the code as it is being written, and they *still* miss 80% of the bugs?
