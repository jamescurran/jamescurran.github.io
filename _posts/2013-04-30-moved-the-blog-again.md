---
layout: post
title: Moved the blog again - GitHub Pages
tags: website my-life
---
"*What*?" you say.  "Another blog post *already*? --- It's only been 11 months since the last one....."

Yeah, Yeah, Yeah.... I've been busy.  In addition to the exciting career in software development, I've been studying Massage Therapy -- I'll be a Licensed Massage Therapist is a couple months.   And with the weekends often taken up with photography, there's not much time left for blogging.

I've also switched my ISP, and no longer have a fixed IP address, which meant I could no longer host the site from my home computer.  Just as well, I had a lot of stability problems doing that.

However, just about that time, I learned about hosting blogs on GitHub, so I thought I've give it a try.  I'm still working on how to import the comments from the old site (although over nearly ten years, there are only about a dozen comments worth carrying over)

Of the things I've learned:

 - If there's a problem building the site, GitHub merely says : '**page build failed.**'  with no indication what the problem was or even on what page the problem occurred.
 - This is because they expect you to be running the site builder program, (Jekyll -- so named because it's designers consider it a monster, which itself is built on the Liquid templating language)
 - Jekyll & Liquid are both Ruby programs, one of which (never figured out which one), requires not only the Ruby runtime, but also the Ruby Development Kit.
 - Neither Ruby nor the DevKit can deal with paths with spaces, so don't try putting it in the "C:\Program Files" folder with all your other executable.
 - Both the Ruby runtime & the DevKit need to be added to the PATH.  (But for the runtime, it's the \bin folder, while for the DevKit it's the main folder)
 - Jekyll spits most of it's errors to stderr, but some to stdout, It also spits out hundreds of lines so it best to redirect both streams to files.
 - When GitHub builds the site, they do not run any plug-ins, so if you found any plug-in based add on for GitHub Pages, it's assumed (but never mentioned) that you'll be running Jekyll offline, and then uploading the generated pages.
 - pygments, one of the plug-ins that GitHub does run, for formatting code snippets, isn't worth the effort.  Use MarkDown's indented-line-are-code formatting for very short samples, and GitHub's Gists for longer ones.
 - Posts are written in MarkDown -- you actually get to choose between several different Markdown processor (they offer no help on the differences), with a "yaml" header on top.
 - YAML - is just a simple list of `key: value` lines offset on top & bottom by three dashes.
 - `title` and `layout` are the important keys the YAML header.  You can also include `categories`  but there's not much you can do with it without a 3rd-party plug-in and external processing.
 - Because the YAML syntax uses a colon, you **cannot** have a colon in the value part of the line.  (e.g., you cannot have a colon in your posts title)
 - 
 


