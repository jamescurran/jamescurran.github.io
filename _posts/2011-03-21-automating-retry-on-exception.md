---
layout: post
title: Automating Retry on Exception
tags: code c# .net programming dotnet csharp codeproject
---
Every so often, you run across some action,  which just fails, where the best response it to just try it again.  This is particularly true when dealing with an external source, like a database or web service, which can have network or other temporary problems, which would have cleared up when you repeat the call seconds later.   Often, these actions fail by throwing an exception which makes the process of trying the call again rather cumbersome.  Having to deal with this a number of times in one application, I decided to wrap the full procedure up in a method, which I share here with you:

OK, this has very weird semantics.  A call will look something like this:

    var msg = Util.Retry(5, 1,
         ( ) =>"Error,
          AppLog.Log, // assuming there is an AppLog.Log(Exception ex) method.
          ( ) =>
              {
                  // Simulation of a function which fails & succeeds at random times
                  Console.WriteLine(DateTime.Now.ToLongTimeString());

                  if((DateTime.Now.Second & 7) != 7)
                      throw new Exception(("Bad");
                  return DateTime.Now.ToString();
              });




 * The first parameter is the maximum num of time to retry the call before giving up. 

 * The second parameter is the time, in seconds, to wait between retries.

 * The third parameter is a function (or lambda expression) taking no parameters, and returning the value to return in case of failure. It done as a function to avoid evaluating it unless needed, in case it was a heavy-weight object, or had some side-effect.

 * The  fourth parameter (made optional by way of an overload) is a function (or lambda expression) taking an exception as a parameter, and returning nothing.  Can be used to log the final exception when it gives up retrying.
 * 
The last parameter is a function (or lambda expression) taking no parameters, which performs the actual work which may need to be retried. 
  

The code looks like this:

    static public T Retry<T>(int retries, int secsDelay, Func<T> errorReturn,
                              Action<Exception> onError,  Func<T> code)
    {
        Exception ex = null;
        do
        {
             try
             {
                  return code();
             }
             catch(Exception dde)
             {
				ex = dde;
               retries--;
               Thread.Sleep(secsDelay * 1000);
			}

      } while(retries > 0);

  	   onError(ex);

       return errorReturn();

    }

The full source code (with comments and everything!) is  [here](http://honestillusion.com/files/folders/c-sharp/entry8103.aspx)
