---
layout: post
title: Throwing the book at em - Knowing Your  storage
tags: programming, dotnet, csharp
---

I just signed up for the 2018 C# Advent Calendar blog cycle --- and realized I’d hadn't written anything since the 2017 edition.  So, I figured I need a bit of a warm up before I get to the big one.   And since that one will also be dealing with memory issues, this will be a warm up in more ways than one.

In modern computers, we have various layers of memory where we store things.  How do they relate?

Let's start with the your CPU's Level III cache which is build right into the CPU chip.  It’s a couple  megabytes.  *Something stored here is like having the information you need in a closed book, sitting on your desk.*

Your CPU Level II cache is also right into the CPU chip.  It’s about 1 megabyte.  *Something stored here is like having the information in a book on your desk, opened to the right chapter. *

Next up is the Level I cache.  Also built into the CPU chip, it’s about 256KB.  *Something stored here is like having it in a book on your desk, opened to the right page.*

Closest in, we have the CPU’s registers.  These are the fastest memory of all, and the most limited: really only about 100 bytes.  *This is like having the information in a book, opened to the right page, with your finger pointing to it.*

Moving back out, the main memory of your computer is generally called just “RAM” and is measured these days in gigabytes (I remember the days when we measured in in kilobytes, but I’m old…).  *Something stored here is like having the book, closed, not on your desk, but sitting on a table nearby, in arm's reach.*

Information on your hard disk is like having it *In a book that's in a bookcase, across the room*.    (Windows' ReadyBoost is like using a retractable "grabber" arm to get the book from your seat).

And a cloud drive (or a webservice), is like have that information is *a book that's in a library across town.*

(If anyone wishes to throw any other form of storage into the metaphor, please add it in the comments below)
