---
layout: post
title: My First CodePlex Project:SnitzMember
tags: 
---
After many months to checking out the new projects on CodePlex almost daily, it's finally time for me to create one. 

As you might know, I also run the website, NJTheater.com.  Written about 10 years ago, all in classic ASP, it has a forum section -- which uses the open source Snitz Forum package.

Over the years, the membership has grown (over 3000 registered users), and I started to use the forum membership database, to manage authorization in other others of the website.

Now ASP.Net v2 came out, with it's built in Authentication/Authorization system, I thought "That's exactly what I need for my website" --- except it used a completely different db schema then the one I already had (and each hashed the passwords in different ways, so no conversion from one to the other was possible).

Fortunately, the membership system was built on using the Provider model, so I just had to create a Membership provider which fitted the interface, but used the existing membership database. [SnitzMember](http://www.codeplex.com/SnitzMember) is the provider.

Also, you may have noticed that I referred to this as my *first* CodePlex project.  Well, you may have also noticed that I referred to NJTheater.com as 10 years old, and written in Classic ASP.  Stay Tuned.
