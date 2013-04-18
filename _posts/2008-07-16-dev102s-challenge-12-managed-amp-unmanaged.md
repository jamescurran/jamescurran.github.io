---
layout: post
title: Dev102's Challenge #12 : Managed &amp; unmanaged
categories: 
tags: 
---

  <p>My solution(s) for last week's challenge were cited, but, only as an "honorable mention" / "also run".</p>  <p><a href="http://www.dev102.com/2008/07/14/a-programming-job-interview-challenge-12-managed-and-unmanaged/">This week's challenge</a> is a different sort of animal.  Not that it is particularly difficult --- actually I suspect it's quite easy --- it's just that it requires a fairly specialize knowledge (Managed Extensions for C++ in this case).  A couple of them in the past required some basic knowledge of .Net &amp; the CLR, but most of the time, the challenge involve a non-platform specific algorithm.</p>  <p>So, knowing nothing about Managed Extension, I was just going to let this one pass.  But, I happened to run into an old friend (Will Depalo) from my days as a VC++ MVP.  When I went to .Net, I also switched to C#, but he stayed with C++, so I figured he would have some insight.  Reducing the problem to one sentence ("we have a unmanaged class accessing an instance variable of a managed class"), and he immediately  knew the answer ("you'll need to pin it"), and offered some advice ("look up '__pin' in the msdn").  However, didn't realize exactly how good that advice was, as the folk's at DEV102 apparently took the example source code from the __pin article to create the challenge.  So, here's my answer (I love stuff that can be answer via copy'n'paste):</p>  <p> </p>  <pre class="c#">int main() 
{
   ManagedClass <strong>__pin</strong> * pMngdClass = new ManagedClass;
   UnmanagedClass* pUnmngd = new UnmanagedClass;
   pUnmngd-&gt;incr(&amp;(pMngdClass-&gt;i));
}</pre>

<p>The rest is unchanged.</p>