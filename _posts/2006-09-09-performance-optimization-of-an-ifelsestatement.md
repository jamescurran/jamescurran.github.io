---
layout: post
title: Performance optimization of an if/else-statement
categories: code c# .net programming
tags: code c# .net programming
---
[Mads Kristensen](http://madskristensen.net/post/Performance-optimization-of-an-ifelse-statement.aspx) wrote on the subject on if/else statements in C#, running time benchmarks on code such as this:

```
		private bool RunIf(string input)
		{
			if (input == "hello")
				return true;
			else if (input == "jelly")
				return true;
			else
				return true;
		}
```

