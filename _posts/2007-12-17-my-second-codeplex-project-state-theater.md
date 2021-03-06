---
layout: post
title: My Second CodePlex Project:State Theater
tags: 
---

Well, as I promised months ago, I've created my second CodePlex project.  Actually, I created months ago, while this site was down (as the artist I am, I think I'll refer to that as it's "black period"), But now that I'm back, I guess it's time to make a formal announcement.

The [State Theater Project](http://www.codeplex.com/StateTheater) is my attempt to completely rewrite my main web site ([www.njtheater.com](http://www.njtheater.com)).  The original (still at that web address for now) was written by me, mostly 10 years ago, in classic ASP.   The ASP pages are almost purely presentational -- they just read stuff from the database and display it on the screen, using a lot of questionable techniques (in some spots, I was formatting HTML in SQL select statements)

I added a few pages "recently" (i.e. within the last five years) in C#/ASP.NET.  These were interactive, and mostly hidden from view of most users.  Only registered users having certain roles saw them, and those pages allow those users to enter updates directly into the web site's database.

However, most of the theater companies just emailed the details of the shows, and hoped I found time to enter them.  (Especially, since most of the important info can't be entered via the web pages, so only I could do it).

The main problem is that I needed a Ajax-based drop-down listbox, because the database is heavily normalized, so when someone entered "William Shakespeare", I needed to know they really meant Person ID#74. 
 
Some years ago I paid good cash for one such control, which allowed me to get the few pages working that I did.  But it was rather limited (I still don't think it works with Firefox) and has a spotty update &amp; support history and in general wasn't letting me advance.  When the Ajax craze start two years ago, a number of new, better and **free** DDL controls became available, so I started to look into a new approach.
 
Originally, I planned on a complete ASP.NET approach, and started to work on that in the spring of 2006.  The first problem I had was my membership/roles data.  Membership was based on the FORUM_MEMBER table from Snitz Forums, which I had been using for the Forums section of the site. 

Now, Snitz is a fine forums packages, but it's also Classic ASP, but is getting kinda old -- it hasn't had a major update in year, and in fact, has had only two minor updates (most fixing security breaches) in the last 2 1/2 years.  But mostly, the problem with it is that it's membership system was intended to be used strictly for the forums themselves, and any allowance for it to be used as a general web site membership subsystem were clearly an afterthought.

What I wanted to do was to use the ASP.NET membership system that came with v2.0.  The trouble here was that Snitz stored passwords as a one-way hash.  There is no way to convert them back into the original password.  Which means that there's no way to more the account to the new database structure.  I'd have to assign new passwords to my 2000+ users.  That's wasn't workable.

Ah, but the ASP.NET membership system works on the provider model.  The SqlMembershipProvider that comes with 2.0 is just one example.  I could just write my own provider which used the existing table structure as a backing store.  I began this project in the summer of 2006. 

The next problem was my laptop.  I was doing most of the work my lunch hour and one the train to &amp; from work.  But my laptop was excessively old. (It was three years old when I bought it on eBay, and that was three years earlier).  It was taking much of the train trip just to awake from hibernation. I was just days away from buying a brand-spanking new laptop when I completely new problem arose -- I was laid off from my job.

Now, I bounced back from that fairly quickly --- but the time out of work chewed up the saving I was planning on using to buy the laptop--- but the new job gave me a laptop --- but the new job took away train ride to work (I had to drive there), and because I was consulting, I had to take short lunches.

Anyway, seven months later, that contract ended, and a new one began -- back on the train to NYC, and this time it was one long ride instead of two short ones, so I could get more done.  After a few weeks of saving, I finally got that new laptop, and I could finally complete the SnitzMembershipProvider which became the first CodePlex project, only a year after I had started it.

And so then I finally began the work on revising the web site. But, by this time, I'd become fascinated with [Castle Monorail](http://www.castleproject.org/monorail) and LINQ, and decided to try those for the web site.   Plus, as much of the graphic design was mixed with the code, I tried for a more generic, skinnable approach of putting everything in &lt;DIV&gt;s and using CSS for styling it.  Finally, I'm trying to make it generic enough so that some other group in a different state could take the code and create there own similar web site for their state -- with a completely different look, just by changing the CSS file.
 
For the Ajax work, and pretty much all the interactive forms, I'm using the <a href="http://extjs.com/" target="_blank">Ext.Js</a> library, except for large textboxes, for which I'm using <a href="http://tinymce.moxiecode.com/" target="_blank">TinyMCE</a>.

You can see what I've done so far as <a href="http://www.NJTheater.org">www.NJTheater.org</a>.
