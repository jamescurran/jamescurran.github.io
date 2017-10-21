---
layout: post
title: DEV102's Programming Job Interview Challenge 4
tags: code c# .net programming dotnet csharp
---

The folks at Dev102.com are offering weekly programming challenges, where they offer questions, and let bloggers post about them.  I meant to write an answer for last week, but never got around to it.  Just as well -- my answer would have been wrong.  So, let's move on to [this week's](http://www.dev102.com/2008/05/19/a-programming-job-interview-challenge-4/):

  > *How would you implement the following method: Foo(7) == 17 and Foo(17) == 7. Any other input to that method is not defined so you can return anything you want. Just follow those rules:*
  
  - *Conditional statements (if, switch, ...) are not allowed.*
  - *Usage of containers (hash tables, arrays, ...) are not allowed.*
 
 My first thought was, since we can't use conditional statements, to use conditional *expressions* instead:
 
      int foo(int n)
      {          
          return (n==7)   17 : (n==17)   7 : n;
      }
      

But clearly, that is just cheating.  (And, besides, it turns out, it's not the best solution).

My next attempt was to use subtraction to create factors of zero.

      int foo(int n)
      {
          return n + (10 * (n-17) * (n-6)) + (-10 * (n-16) * (n-7))  ;
      }
      

There may be a formula in there that works, but that one doesn't, and before I found it, I stumble upon the *correct* solution, which was just sitting there staring me in the face:
  
    int foo(int n)
    {
     	return 24-n;
    } 
    
Yep, it's just that simple.

