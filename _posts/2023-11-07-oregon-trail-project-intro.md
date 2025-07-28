---
layout: post
title: The Oregon Trail Project - Part 0 - Intro
tags: code programming retro history
---
 You might notice that I don't write much on this blog.  My last entry was the written last Decemeber for the [2022 C# Advent Calendar](https://www.csadvent.christmas/), and I've already signed up for the 2023 version, and I had a idea for that.

> Side note:  While not writing to this blog, I missed the 20th anniversary of my starting it in [August of 2003](https://honestillusion.com/blog/2003/08/01/first-things-first/)

But then, [Virtual Coffee](https://virtualcoffee.io/) announced that this month's Challenge, inspired by the  [NaNoWriMo (National Novel Writing Month) Challenge](https://nanowrimo.org/), would be a Blogging Challenge, where the members, collectively, try to write 100,000 words worth of blog articles.

> Side note II:  I'm writing this in VS Code, which has some ChatGPT based extension loaded.  I paused for a second, and it provided me with this bit of text:
>> The Oregon Trial Project is a collection of Python scripts that I've written to help me understand the trial process in the state of Oregon.

> This is not even close to what this is going to be about (But then, it is based on my original misspelling of "Trail" in the title.)

I came up with one idea for that (which I think I'll write up after this -- but post it first --  just to keep you guessing), and was just going to leave it at that, when I recalled this project, which I had been thinking about for a few months, but hadn't spend any actual time working on it.  It seemed like it could make a nice serial, which would rack up the word count.


## The Oregon Trail : Background

The Oregon trail, of course, was an actual historical route real American settlers used in the 1800s, but no one cares about that.  The important one was a computer game used in schools in the 1970s & '80s, to teach history (or something like that).  It was apparently written in 1971, but didn't go nationwide until 1978, when grammar schools were being given Apple II computers, and Oregon Trail was pretty much the only thing they had that ran on them.

Or at least, that's what I've been told.  By 1978, I was already in high school, and I missed all that.  I didn't hear about it until the 2000s, when I was quite surprised to learn it was some sort of cultural phenomenon.  And it mostly involved dying of dysentery.

So, I was a bit delighted to find copy of the original BASIC source code on the Interwebs a few months ago.  (OK, not the "original-original" -- [The one I found claims it was from 1978](https://archive.org/details/200106-tops10-in-a-box) (thanks to Jimmy Maher for posting it)). 

## The Project

The bad news is that I don't have access to a computer running a HP Timed-Shared BASIC from the 1970s.  The good news is that I learned BASIC back in the '70s, and it's simple enough to understand.  So, I can read this source, but can't run it.

So, the plan is to port it into a language/environment that I do have access to, specifically C#.

Now, I've been involved in a project very much like this before.  Back in 2008, (*could it really be 15 years already?*) I was part of a contest on CodeProject to port a Basic version of a **Star Trek** game.  Usually, when someone attempts something like this, they usually just work on getting a modern compiler to accept old code, by tricking it into forgetting 40 years of software development techniques.

You see, back then Basic didn't have a concept of functions/methods, let alone `objects`.  (Neither did Fortran or COBOL or most other languages at the time)  Basic had a keyword `GOSUB` for calling "subroutines" but they had only the barest resemblance to what we now call a function.  Particular, there was no way to pass arguments to a subroutine, nor a way of return a value. The work-around for this that we all accepted was using global variable to handle those tasks.  Further, most Basic interpreters  only allowed one statement per line. These limitation, plus a few others, led to a code-style known as "spaghetti code".

So, when people wanted to port a basic program to modern language, they just made a single class with a single method with hundreds of local variables, and overuse the `goto` command which most languages still have (but don't talk about).  An example of this can be seen in one of the other entries in the **Star Trek** contest ([Star Trek 1971 Text Game](https://www.codeproject.com/Articles/28228/Star-Trek-1971-Text-Game)). 

This always impressed me as a big waste of time. If I was going to port a program to a new language, I was going to take advances of all that language had to offer.  For the **Start Trek** contest, I completely rewrote the code, giving it a modern Object-oriented design --- ***but,*** retaining the exact same look and game play. You can see that here: [The Object-Oriented Text Star Trek Game in C++](https://www.codeproject.com/Articles/28399/The-Object-Oriented-Text-Star-Trek-Game-in-C).

That's my goal for this project:  Oregon Trail, exactly as people remember it, but a completely new (and *better*) design under the covers.  Further, I want to separate the actual processing in its own assembly, apart from the UI, so that it can be uses to port the game different clients.  (There may be a Blazer version in the future.)

As the series progresses, I'll be taking you through this process...

