---
layout: post
title: One thing after another....
tags: site my-life
---

  
The problem started small.

I was trying to install a headset on my PC.  I'd got a switch, to toggle between the speakers and the headphones.  Things were going fine, when suddenly, my keyboard stopped working.   I've got 5 PCs using the same keyboard & monitor using a KVM switch, so I switched to a different box, and the keyboard worked fine there.  So, I switched back -- still nothing on my primary PC.

OK, I still not concerned.  I tried rebooting.  No good.

I did a power cycle.  Still nothing.  I'm getting a little concerned.  I opened up the case, to check that the cable is tight.  

When I opened it up, I noticed that the CPU heat sink fan was packed was dust.  It was still spinning fine, but I figured I clean it out while I had the case open.  Here's where things start to go south.

I released the straps that held the heat sink to the motherboard, and lifted it up -- and discovered that the CPU was glued to the heat sink, so I had just yanked the CPU out of the motherboard.

Now to install a CPU chip, you place it in a socket, then throw a level, which locks it in.  Then you put the heat sink on top.  The heat sink, by the way, in huge -- about a third the size of a shoebox --- compared to the CPU chip which about the size of a credit card.  Now, with the heat sink attached to the CPU, it impossible to throw the level to lock the socket --- the lever cannot be reached, and even if it could, it cannot be moved, with the heat sink in the way.   Not to mention, as I yanked it off, I bend some of the pins.

So, I figured it time to call in a trained professional.  I found a computer repair place in town, and stopped by after work.  I explained the problem to the owner who simply pried the chip off the heat sink with a screw-driver, and straightened the pins with  a hypodermic needle.  He re-seated the CPU and powered it up.  It booted.... sort-of.

It would start to load Windows, and then lock up. It did this repeatedly, always at the same spot.  Figuring an operating system file was corrupted in the crash, he gave it back to me to re-install the OS.  This puzzled me, as the system didn't actually crash -- the main trouble happened while it was powered down, and by all accounts, the hard disk was OK.

Nevertheless, I took it back and tried to reinstall XP.  I tried three times, and the system would always lock up, just as it started the second phase of the setup.  I concluded that I had damaged the part of the CPU that handles [Protected Mode](http://en.wikipedia.org/wiki/Protected_mode) while leaving the circuits that handle [Real Mode](http://en.wikipedia.org/wiki/Real_mode) intact.  So, it would boot OK in real mode, and then lock up when Windows tried to switch to protected mode.

So, back to the repair shop, with the instruction to replace the CPU (if he had a AMD64 available) or the entire motherboard with a different CPU (if he didn't)  (CPUs are about $300-$500 with that particular one on the high end, while the motherboard is about $50, so replacing the CPU only would probably be to his advantage).

However, not trusting my diagnostic abilities, he spend much of a day trying to figure out what was wrong himself.  What he found was that the CPU itself was OK, and the problem was in the motherboard.  This saved me a couple hundred dollars which is a good thing, but.....

But... Apparently out of habit (because the problem is *always* the hard disk), he reformatted the hard disk & re-installed the OS. Recall, there was nothing wrong with the hard disk --- except that they weren't being backed up properly ( * ).  

So, in the end, I lost a year's worth of documents (mostly photos and financial records in MSMoney)...

( * ) Improperly backup hard disk:  The entire hard disk was being backed up to a [Network attached storage](http://en.wikipedia.org/wiki/Network-attached_storage) system --- except the process that run the backup didn't have access to my "My Documents" folder (which is naturally the most important one). This has been corrected.
