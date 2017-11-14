---
layout: post
title: HumanNameParser - Parsing people's names.
tags: code csharp .net programming dotnet csharp 
---
# HumanNameParser - Breaking a name into it's parts.

Recently I needed to be able to take a list of names, and separate them into First Name and Last name. Of course, it's not quite as simple as that, because names come in all kinds of forms.  So, naturally, I googled to find an algorithm for the task.  Fortunately, I was able to find a near perfect tool to handle this, originally written Jason Priem.  Unfortunately, it was written in PHP.  So, I set about porting it to a more sensible language -- C#. (Now, you may want to argue whether or not C# is the best language for this task, but I think we can all agreee PHP definitely isn't.)

My first step was to define two classes, `Parser` and `ParsedName`,  to strictly enforce the Separation of Concerns.  The original PHP code also had two similar classes, but divided the methods used between them in the most odd way.   There was a class similar to ParsedName, but the methods to access the parts of the name from it were in the Parser class, which also had the high-level functions for extracting the names, but the low-level string utility function -- with had nothing specific to parsing the name -- were stuck in the Name class for no discernable reason.

I straightened that out.  `ParsedName` is a simple POCO/DTO object, with just a bunch of public properties, and all the work to parse the name is in `Parser`, which just has one public method you need to care about, `Parse()` which just takes a single string - the name you want parsed.

The methodology itself is quite good, and handles a number of formats you may be receiving names in, such as :

	John Q. Smith
	Smith, John Q.
	John Q. Smith, PhD
	Smith, John Q. Jr.
	William Carlos Williams
	J. Edgar Hoover
	B.J. Thomas
	Colin "Bomber" Harris
	de la Cruz, Ana M.
	Cher
	
And many other variations.

It works by a series of complicated Regex expressions.  (I tried improving the patterns, and only ended up breaking them, and having to go back to the originals.)

You use it like this:

            var parser = new Parser();
            ParsedName pname = parser.Parse("Björn O'Malley");

`ParesedName`  has the following properties :

        public string FullName { get; set; }

        public string LeadingInitial { get; set; }
        public string First { get; set; }
        public string Nicknames { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
    
`FullName` is the original string, and the others are set to an empty string if there is no corresponding part to the name.

`Parser` compiles Regex patterns in its static constructor, so, it's best if you crete just one to parse multiple names.

Code is available here: https://github.com/jamescurran/HumanNameParser
The original PHP version, by Jason Priem is here : https://github.com/jasonpriem/HumanNameParser.php

	
