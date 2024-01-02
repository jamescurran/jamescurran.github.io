---
layout: post
title: Performance optimization of an if/else-statement
tags: code c# .net programming
---
[Mads Kristensen](http://madskristensen.net/post/Performance-optimization-of-an-ifelse-statement.aspx) wrote on the subject on if/else statements in C#, running time benchmarks on code such as this:

	private bool RunIf(string input)
	{
		if (input == "hello")
			return true;
		else if (input == "jelly")
			return true;
		else
			return true;
	}

I wrote an analysis in the comments in his blog, but I figured, why not used for my own blog? 

Well, the first thing you need to do is to get rid of the string comparison. I'm sure the time it takes to do those is swamping the time for the jump processing. Try using integers instead, and an Neil pointed out, return different things so that the compiler doesn't optimized it: 

	private int RunIf(int input) 
	{ 
		if (input == 1) 
			return 10; 
		else if (input == 2) 
			return 3; 
		else 
			return 314; 
	} 

Further, we have to actually do something with the returned value, to prevent the optimizer from just removing the entire loop: 
	
	int z = 0; 
	int test = 1; 
	for (int i = 0; i < 100000000; i++) 
	{ 
		z = i + RunIf(test) + z; 
	} 
	TimeSpan span = DateTime.Now.Subtract(start); 
	Console.WriteLine(span.TotalMilliseconds); 
	Console.WriteLine("meaningless final value: {0}", z); 

With those code changes, on my system, I get: 

 * test = 1: 515.6118 or 531.2364ms 
 * test = 2: 593.7348 or 609.3594ms
 * test = 3: 593.7348 or 609.3594ms
 
Analysis: 

 *  Clearly the resolution of the timer is 15.6246ms -- all the times are multiples of that value (ie, test =1 took either 38 or 39 ticks of the clock). 
 *  Test=2 & Test=3 took the same amount of time, as you'd expect, as it's just taking one branch or the other. 
 * Test=1 is faster because it only required one comparison, while test =2 & test =3 each required 2. 
 *  The second comparison (and the jump associated with it) took approx 78ms. Assuming the first comparison & jump also took 78ms, then we have approx 440ms overhead to pass the parameter and call the function. 

This does leave open the question why "jelly" & "other" had different values in the tests. The next step would be to look at the generated code using Ildasm.exe or Reflector, and see what it's doing differently.

Putting the strings back into the test harness, I was able to duplicate Mad's results. This makes no sense whatsoever. Consider, you can tell two string are different faster than you can determine that they are the same (looking at one letter from each versus looking at 5 letters from each). 

So, I dug a bit further. The trick is in how String::Equals is implemented. First it does a Object::ReferenceEquals, and then, only if that fails, does it do the character-by-character comparison. So, when you code it as he did, it's the same as: 

	const string Hello = "hello"; 
	// : 
	RunIf(Hello); 
	//: 
	if (input == Hello)	

Everything's pointing to the same string so the ReferenceEquals comes back true. So, in your example, here's what was happening: 

 * input = "hello": One reference comparison (true), one branch (not taken) and one return. 
 * input = "jelly": One reference comparison (false), one letter-by-letter comparison (false), one branch (taken), another reference comparison (true), another branch (not taken) and one return. 
 * input = "other" : One reference comparison (false), one letter-by-letter comparison (false), one branch (taken), another reference comparison (false), another letter-by-letter comparison (false), another branch (not taken) and one return. 
 
 To see the effect, change the test to :  

	string n1 = "je"; 
	string test = n1 + "lly"; 
	for (int i = 0; i < 100000000; i++) 
	{ 
		z = i + RunIf(test) + z; 
	} 
	
(It has to be like that. "je" + "lly" won't fool the compiler). This way, the "jelly" comes out a bit slower that "other". 
