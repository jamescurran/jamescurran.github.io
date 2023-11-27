---
layout: post
title: The Oregon Trail Project - Part 1 - Text (Markdig.SpectreConsole)
tags: code programming retro history opensource
---

Oregon Trail Project
--------------------

- Part 0 : [Intro](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)
- Part 1 : Markdown to Spectre Console (this one)

Continuing my attempt to port the 1978 BASIC code for the Oregon Trail game to C#, the first step is *quite simple*. We need a way to display text.  

The original game just had some basic text.  Mostly just a sentence or less at a time.  The largest block was the instructions and it's not even 300 words. But I wanted to added some formatting to it -- in a way that could be adapted to whatever output media this was eventually going to be used.  That was going to be console text in this first version, but HTML or WPF versions might be in the future.  I figured the best way would be to use [MarkDown.](https://en.wikipedia.org/wiki/Markdown).

## And Then I went down a rabbit hole ##

I started looking for a markdown component to output to the console. The problem here is that Markdown was intended to translate to Html, so a parser to do that is easy to find. It was also intended for formatting which just can't be done on a text console.  So, no one tried.

I did, however, discover [SpectreConsole](https://github.com/spectreconsole/spectre.console), which makes it easier to create beautiful console output.

So, I could get a really nice output, if I was willing to give up the possibility of non-console output -- or I could keep looking for a markdown solution.

But then I discovered [MarkDig](https://github.com/xoofx/markdig), which is a parser for markdown, which has a number of renderers for output media (Html, Wpf, Xml), but more importantly, it's essentially a toolkit for building a renderer for other media.

So, I just used [Kryptos-FR's Wpf renderer for Markdig](https://github.com/Kryptos-FR/markdig.wpf) as a model -- really, I stole *a lot* from that project -- and created a renderer for SpectreConsole.  

For the most part, the translation was straightforward and simple.  For example, a simple bit of markdown like `This is *emphasized*` becomes `This is <em>emphasized</em>` in Html (or, if you prefer, `This is <span style="font-style:italic">emphasized</style>`), in Spectre, in needs to be `This is [italic]emphasized[/]`.

The tricky part was ordered and unordered lists.  In Html (and Markdown), you just say "This is a list, and these are the items", and the browser takes care of the details of indenting and bullets and sequential numbering and all that -- all of which I had to handle directly in the renderer. Which led to another problem.  The parser treats all the text of an item as a block, or, more specifically, as a paragraph. And paragraphs start and end with a newline.  Now, browsers know to strip this whitespace off. But consoles are much more literal when it comes to spaces and new lines. And these line breaks were messing out the flow of the list:

        1.

    This is the text for bullet #1

        2.

    This is the text for bullet #2

It took me a while to figure out that the solution wasn't in the list handling code, but involved changing the paragraph handler.  Once I came to that realization, the fix was easy, which just shows how robust the MarkDig parser is.

And those pretty much handled everything I needed for Oregon Trail.

## And the rabbit hole got deeper ##

While that was enough, I wasn't quite satisfied. I really should to something for headers, but we can't change the font size in text mode, but SpectreConsole did have a feature called `Figlet` which could be used.  And both MarkDown and Spectre had Tables, and Code Samples.

But, the problem here is that those features didn't use the markup style the other features did. Using the advanced features entails creating a object and rendering it, which didn't really play well with the markup style.  After playing around for much too long, I decided to give up of those, and come back to them later.

The code for this is available at: https://github.com/jamescurran/Markdig.SpectreConsole
