---
layout: post
title: First Non-Repeating Character - My Solution
categories: code C# csharp programming ASP.NET aspnet MVC javascriptloader
tags: code C# csharp programming ASP.NET aspnet MVC javascriptloader
---
 
Over on GeeksWithBlog.com, James Michael Hare posted a "Little Puzzler" : [First Non-Repeating Character](http://geekswithblogs.net/BlackRabbitCoder/archive/2015/03/09/little-puzzlers-first-non-repeating-character.aspx).  As I write this, he seems to have posted his solution, but that page isn't coming up for me, so before I see that, let me post mine.  

For the full problem spec, see James Michael's post linked above, but basically, given a stream (of unknown size) of characters (later clarified to upper case letters only), first of value & position of the first character that isn't repeated anywhere in the sequence.

This led to a lively discussion in the comments, and through them, after I'd formulated one solution, I was able to come up with a second.  So, let's get to them...

First we have the bits that are common to both and with the possible except of the `Marker` class, would be common to most solutions.

<script src="https://gist.github.com/jamescurran/d4e63b0bb1509c89ea35.js">   </script> 

The `ICharStream` interface was provided by Hare for the puzzle.  And `CharStream` is my simpleminded implementation of it.

My idea basically, was to keep a linked list of non-repeated characters, and store the list node (not just the value, but the whole node) in a HashSet.  Then as each new character comes into, I can quickly find the node it represents (lookup in the HashSet), quickly remove repeats from the linked list (because I have the node, with the front & back pointers), and quickly find the first remaining node (because it will be the head of the linked list).

Here is that implementation:

<script src="https://gist.github.com/jamescurran/703f7345c46d4a3ee598.js">   </script>

However, in the comments, one reader (using only the handle "Wizou"), suggested using an array to keep track of repeated characters.  This worked fine for one part of the problem, but left other requirement out:  You have to scan the entire array to first out which characters weren't repeated, and you'd still have no idea which was first.

However, when the range of characters was limited to just the upper case letters, the array become only 26 elements long, so scanning it (even repeated times) isn't really a problem, which lead me to this solution:

<script src="https://gist.github.com/jamescurran/3676fe66da63dd0dfaeb.js">    </script>
 