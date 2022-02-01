---
layout: post
title: Some Better-Written Custom String Methods using C#
tags: code c# .net programming dotnet csharp
---

  In my daily web-surfing, I often stumble upon snippets of C# code posted by people.  Usually, I can tweak  it a bit. Sometimes, I can tweak it a lot.  I usually post a quick comment to the site offering it.  Today, I came upon some code that was so bad --- which the author said was from his forthcoming ***book!***--- more drastic measures must be taken.

First we have a function to put a string into "Title Case" (which the author refers to as "Proper Case") - Having the first letter of each word capitalized.  Here's the original:

<script src="https://gist.github.com/jamescurran/5437884.js">   </script>

What wrong here? Lot's of really bad string handling.  Remember, strings are immutable, so any action on one creates a new string.  So, `strParam.Substring(iIndex, 1)` creates a new string. `strParam.Substring(iIndex, 1).ToUpper()` create two new strings, and `strProper += strParam.Substring(iIndex, 1).ToUpper();` creates three new strings.  And, that's within a loop.   And, since `Substring` is always used here to create a one-character string, it's easier to just use a char --- except apparently, this book author doesn't know how to.   Nor, doesn't he apparently know about `StringBuilder`.  Then, we get to the algorithm itself, where he does such bizarre things as pointlessly treat the first character as a special case, in two different places. 

Ok, now let's see the revision:

<script src="https://gist.github.com/jamescurran/5437912.js">   </script>

First we start with a string builder, preallocated to the size of the string we are building.  The method doesn't change the length of the string, so we know the length of the final string right from the start.

Next, since we are going to capitalize every letter after a period, and also the first letter, why not just pretend the mythical initial "last" character was a period?   Suddenly, the first letter is no longer a special case, and we still get what we want.

Then, we just loop through the letters, raising or lowering letter as we need. Note that it works on characters and not strings, and uses the build-in `IsWhitespace` method, instead of using a  hardcoded list of a subset of them.  A for() loop can in certain cases (however, not the one used in the original) be faster than a foreach(), but here I figured it was safe to sacrifice a tiny bit of speed for clearer code.

Next up, Reversing a String.  The Original:

<script src="https://gist.github.com/jamescurran/5437943.js">   </script>

Now, here the author might be able to earn a pass.  It's possible that somewhere in his book, he talks about recursive functions, and uses this as an example.  Then, if might be OK.  I mean, it is the only reason I can think of that someone might want a function which reverses a string - despite ubiquity of string reversing functions in libraries like this.  But, he presented it on his blog as a collection of string functions, so we'll have to judge them on that basis.  

Again we have lots of string manipulation to accomplish something simple --- where there are already methods built into the framework to handle such things:

<script src="https://gist.github.com/jamescurran/5437955.js">   </script>

**UPDATE:** One commentator (quite rightly) noted that my Reverse() method would only work for strings of ASCII characters and will fail if there are any Unicode characters. I knew that at the time, but I was hoping no one else would notice. The problem was I needed a method which converted a string into an array of characters, and being unable to find one in the CLR, I substituted a string to byte array method instead.   I guess this is one of those times where you just have to step away for a while and come back to it, because now, I found the right method in a few seconds:

<script src="https://gist.github.com/jamescurran/5437971.js">   </script>

And while my first version was a big improvement over the original, this is a big improvement over my first version, so you get double the benefits!

Next, we have a simple function to count the occurrences of a substring. 

<script src="https://gist.github.com/jamescurran/5437981.js">   </script>

The revision isn't much different but the subtle difference is important.  Instead of creating a new, shorter string to search, we tell it to just start looking after the last match.  Instead we now merely look at the string.

<script src="https://gist.github.com/jamescurran/5437995.js">   </script>

The next one has a special problem --- It doesn't do what it claims to do!

<script src="https://gist.github.com/jamescurran/5438000.js">   </script>

Now, it says that it should remove repeated spaces, so that there is only one space between words. However, what it actually does it to remove all spaces.  This gives us a problem: Should my rewritten function do what it claims to do, or what it actually does? I decided to give you one of each.

Duplicating the result is quite straightforward:

<script src="https://gist.github.com/jamescurran/5438013.js">   </script>

Writing a function to do what it is supposed to is a little more involved, but still simple:

<script src="https://gist.github.com/jamescurran/5438021.js">   </script>

We create a string builder the size of our source string, which would be the maximum size our trimmed string could be, and we set prevWS to true.  This way, it will remove all leading whitespace.  Then we just step through the string, character by character, appending that character to our new string if it's not a whitespace character, and appending a space for the first whitespace character found.  Note that this reduces all forms of whitespace (tabs, newlines, spaces etc) to a single space.  The original just worked on spaces. 

Finally, we have a function to determine is a string is a palindrome. 

<script src="https://gist.github.com/jamescurran/5438029.js">   </script>

Here the change is subtle (ignore the length calculations at the start, which are a trivial micro-optimization).

<script src="https://gist.github.com/jamescurran/5438044.js">   </script>

In this function, instead of comparing one-character long strings, I compare individual characters.  That change, by itself, cause a 4X speed improvement.

So, there you have it.  The article had more, but the other were trivial, and I couldn't make them any better.  And in case you think I'm all talk here, each of those rewrites was benchmarked to run 2 to 5 times faster than the original.
