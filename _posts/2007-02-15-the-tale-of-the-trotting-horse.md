---
layout: post
title: The Tale of the Trotting Horse
categories: 
tags: 
---

It was originally a GW-Basic program for MSDOS.  I'm not even sure where I got it from --- Possibly PC Magazine.  It drew five images of a horse running, and then flipped between them, demonstrating simple animation.  *Really* simple animation, but in the early 80's that was impressive on a PC. I believe the original source claimed that the images were drawn from the photos taken by [Eadweard Muybridge](http://en.wikipedia.org/wiki/Eadweard_Muybridge), which inspired the entire motion picture industry.  And while the horses do look similar, that one had a rider and this one doesn't.... Anyway, I remember hacking it, so that instead of one horse, it would show four.  I staggered the starting position among them so that they wouldn't appear in lock step.
  
But eventually, I moved to Windows, so I created a WinApp version of it in 1990, for Windows 3.0.

  [photo:4450]  Shown, actual size.
  
It was a simple a little app, which just sits in the corner of your screen, just endlessly running.  It reminded me of the Talking Heads' video "*Road to Nowhere*".
  
This led to a wee bit of fame, as it was included in the book *Windows Magic Tricks* by Judd Robbins (Copyright 1992, Sybex; probably out of print for years).  A reader of that book wrote to me to point out that, despite it's original name of "Galloping Horse", the horse was, in fact, trotting.
  
The trick to writing that was converting the image data, which was just an array of hex bytes, and was intended to be drawn on a DOS screen, into a Windows bitmap, but by borrowing a couple functions from Petzold's [amazon:type=Blended:search=157231995X] (Actually, it was my *autographed* first edition), I got it working.
  
Then I tried running two or more instances of it at the same time.  That got a bit monotonous, so I added a counter to figure out how many were running at once, and varied the background for each subsequent instance.  In 16-bit Windows, where there was virtually no memory protection, this was easy.

In his chapter on animation, Petzold describes using a timer to move the image, but warns that Windows only has 16 timers available, so he suggests using the idle time.  I tried that at first, but it made the horse run too fast, so I went back to a timer.  However, this limited me to only 16 Horse running at once.  This wasn't really a problem, but sometimes the urge to be robust just gets the best of you, so I needed to fix it.  I learned that if you have 16 copies running at once, it put enough of a strain on the system that the idle time method of animation works OK (provided that "the system" in question was a 386, but I digress), so I added an algorithm to try to create a timer, and if that failed, to fall back to using the idle time.

Cool trick: start 17 instances at once, so that the 17th is not using a timer.  Then kill them one at a time, starting with the first --- The last one keeps getting faster and faster.  Unfortunately (well, actually, "*Fortunately"*, but for other reasons), with Windows 3.1, Microsoft bumped the number of timers to 32, so that got to be more work.

When Windows 95 was introduced, I started to write a Win32 version, but never finished -- The improved memory isolation made duplicating the features a problem, and the 16-bit version still worked.

However, with Windows XP, the 16-bit subsystem complains about it every now and then, so I figured that time was right to go all the way, and write a .Net v2 / C# version.  As I started to write it, a found the older incomplete Win32 version, so first I finished that one.  I found a simple way to count the number of instances running (using a semaphore), but, since I didn't want to add a main menu to an application whose purpose is to be small and unassuming, the real trick was adding a menu item (for the About box) to the system menu from C#.

Now, you can have all three version [here](/files/Horse.zip).

Horse16.exe was written in C in 1990.
Horse32.exe is written in C++ from the same source as Horse16.  It's really a C program, except I can no longer deal with declaring all my variables at the top of the function, so I renamed the file with a .cpp extension.

Horse.exe is written in C#.

Horse16 &amp; Horse32 use the win.ini (remember that?) to store their information.  Horse uses the registry.

Due to the different method used to determine multiple instances, Horse32 &amp; Horse have an arbitrary limit of 100 instances at once.
