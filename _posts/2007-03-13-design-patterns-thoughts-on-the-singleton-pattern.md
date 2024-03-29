---
layout: post
title: Design Patterns:Thoughts on the Singleton Pattern
tags: code c# .net programming
---
(Note: I started writing this a couple days ago --- a short while before Andrew Matthews published his own very similar <a href="http://aabs.wordpress.com/2007/03/08/singleton-%e2%80%93-the-most-overused-pattern/">article</a>.  He, of course, finished his first...)

(Update: Fixed the spelling/grammar, and then wrote a bit more)
 
 Lately on some of the blogs I read, I've been seeing  people giving code implementing various Design Patterns.  Much of it, I thought, wasn't particularly good, so I figured I'd inflected the blogosphere with my takes on some of them.

I'll start with everyone's favorite Pattern : The Singleton.

However, before I get to code, I figure I should give some principles for proper use:

 1. **You will, almost certainly, never actually need a singleton.**
Singletons are, by far, the most overused, misused, and generally abused Design Pattern. 

 2. **The Singleton pattern is used to model objects of which there *can be* only one (in the universe).  It is not intended for objects for which you *happen to need* only one in your program.**
 For example, some might want to create a singleton for "currently logged-in user", based on the idea that can only be one "currently logged-in user".  But no.  There are many users, one just happens to be logged in now.  That is not a place not a singleton.

If you have just one of an object, and you want to make sure that when you reference that object anywhere in your program, you refer specifically to that one object, you do *not* need a singleton.  What you want is a completely different programming technique known as a *Global Variable*.

"Wait a minute", you say.  "Aren't global variables *evil*"

Well, not exactly evil, but they are definitely frowned upon, and they will definitely cause problems if they are not handled properly.  However, the important point here is that regardless of how you refer to your global variable -- whether it's as "`g_MyVar`"  or "`pMasterPtr->pMyStructPtr->MyVar`" or "`CMySingleton.Instance().MyVar`" -- It's *still* a global variable and will still have all the problems associated with one.  If you are going to have to deal with all those problems anyway, you might as well choose the method which is both the most easily understood, most efficient, and requires the least typing.

*Side Note:* The second example of "renamed global variables" above came from an actual C program I was involved with some years ago.  pMasterPtr pointed to a (malloc'ed) struct, which was just a collection of pointers to other malloc'ed structs (one of each).  The elements of these structs had nothing to do with each, beyond the fact that they were all associated with a particular module.  The project leader proudly announced that the program had "only one global variable".  On program start-up, the "Structure Allocated Assignment Block" (i.e., SAAB, named after my car, over my objections) was malloc'ed, and then assigned to pMasterPtr (which I think was actually called pSAAB), then the other struct were malloc'ed and their pointers stored in the SAAB.  We could have just as easily made the elements of the secondary structs, elements of the primary struct, or just plain hard allocated data items, and it would have have no effect on the program --- the only difference is that this plan allowed the boss to say it had "only one global variable" --- which is, of course, nonsense -- it had dozens, they just had awkward names.  Additionally, due to a over-eager company coding policy, every access to a pointer had to be proceeded by a check if it were NULL, so, we'd have long blocks of code like this:

	if (pSAAB == NULL)
	{
		PrintErrorMessage();
		return;
	}
	if (pSAAB->pIOStruct == NULL)
	{
		PrintErrorMessage();
		return;
	}
	pSAAB->pIOStruct->nColor = NewColor;

When actually a simple line like this would have sufficed:

	g_nIOColor = NewColor;

The point of all the was to show to what great length some programmers would go to, to pretend that they don't use global variables.   And this leads directly to our obsession with, and abuse of, singletons.

Now, when you find something in your application of which there *Can Be Only One*, such as the Screen or the Mouse or the Keyboard, then you have a place for a singleton.  And in these cases, in .Net, I suggest using a static class.

"Wait a minute", you say.  "Aren't static classes as singletons *evil* ?"

Well, not exactly evil, but they are definitely frowned upon, mainly because they aren't inherently thread-safe.  However, no singleton technique is inherently thread-safe.  The methods of making a singleton and serializing access are completely separate.  The basic logic here is that if you are going to go to the trouble of allocating a singleton, it's easy to remember to also add the code to serialize access, but if you're going to take the easy way out on the singleton, you'll probably forget about synchronization also.  Not a particularly rational design strategy, but we haven't talked about a rational design policy yet in this article.

So, we've created the static class, and carefully implement thread-safe access, now do we have a good case for a singleton?  Still, no.  Because, as I pointed out earlier, a singleton is just a special kind of global variable.  Which means it has all the problems of a global variable. For instance, going back to another earlier example, the currently logged on user.  This appears to be a perfect candidate for a global variable if not actually a singleton.  And it was in that light the High-Priced Consultant Who Was Designing The System (on another project I worked on years ago --  C++ this time), decided that there would be "just a handful" of global variables in the app, one of which was "CurrentUser".  And I immediately protested -- I was assigned to write the administration module, where an admin user could log-in and *impersonate* another user.  Hence many (most) of the tasks in the app was to perform had to be done for *this specified user,*  who is not necessarily the (singular) CurrentUser.  Nevertheless, everyone else on the team used the global CurrentUser throughout the app, and I was forced to re-write much of it, to pass EffectiveUser as a parameter.

"Wait a minute", you say.  "Aren't you talking about a global variable, not a singleton?"  Yes, but the same rules apply.  Consider the examples I gave before of true singletons: the Screen, or the Mouse or the Keyboard.  Can you say for sure that you application will only ever have to deal with one of each of them?   Multiple monitors are already quite fashionable, and in this connected world, having a local user &amp; a remote user, sharing one screen but each with their own mouse &amp; keyboard isn't that unusual.
