---
layout: post
title: Oh My God! It's back!
tags: 
---

Yes, after about 5 months of downtime, I finally was able to restore my blog.
 
What happened was, sometime mid-July, the PC that was running Sql Server on my network died.  It was acting funny for a few weeks, so I took it off line.  I removed the hard disk from it and attempted to install it on a different machine.  It worked OK as a second drive, but I couldn't boot from it.  At this point, I should have just left it as a second drive, but I didn't....

I tried re-installing Windows on that drive, and as I was doing this, the second PC failed.  That PC was my Internet gateway, so it was imperative that I get that one back online.  I shuffled disks around a bit, and got it working again.

Map of PCs:

 - pcA - Internet Gateway  (old Compaq, circa 2001)
 - pcB - Sql Server 2000  (identical old Compaq)
 - pcC - Sql Server 2005 (more recent generic PC, circa 2004)
 - pcD - Web server (Micron PC, circa 2002)
 - pcE - Work machine (generic PC, circa 2006)
 - pcF - HomeServer Beta 2 (generic PC, circa 2007)

Now, pcB had been decommissioned, but was still was on the table with the others. pcC died, which is why the blog went down.   I tried moving the disk to pcA, which then proceeded to die itself.  I move the system disk from pcA to pcB to regain access to the Internet.  (Complication -- they had different NIC cards).  I then had to install the other apps that were running on pcC (notably my mail server). In midst of all this disk swapping, twice I forgot to power down the PC before pulling &amp; reattaching a ribbon cable to a disk -- which killed the disks, one of which was the system disk to pcC.

So, all I needed to do now was to restore the back up of pcC from HomeServer onto a new hard disk.  pcA seemed to recover and I bought a new HD at a computer show, so, mid-August, you would think I was good to go.  No such luck.  HomeServer refused to restore the disk.  Which was crazy, as I had already restore that disk a few weeks earlier -- You'll recall that I said that pcC had been flaky for a while; one of those acts of flakiness was crashing it's hard disk.

I bought a new PC, which we'll call pcG (Woot.com had a close-out for $250).  HomeServer refused to restore to that machine either.   Finally, I had an idea: I put a disk back into pcC, and boot up HomeServer's restore disk on that.  This time it would restore the disk.  I then quickly copied the database files off it. (The website is now being run off SqlServer Express on pcD, which means beside reducing the number of mission-critical PCs from two to one, the setup is now finally LEGAL).

In the coming days, I plan on installing HomeServer RTM on pcF, and decommission my pcG (or at least salvage the most of the hard disks from it, for the new HomeServer).  I'm not sure what pcG's eventual use will be, but I'm happy just to be back to square one.