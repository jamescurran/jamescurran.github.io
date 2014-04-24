---
layout: post
title: Throwback Thursday - C++ Iterator of the Week
categories: code cpp C++ programming 
tags: code c# .net programming 
---
The *Throwback Thursday* concept is to post on old picture on Facebook on Thursdays.  But we do code here, so let's post some old code: 

#The C++ Iterator of the Week
>These are from a series of articles showcasing different uses for STL style iterators in C++ which I wrote around the turn of the century. I originally posted them on Microsoft's C++ newsgroup, so they're rather Microsoft & MFC specific, but I think the tutorials accompanying them are useful in general. All source code presented here is copyright 1998-2002 James M. Curran, but may be used freely. All comment and suggestion are welcome.  

>My one bid to modernize these is to have put the code on Github: [https://github.com/jamescurran/HonestIllusion/tree/master/Iterators](https://github.com/jamescurran/HonestIllusion/tree/master/Iterators)
 
 0. **Circular Iterator**:  The common usage of an iterator is to have it start at the beginning and progress to the end. But what if you don’t want to get off the roller coaster? What if you want through the container over and over again, just like a circular buffer. For that, we need a “circular” iterator. 
1. **CView Iterator**: Two years after my initial foray we try this thing again.... Step through the CView object attached to a CDocument. 
 2. **POSITION**: IteratorBased on the CView Iterator, now we have an STL-style iterator templated to handle  any MFC container that uses a POSITION object. 
 3. **SerialString Container & Iterator**: Iterator through a series of variable length, NUL terminated strings. 

Articles describing these are on my website, [http://noveltheory.com/iterators](http://noveltheory.com/iterators)

>While you are at NovelTheory reading about the iterators, take a look around the rest of [NovelTheory.com](http://NovelTheory.com) -- I just gave it a face-lift!

