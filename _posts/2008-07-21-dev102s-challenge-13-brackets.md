---
layout: post
title: Dev102's Challenge 13:Brackets
categories: 
tags: 
---

Many people skipped last week's challenge (like I had planned to).  As it turned out, I was the only blogger to responded.

[For this week's challenge](http://www.dev102.com/2008/07/21/a-programming-job-interview-challenge-13-brackets/), they've gone back to a platform-neutral algorithm question:

>   *Your input is a string which is composed from bracket characters. The allowed characters are:'(', ')', '['. ']', '{', '}', '&lt;' and '&gt;'. Your mission is to determine whether the brackets structure is legal or not.*
     
The simple sentence answer is "use a stack, pushing on an open character, and popping on a close character".  There are a few other things to look out for, but that's the basic concept.  For the actual code, I bypassed any kind of library Stack class, since we wanted the most efficient and for the very limited needs of this function, I could jury-rig a faster one out of a char array.  Complexity is speed: O(N), space O(N)

<script src="https://gist.github.com/jamescurran/5444058.js">    </script>