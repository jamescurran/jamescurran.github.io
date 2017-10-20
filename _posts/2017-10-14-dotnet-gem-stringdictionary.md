---
layout: post
title: Forgotten .NET Gem --- StringDictionary
tags: code csharp .net programming dotnet csharp 
---

## Forgotten .NET Gem --- StringDictionary

In the olden days of .NET v1.1, before we had generics, when collections were liimited to arrays and ArrayList, Microsoft tried to fill the gap with a few niche collection classes, which it placed in the nearly forgotten `System.Collection.Specialized` namespace.  Most were superceded by generic version once .NET v2.0 came around, but they've always stay in the framework.  And one, `StringDictionary`, you should reconsider.
 
StringDictionary simply maps one string to another, exactly like `Dictionary<string, string>`.  However, there is one critical difference in it's API which makes all the difference.
 
You are supposed to access items in a Dictionary using the indexer operator (that is, the square brackets, "[]"). 
 
     var item = itemsDict[key];
	 
Except, you can't, because it `key` isn't in the dictionary, that throws an exception.  To protect against this, many developers wrap the access with a check:

      MyItem item;
	  if (itemsDict.ContainsKey(key))
	      item = itemsDict[key]
	  else
	      item = null;
 
But that means you have to look up `key` twice, which is a waste.  The only way to handle this efficiently  is with `TryGetValue`:
 
     MyItem item;
	 if (!itemsDict.TryGetValue(key, out item))
	      item = null;
	
which is cryptic and verbose.  In fact, C#7 added a new syntax, I'm pretty sure, *just* for that situation:
	
	 if (!itemsDict(key, out MyItem item))
	      item = null;

The reason for this is that the value of being return *might* be a value type, so operator[] can't return null.  `default(TValue)` would be possible as a error return value, except for the standard value types (int, float, decimal et al), `default` returns 0, which could be a legitimate value.  Throwing an exception is the only unambiguous way to show an error.

Which bring us to `StringDictionary`.  Since the value is always a string, indicating "Key not Found" with a null return is perfectly acceptable. So, we can go back to the simple ` var item = itemsDict[key];` form. We can even use `var` again!

"But", you say, "How often do you use a string to string dictionary?"   Well, I scanned the source code of my current project for `Dictionary<>`s.  Overall, 75% mappped a string to something, 66% mapped something to a string, and 50% mapped one sring to another.

"But", you say, "How much longer will it be part of the framework?"   It's been added to the .NET Standard 2.0, and is included in the .NET Core 2.0.

"But", you say. "How can I create one with LINQ?"    I've got you covered.  Just add this to your extension methods collection.:

<script src="https://gist.github.com/jamescurran/e058adff5a6610ed2608924383e2bb31.js"> </script>

