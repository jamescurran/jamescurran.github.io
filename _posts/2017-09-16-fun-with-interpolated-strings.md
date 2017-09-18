---
layout: post
title: Fun With Interpolated Strings
tags: code programming csharp
---


Hard to believe it's been over two years since my last blog post.   

I've been busy...

OK, I've just been lazy.

Actually, I've just been at a series of contract assignments which didn't lead to any techniques worth writing about.

However, at my new job, they let me play around with the code, so now I've got something to talk about.  I've got a big article I'm half way through writing, which will be up in a couple days, but for now, how 'bout a quickie I just discovered yesterday.

Actually, two things, both dealing with interpolated strings.

First, you can combine interpolated strings with literal strings:

    var fname = "fullname.txt";
    var pathname = $@"C:\Temp\{fname}";
    
Next, inside the brace, you can put real code -- but quotes there are not escaped.

    var fname = "fullname.txt";
    var pathname = $@"C:\Temp\{fname.Replace(".txt", ".out")}";
    
