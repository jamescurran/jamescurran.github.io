---
layout: post
title: The Evolutionary Guide to C# Lambda Syntax
categories: code c# .net programming dotnet csharp codeproject
tags: code c# .net programming dotnet csharp codeproject
---
Originally (.NET V1.1), we had to explicitly create a Delegate object to wrap a method reference to use it as a callback method, and that method had to named.

    button1.Click += new EventHandler(button1_Click);
    // :
    // :
    void button1_Click(object sender, EventArgs e)
    {
        DoStuff();
    }

With .NET v2.0, The C# compiler got smart enough to realize that when I used a method reference in code, I needed it wrapped up as a delegate, so it would silently to that for me.

    button1.Click += button1_Click;

Better still, C#2 added anonymous methods, which could be written inline.

    button1.Click += delegate (object s, EventArgs ea) { DoStuff();}

Then, C#3, we got lambdas, which were basically anonymous methods with a cleaned up syntax.

    button1.Click += (object s, EventArgs ea) => { DoStuff(); }
    
But, at the same time, the compiler got brighter about figuring things out for itself.  For example,  the Button Click event took a delegate to a method which had an object and an EventArgs parameter.  Giving it anything else is a compile-time error.  So, since we all agree that those are the parameters, why is it necessary for us to stand that out loud.  Why not just let the compile assume it.

    button1.Click += (s, ea) => { DoStuff(); };

From there, we have just a few more refinements, for special (but common) cases, but for these we can no longer use Button Click as the destination of our method reference, so from here on out, we’ll be use Enumerator.Where on an int array.  The important point here is that the lambda we will be writing takes an int, and return a bool.  Within that environment, our last syntax would look like this:

    int[] x = new int[] {1,2,3,4};
    var y = x.Where((x)=>{return x % 2 == 0;}).ToList();

But, if we have just one parameter, the compiler can figure out where it starts and ends, so we don’t need the parenthesis. 

    var y = x.Where( x => { return x % 2 == 0;}).ToList();

Finally, if all the function does is return a value (which is all a true lambda function is supposed to do), we can eliminate the curly braces and even the `return` :

    var y = x.Where( x =>  x % 2 == 0).ToList();

And that’s really all you need to know to write a lambda function.

<a href="http://dotnetshoutout.com/Honest-Illusion-The-Evolutionary-Guide-to-C-Lambda-Syntax">
  <img alt="Shout it" src="http://dotnetshoutout.com/image.axd?url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F03%2F17%2Fthe-evolutionary-guide-to-c-lambda-syntax.aspx" style="border:0px;" />
</a>