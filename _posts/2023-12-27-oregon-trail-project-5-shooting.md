---
layout: post
title: The Oregon Trail Project - Part 5 - Shooting
tags: code programming retro history opensource 
---

 - Part 0 : [Intro](https://honestillusion.com/blog/2023/11/07/oregon-trail-project-intro/)
 - Part 1 : [Markdown to Spectre Console ](https://honestillusion.com/blog/2023/11/26/oregon-trail-project-1-text/)
 - Part 2 : [Dates](https://honestillusion.com/blog/2023/11/27/oregon-trail-project-2-date/) 
 - Part 3 : [Data](https://honestillusion.com/blog/2023/11/29/oregon-trail-project-3-data/)
 - Part 4 : [Trail Events](https://honestillusion.com/blog/2023/11/30/oregon-trail-project-4-events/)
 - Part 5 : Shooting   (this one)
 
At various time in the course of the game, you'll have to shoot things, either to hunt for food, or to defend yourself against bandits or wild animals.  This is handled by asking you to type a word, and timing how long it takes you.  This is scaled by a rating you gave yourself.  At the start of the game, you are asked how good a shot you are;

    710  PRINT "HOW GOOD A SHOT ARE YOU WITH YOUR RIFLE?"
    720  PRINT "  (1) ACE MARKSMAN,  (2) GOOD SHOT,  (3) FAIR TO MIDDLIN'"
    730  PRINT "         (4) NEED MORE PRACTICE,  (5) SHAKY KNEES"
    740  PRINT "ENTER ONE OF THE ABOVE . THE BETTER YOU CLAIM YOU ARE, THE"
    750  PRINT "FASTER YOU'LL HAVE TO BE WITH YOUR GUN TO BE SUCCESSFUL."
    760  INPUT D9
     
Due to the way it is handled, the better a shot you claim to be, the worst shot you actually are.  And this rating it used nowhere else in the code, so there is zero advantage to claiming you are a good shot. But, regardless....

Now, the actual code handling shooting take advantage of a specific feature of HP Timeshare Basic.

    6180  S$="WHAM"
    6200  PRINT "TYPE ";S$
    6210  ENTER 255,B1,C$
    6240  B1=B1-(D9-1)
    6250  PRINT
    6255  IF B1>0 THEN 6260
    6257  B1=0
    6260  IF C$=S$ THEN 6280
    6270  B1=100
    6280  RETURN

Fortunately, I was able to track down a manual for the version of Basic online:

> TSB includes ENTER, a variation on the standard INPUT statement that continues after a time limit is reached.  ENTER has three inputs, a time limit in seconds, a return variable containing the actual time elapsed (or a status code), and then finally the user input. 
>  For instance, ENTER 15,T,A$[1,1] will wait 15 seconds for the user to type in a single character.  T will contain the actual time they took, -256 if the timer expired, or -257 or -258 to indicate problems with the terminal.

So, on line 6210, the `ENTER 255,B1, C$` means wait up to 255 seconds (4+ minutes) for the player to type a line, returning the line typed in `C$` and the length of time (presumably in seconds) in `B1`.

The timing in fairly simple in .NET.  But, we have to be concerned about making it device independent. Which give us this:

<script src="https://gist.github.com/jamescurran/b56bc09895d386b564ed8bdf3566c541.js"> </script>

And this bring us to the rather mysterious `IO` property in the `context` object, which will be the subject of our next update.
