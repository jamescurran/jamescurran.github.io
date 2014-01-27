---
layout: post
title: The SOLID Buzz-Fuzz
categories: code csharp .net programming dotnet csharp buzzfuzz
tags: code csharp .net programming dotnet csharp buzzfuzz
---
Buss-Fuzz has become a staple of job interview screening in the last few years.  A few weeks ago, I posted the simplest version of it I could -- [one line of LINQ code](http://honestillusion.com/blog/2013/08/30/Buzz-fuzz-in-linq/)), and at the time, I mentioned my version of Buzz-Fuzz following the SOLID principles.  And so, it's time for that...

> *Quick recap for the few unaware of Buzz-Fuzz: The goal is to write a program which will display the numbers 1 through 100, but replacing any number which is a multiple of 5 or contains a 5 with "Buzz", any which is a multiple of 7 or contains a 7 with "Fuzz", and any which are both with "BuzzFuzz".*  
> So "`1 2 3 4 Buzz 6 Fuzz 8 9 Buzz 11 12 13 Fuzz Buzz 16 Fuzz"` etc.

And the SOLID Principles go like this:

### S - Single responsibility principle (SRP)
#### A class should have only a single responsibility.

At first I thought this was going to be a gimme, as I was only writing one class and it was only going to do one thing. The I realized it's actually doing two things: The count itself, plus building the parameters for the count.  However, I don't feel the latter truly counts as a responsibility, so I'm going to let it slide.  If you disagree, make your case in the comments, and maybe I'll rewrite it.

### O - Open/closed principle (OCP)
#### "software entities...should be open for extension, but closed for modification".

Fulfilling this one caused the problem in the previous paragraph.  First of all, I'm not sure how to make it "closed for modification".  I could make it `sealed` but that just seems rude.  The only way I could think of is to make it "open for extension" to the point that there is no reason for modifications.  To that end, you can specific the starting &amp; ending numbers, and add additional substitutions (e.g., replace 8s with "Wow").  I wrote a fluent interface for this, which added a number of methods to the class.  I could have done the same thing with a couple overloads to the constructor, and that would probably keep the people complaining about the SRP violation happy, but whether I set the values via the ctor or the fluent interface doesn't fundamentally change the utility of the object, so I figure I'm on solid ground (pun not intended, but I still left it after I realized it...)

### L - Liskov substitution principle (LSP)
#### "objects in a program should be replaceable with instances of their subtypes without altering the correctness of that program".

### I - Interface segregation principle (ISP)
#### "many client-specific interfaces are better than one general-purpose interface."

I'm going to do these together, since their related here.  Once configured, the object will have one task -- to return the next item in the sequence (as a string) when called upon.    I would create an interface for that, except there already exists such an interface.  It's called` IEnumerable<string>`.  It's very specific to the task at hand.   So, all I need do is implement that, and we've handled ISP, and, since it can how be used anywhere we need an `IEnumerable<String>` (such as anything using LINQ), taking care of LSP.

### D - Dependency inversion principle (DIP)
#### One should "Depend upon Abstractions. Do not depend upon concretions."
 
OK, home stretch now.    This is a bit tricky, as the principle is intended more for overall systems rather than individual components.   A small class like this is rather self-contained, so it's not really "depending" on anything.  It's something that other components depend upon, so we just need to allow them to depend on it via abstraction, which we've largely covered in the last section.   Once a BuzzFuzz object is configured, it can be passed around as a parameter, or stored in an IoC container, until it's called upon to produce it's sequence.


### And now, the code....
As promised, we define the class as implementing IEnumerator of string.  I cheated a tiny bit and made it also an IEnumerable, because you really need one of each, and it's such a minor interface, it's easiest it stick it here.

	public class BuzzFuzz : IEnumerator<string>, IEnumerable<string>
       {
 
Next, we have a simple internal class defined within BuzzFuzz class, to hold the word substitutions:

		internal class Substitution
		{
			public int Digit { get; set; }
			public string Word { get; set; }
		}

 We'll hold a list of them, pre-populated with "5"/"Buzz" and "7"/"Fuzz".   And, through the fluent interface, we can add more and/or remove those.
 
 		private int _num;
		private int _start = 1;
		private int _end = Int32.MaxValue;
		private readonly List<Substitution> _substitutions = new List<Substitution>();

Note that `_end` is preset to MaxValue, so until you set the ending  value, it'll keep going for a long time.
 
 The constructors are straightforward:
 
 		public BuzzFuzz(int start)
		{
			_start = start;
			_substitutions.Add(new Substitution {Digit = 5, Word = "Buzz"});
			_substitutions.Add(new Substitution {Digit = 7, Word = "Fuzz"});
			Reset();
		}

		public BuzzFuzz() : this(1)
		{
		}

So, a default constructor BuzzFuzz object, will start at 1, will make the two standard replacements, and continues forever (close enough). 

Finally we get to the real meat of the class:

		public bool MoveNext()
		{
			Current = _num.ToString(CultureInfo.InvariantCulture);
			bool showWord = false;
			var sb = new StringBuilder();
			foreach (var sub in _substitutions)
			{
				if ((_num%sub.Digit == 0) ||
				    (sub.Digit < 10 && Current.Contains(sub.Digit.ToString(CultureInfo.InvariantCulture))))
				{
					sb.Append(sub.Word);
					showWord = true;
				}
			}

			if (showWord)
				Current = sb.ToString();

			++_num;
			return _num <= _end;
		}

`MoveNext` has three tasks to do: Advance to the next item in the sequence, Set the object `Current` to the value, and Return true/false as to whether there are any more in the sequence.

Since `_num` starts at the first "next" value, the actual advance come at the end, after we've set `Current`.  And we begin by setting `Current` to the current `_num` value (as a string.  It's the default value we use if none of the substitutions fire, and we'll need it represented as string, so we lose nothing by setting it up front.

Then we test each substitutions, first for a multiple and then (if it's just a single digit) if that digit is contained in the string.

Here is the implement of IEnumerable.  It's funny that this is probably a bigger violation of the SRP then the fluent interface, even though, after the boilerplate, it's just very simple one line.

		public IEnumerator<string> GetEnumerator()
		{
			return this;
		}
 
 
 The oft-mentioned fluent interface is rather straightforward.   The key is to return `this`, so that the method can be chained together.
 
 		public BuzzFuzz Add(int digit, string word)
		{
			_substitutions.Add(new Substitution {Digit = digit, Word = word});
			return this;
		}

		public BuzzFuzz StartAt(int start)
		{
			_start = start;
			Reset();
			return this;
		}

		public BuzzFuzz StopAt(int term)
		{
			_end = term;
			return this;
		}

		public BuzzFuzz Clear()
		{
			_substitutions.Clear();
			return this;
		}

 One thing to remember is that `StopAt` set the **exclusive** end point ( The enumeration stops before it return that value.  This is consistent with C++ iterators (which C# enumerator are modelled after), but I really did it that way because it was easier.
 
 We end with a little bit of fluff I throw in because I thought writing `foreach(var num in new BuzzFuzz())` looked ugly.   So I added a class who entire purpose is just to create a BuzzFuzz object, neatly:
 
 	internal class Buzz
	{
		public static BuzzFuzz Fuzz
		{
			get { return new BuzzFuzz(); }
		}
	}

So now we can write `foreach( var num in Buzz.Fuzz)`   Or with the full fluent syntax:

    foreach(var num in Buzz.Fuzz.StartAt(1).EndAt(100).Add(6, "Sass"))
    
 
 
 Full code in github:  [https://github.com/jamescurran/HonestIllusion/tree/master/SOLIDBuzzFuzz](https://github.com/jamescurran/HonestIllusion/tree/master/SOLIDBuzzFuzz)

<a rev="vote-for" href="http://dotnetshoutout.com/The-SOLID-Buzz-Fuzz-HonestIllusionCom"><img alt="Shout it" src="http://dotnetshoutout.com/image.axd?url=http%3A%2F%2Fhonestillusion.com%2Fblog%2F2013%2F12%2F19%2Fsolid-buzzfuzz%2F" style="border:0px"/></a>