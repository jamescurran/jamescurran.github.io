---
layout: post
title: Running An Async Event Handler on the UI thread (with lambdas and extension methods!)
categories: code c# .net programming dotnet csharp codeproject
tags: code c# .net programming dotnet csharp codeproject
---

  
So, it's been a freakishly long time since my last post here. 

I've been trying to get better... I even wrote out a list of topics I wanted to write about.  So, let's start talking about them.

As we do more and more work in our applications asynchronously, we're confronted with the problem of how to communicate the status of the background thread to our users.  Firing an event seems like the best way, except anything the user can see has to be done by the UI thread, which is specifically not the thread the background task is running on.

Winforms offers a means of redirecting execution onto the UI thread, the Form.Invoke method, but using it makes life difficult.   This is particular true when you throw lambdas into the mix.  Lambdas made it quite easy to write a simple event handler:

     backgroundAction.StepCompleted +=
         (s, ea) => {statusMsg.Text = String.Format("Step #{0} Completed", ea.StepNum);}


But when we add is thread marshaling, it gets messy:

     backgroundAction.StepCompleted +=
      (s, ea) =>{this.Invoke((s1,ea1)=>{statusMsg.Text = String.Format("Step #{0} Completed", ea.StepNum);}, s,ea);


And that's with always marshaling it.  If the event could be fired from both a background thread or from the main thread depending on the context, then you should check the InvokeRequired property, if call the action directly if it's not needed.  But it  complicates our lives, making us split the action out into a named function:

<script src="https://gist.github.com/jamescurran/5452468.js">   </script>


Yech!  We have a lambda, but it we're using it for the boiler-plate code and that we will probably have to repeat.  And the real method we want to perform is in a separate function.

What we need is a handle utility function which will take a method reference, or, better yet, a lambda, and package it up as we need it. Then we could write it like: 

    backgroundTask.StepCompleted += 
       ToUIThread<StepEventArgs>((s, ea) => statusMsg.Text = String.Format("Step #{0} Completed", ea.StepNo));

The tricky part about this is that it must take a function as a parameter, and *return* a function. Plus, the compiler can't figure out the type of the second parameter by itself, so we have to give it some help.  And, we we'll need a non-generic version, for events which are defined as `EventHandle` instead of `EventHandler<TEventArgs>.`   Make it an extension method on Form, and we've got:

<script src="https://gist.github.com/jamescurran/5452498.js">    </script>

<a href="http://dotnetshoutout.com/Honest-Illusion-Running-An-Async-Event-Handler-on-the-UI-thread-with-lambdas-and-extension-methods">
  <img alt="Shout it" src="http://dotnetshoutout.com/image.axd url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F03%2F15%2Frunning-an-async-event-handler-on-the-ui-thread-with-lambdas-and-extension-methods.aspx" style="border:0px;" />
</a>