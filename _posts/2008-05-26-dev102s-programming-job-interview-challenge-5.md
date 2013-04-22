---
layout: post
title: DEV102's Programming Job Interview Challenge #5
categories: 
tags: 
---

  DEV102 announced the [correct responses to last weeks challenge](http://www.dev102.com/2008/05/26/a-programming-job-interview-challenge-5-records-sorting/) today.  Since I answered in my blog I got a prominent spot in their post, which is good considering I was one of about 10,000 correct answers and I gave it rather late in the process.  Hopefully, I can improve on both those areas this week.
  
  Two other things they messages showed.  1) It apparently takes a lot of hunting on this blog to figure out my name (**James Curran**), and 2) They like it if you hide the solution with HTML tricks.  Unfortunately, while last week's answer was three simple lines of code, this week's so going to be too long to do that effectively.

Anyway, on to [this week's puzzle](http://www.dev102.com/2008/05/26/a-programming-job-interview-challenge-5-records-sorting/):

  > *You are asked to sort a collection of records. The records are stored in a file on the disk, and the size of the file is much bigger than available memory you can work with. Assume a record size is 10 bytes and file size is 1GB. you read and write data to/from disk in units of pages where each page is 1KB big. You have few (less than 10) available pages of physical memory. Assume you are given a method to compare two records.*
  
  > *How can you sort the records under the described conditions?*
  
 The trick here is to do lots of merge sorts, but we get to that in a minute.  But first to prepare, we have to sort each page, hence, read one page, and sort it's records in memory.  Exactly how it's sorted is irrelevant. A page only has 100 10-byte records so Quicksort is probably overkill -- an Insertion sort is probably best. Then that page is written back to the file in place, and we more to the next one.
 
 When we finish sorting each page individually, then we begin the Mergesort.  Read two pages and merge them by comparing the first items in each list.  Once the new list fills a page, it's written to a new file.  Repeat this, and eventually, you have half as many segments, each two pages long. Then, do it all over again, merging two-page segments into four page segment.  The trick is, since the two-page segments are already sorted, they can be read in one page at a time, and the output can be written to disk as soon as a page is filled.
 
 Keep repeating the process, merge four-page segment into eight-page segments, 8-pages into 16-page, etc.  Eventually you will be merging two 512-page segments, and you're done.
  
  The process only requires three pages of physical memory, and we have a bit more, so some optimizing can be done, but that's essentially it.</p>
  
  