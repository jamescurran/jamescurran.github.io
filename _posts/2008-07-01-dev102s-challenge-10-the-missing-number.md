---
layout: post
title: Dev102's Challenge #10 - The Missing Number
categories:  code dotnet csharp
tags:  code dotnet csharp
---

  I didn't actually skip last week's challenge for Dev102.  I did write up a solution.  I just forgot to post it.  It was wrong anyway.
  
  Well, no sense it looking backward... [This week's](http://www.dev102.com/net/a-programming-job-interview-challenge-10-the-missing-number/):
>*Your input is an unsorted list of n numbers ranging from 1 to n+1, all of the numbers are unique, meaning that a number can't appear twice in that list. ..One of the numbers is missing and you are asked to provide the most efficient method to find that missing number.*

And Shahar was right, it was rather easy.  We just add up the number we get, and subtract that value from the sum we should have gotten if the missing number wasn't missing.

	 int FindMissingNumber(IEnumerable<int> list) 
	{ 
		int actualSum = 0; 
		int expectedSum = 0; 
		int n = 1; 
		foreach(int i in list) 
		{ 
			actualSum += i; 
			expectedSum += n++; 
		} 
		expectedSum += n; 
		return expectedSum - actualSum; 
	}


Things to note:
  
* The complexity of the algorithm in O(n).
*  I  assured that complexity by using the least powerful collection interface (IEnumerable), and iterated through it only once.
*  I summed the expected total manually instead of an formula on list.Count, because for some collection types (such as a linked lists) Count is, by itself, O(n).
*  The method will work for an empty sequence, returning 1.