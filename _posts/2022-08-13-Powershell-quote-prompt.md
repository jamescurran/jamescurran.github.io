---
layout: post
title: Putting quotes in your Powershell prompt.
tags: code programming powershell  dotnet
---

I just discovered my friend Meg Gutshall has a blog, and was reading her latest entry (from last year -- she updates hers about as often as I update this one): [A Random Quote Generator for Your Terminal](https://meghangutshall.com/2021/07/08/random-quotes-generator/).  

It display a quote as part of the prompt in a BASH shell.  I thought this was a cool idea, but I don't use BASH -- I use PowerShell, so I figured I port hers to PS. I'd also found this project on Github ([Literature Clock](https://github.com/JohannesNE/literature-clock)) which collects quotes based on the time (e.g., "At 7:32, he suffered a fatal stroke," *IT*, Stephen King), and I thought it would be cool to use those.

But, I'm getting a bit ahead. Let's first talk of changing the PowerShell prompt.  The prompt is producted by a script which is held in a file named `Microsoft.PowerShell_profile.ps1`, which, on my system is located in the `WindowsPowerShell\` subfolder of my `Documents` folder.  I'm not sure how standard that is.

The default prompt is the current folder name followed by a ">".  This is fine if you have short folder names, but no one has short path names anymore.  So now it looks something like:

    PS C:\Users\James\Source\repos\Tests\MyBlazorApp1>

And you've already used half the line before you typed anything.  The simplest command wraps to the next line.  That's too messy for me.  So, I first put the current directory on it's own line, and then threw in the current date & time for good measure:

    C:\Users\James\Source\repos\Tests\MyBlazorApp1
    12-Aug-2022 18:52:29
    PS>

(The path is in magenta and the date in green, but I don't seem to be able to color text in markdown.)

<script src="https://gist.github.com/jamescurran/43824e609e8f2858be5665ad4d02f95b.js"> </script>

Ok, now we move to adding the quotes. The Literature Clock project has a large collection of time-based quotes from book, organized by minute.  They have at least one quote from 950 of the 1440 minutes in a day. Some are duplicated if it's unclear if the time refers to AM or PM.  And while many minutes have no quote, others (like midnight or 3 o'clock) have dozens. The quotes are kept in a CSV (actually pipe-separated) file.  The project includes a javascript program to display the clock on a webpage, and a script written in the R language to convert the CSV file into a collection of JSON files.  It's the json files that the javascript and this project will use.

I've also ported the clock webpage into Blazor (as well as port the R program into C#) so that might be a future blog story.

For each minutes, there's a json file with a name in the form "HH_MM.json", which holds an array of objects which look like this:

<script src="https://gist.github.com/jamescurran/11d9dea47ef0741128fcc0959afb2ce1.js"> </script>

So, our steps will be: Get the time, format it like the filename, read the file, deserialize it into an array of objects, pick one randomly, and display it.  Except for the lst step, the PowerShell is just as straightforward as the description:

    $quote = get-content "C:\Users\zames\OneDrive\Documents\times\$(Get-Date -Format "HH_mm").json"  | ConvertFrom-Json  | Get-random

The rest just is formatting and coloring it to look nice.  Full script here:

<script src="https://gist.github.com/jamescurran/3110a8dfbb1ba85a6e5f6de41c7edbdd.js"> </script>


![PowerShellPrompt](/images/PowerShellPrompt.png)


