---
layout: post
title: How can I easily log a message to a file for debugging purposes 
tags: code c# .net programming mvp
---

Today, either Bloglines.com or blogs.MSDN.com blinked, and suddenly I'm seeing old entries on the 'C# Frequently Asked Questions' blog as new.  No one has posted anything there in over two years. 

Anyway, reading the most recent message, it offered a method for logging a message.  Now, ignoring side debates over log4net vs nLog vs. the Event log vs. Trace, and just concentrating on the code post --- It's not very good.  Look for yourself (then come back here) http://blogs.msdn.com/csharpfaq/archive/2006/03/27/562555.aspx

Despite being written by a C# MVP ( * ), it shows a lack of understanding of basic .Net library features, and well as C#.  So, I figured, I'd rewrite it.  I limited myself to taking what's there and fixing it instead of going an entirely different way (ie, log4net vs nLog vs etc).  Here's the revised code:

	using System.IO;
	public void LogMessageToFile(string msg)
	{
		string logFilepath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"),"My Log File.txt");
		using (StreamWriter sw = File.AppendText(logFilepath))
		{
			string logLine = String.Format("{0:G}: {1}.", DateTime.Now, msg);
			sw.WriteLine(logLine);
		}
	}

A few years ago, I bought a number of books, all with the title of same variation of "C# Cookbook".  Again, a lot of bad code.  I've been meaning to start a series of articles about revising them.....

( * ) Years ago, I was one of the very first C++ MVPs.  After ten years, I was dropped from the program.  Since then I've tried to become a C# MVP, without any luck. I'm beginning to get bitter about it.  ;-)