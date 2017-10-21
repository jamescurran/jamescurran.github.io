---
layout: post
title: Dev102's Challenge 11 - Summing Numbers
tags: code dotnet csharp
---

My answer was acknowledged as correct for last week's challenge.  So, let's see if we can make it two in a row.  [This week](http://www.dev102.com/misc/a-programming-job-interview-challenge-11-summing-numbers/):

> *Given a list of n integers and another integer called m, determine (true / false) if there exist 2 numbers in that list which sum up to m.*
> *Example: 2,6,4,9,1,12,7 and m=14 -&gt; 2 and 12 sum up to 14, so the answer is true.*
  
This one is rather tricky.  There is no obvious (to me) solution.   I can see three viable methods, each with its own pros &amp; cons.
  
 * Method 1: We'll call this "brute force".  The obvious answer.  We add the values of list\[1\] and list\[2\],  then list\[1\] and list\[3\], then list\[1\] and list\[4\] and so forth, until we reach list\[1\] and list\[n\].  If we haven't found a match yet, we then move on to adding list\[2\] to list\[3\], then list\[2\] to list\[4\] etc. In code that would be: 
   
        for(int i= 0; i &lt;N-1; ++i) 
          for(int j= i+1; j &lt; N; ++j) 
             if (list[ i ] + list[ j ] == M)

The complexity for this would be Summation N which is officially `O(N * N)` (although it's closer to `O(N * N/2)` ).  However, that's the worst case: we can exit early as soon as we find a valid match, making the average case `O(N * N/4)`.  It's also important to note the basic operation that's being repeated (adding two number) is very fast.  Hence, this would be the winner for small values of N (and probably some very large "small values of N").

 * Method 2: Which we'll call the "bi-directional search".  This would clearly be the winner, except for it's precondition: start with a sorted list.  Add the first and last elements of the list (`list[1] + list[ N ]`).  If they equal to goal, we're done. If they are more than the goal, add the first and second-to-last elements(`list[1] + list[ N-1 ]`).  If they are less, then add the second element to the last element (`list[2] + list[ N ]`).  Continue this way, moving up from the front when the sum is too low, and down from the back when it's too high until you either find a match, or meet in the middle.  The complexity of this algorithm in O(N) --- for a sorted list.  We're given an unsorted list, which means we'd have to sort it first.  Sorting is, at best, `O(N*logN)`, making the total complexity `O(N*logN + N)`, which I believe is still less than method 1, but the basic task being done for the sort (comparing *and*  swapping) is much more expensive than for method 1.  Further, you must complete the sort before you can start the search, so you've taken the big hit before you get a chance for an early exit, making the average case O(NlogN+N/2), so this will win for large values of N, but they'd have to be *very* large values of N.

 * And, finally, Method 3, which we'll call the "partitioned search". Partition the list into two sublists, one with values less the M/2 and one with values greater than M/2. (The value equal to M/2 can be ignored, as we would need two of them, and we've been assured that the values in the list are unique).  If a solution exists, it will require one from each list; any combination of two from the lower sublist will be two low.  Any two from the upper list will be too high. So we just try every combination of one from each sublist.   In the worst case, that's `O((N/2) * (N/2))` or `O(N*N/4)`,  with an average case of `O(N*N/8)`.  To explain with examples: Say we are given a list of 10 numbers.  Method 1 would require 55 additions to try every possibility. If we partitions the list into sublists of 5 &amp; 5, then this method would require only 25 additions.  However, if the numbers break down so that the partitions are 8 &amp; 2, then we'd need only 16 additions.  And, as before, we can exit early as soon as we find a match.  Partitioning can be done in O(N): Start by pointing at the first and last elements, just as in Method 2. Seek forward looking for an element greater than N/2.  When one is found, seek from the end, looking for an element less then N/2. When you have one of each, swap 'em and continue.  When the pointers meet, the list is partitioned in place.  That required only one pass through the list, so the step is O(N), making the total for the average case `O(N*N/8 + N)`.
Overall, Method 1 wins for small lists, Method 3 for larger lists, and Method 2 for very large lists.  Since memory read/write efficiency is the last bottleneck (how many additions is one swap worth ) the exact point where one method passes another is heavily platform dependent.

In the back of my mind, two other ideas for other methods keep flowing through. The first tells me that there should be some way to accomplish this through summing the list.  This would be O(N), but I just can't think of a way to make it work.  The other involves building a matrix.  I'm pretty sure I could make that work, in a way that O(N) once the matrix is built, but building it would be greater the O(N) itself, so the whole task would be no faster than the means described above.
