---
layout: post
title: Some Better-Written Custom String Methods using C#
categories: code c# .net programming dotnet csharp
tags: code c# .net programming dotnet csharp
---

  In my daily web-surfing, I often stumble upon snippets of C# code posted by people.  Usually, I can tweak  it a bit. Sometimes, I can tweak it a lot.  I usually post a quick comment to the site offering it.  Today, I came upon some code that was so bad --- which the author said was from his forthcoming ***book!***--- more drastic measures must be taken.</P>

First we have a function to put a string into “Title Case” (which the author refers to as “Proper Case”) – Having the first letter of each word capitalized.  Here’s the original:

</P><PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String PCase(String strParam)
 {
     String strProper = strParam.Substring(0, 1).ToUpper();
     strParam = strParam.Substring(1).ToLower();
     String strPrev = <SPAN class="str">""</SPAN>;
     <SPAN class="kwrd">for</SPAN> (<SPAN class="kwrd">int</SPAN> iIndex = 0; iIndex &lt; strParam.Length; iIndex++)
     {         
<SPAN class="kwrd">         if</SPAN> (iIndex &gt; 1)
         {
             strPrev = strParam.Substring(iIndex - 1, 1);
         }
         <SPAN class="kwrd">if</SPAN> (strPrev.Equals(<SPAN class="str">" "</SPAN>) ||
         strPrev.Equals(<SPAN class="str">"\t"</SPAN>) ||
         strPrev.Equals(<SPAN class="str">"\n"</SPAN>) ||
         strPrev.Equals(<SPAN class="str">"."</SPAN>))
         {
             strProper += strParam.Substring(iIndex, 1).ToUpper();
         }
         <SPAN class="kwrd">else</SPAN>
         {
             strProper += strParam.Substring(iIndex, 1);
         }
     }
     <SPAN class="kwrd">return</SPAN> strProper;
 } </PRE>

What wrong here?  Lot’s of really bad string handling.  Remember, strings are 
immutable, so any action on one creates a new string.  So, “strParam.Substring(iIndex, 1)” creates a new string. “strParam.Substring(iIndex, 1).ToUpper()” create two new strings, and “strProper += strParam.Substring(iIndex, 1).ToUpper();” creates three new strings.  And, that’s within a loop.   And, since Substring is always used here to create a one-character string, it easier to just use a char --- except apparently, this book author doesn’t know how to.   Nor, doesn’t he apparently know about StringBuilder.  Then, we get to the algorithm itself, where he does such bizarre things as pointlessly treat the first character as a special case, in two different places. 

Ok, now let’s see the revision:

<PRE class="csharpcode">    <SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String PCase(String strParam)
{
         StringBuilder sb = <SPAN class="kwrd">new</SPAN> StringBuilder(strParam.Length);
         <SPAN class="kwrd">char</SPAN> cPrev = <SPAN class="str">'.'</SPAN>;  <SPAN class="rem">// start with something to force the next character to upper.</SPAN>
        <SPAN class="kwrd">foreach</SPAN>(<SPAN class="kwrd">char</SPAN> c <SPAN class="kwrd">in</SPAN> strParam)
         {
             <SPAN class="kwrd">if</SPAN> (cPrev == <SPAN class="str">'.'</SPAN> || Char.IsWhiteSpace(cPrev))
                 sb.Append(Char.ToUpper(c));
             <SPAN class="kwrd">else</SPAN>
                 sb.Append(Char.ToLower(c));
             cPrev = c;
         }
         <SPAN class="kwrd">return</SPAN> sb.ToString();
     }  </PRE>

First we start with a string builder, preallocated to the size of the string we are building.  The method doesn’t change the length of the string, so we know the length of the final string right from the start.

Next, since we are going to capitalize every letter after a period, and also the first letter, why not just pretend the mythical initial “last” character was a period?  Suddenly, the first letter is no longer a special case, and we still get what we want.

Then, we just loop through the letters, raising or lowering letter as we need. Note that it works on characters and not strings, and uses the build-in IsWhitespace method, instead of using a  hardcoded list of a subset of them.  A for() loop can in certain cases (however, not the one used in the original) be faster than a foreach(), but here I figured it was safe to sacrifice a tiny bit of speed for clearer code.

Next up, Reversing a String.  The Original:

<PRE class="csharpcode">    <SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String Reverse(String strParam)
     {
         <SPAN class="kwrd">if</SPAN> (strParam.Length == 1)
         {
             <SPAN class="kwrd">return</SPAN> strParam;
         }
         <SPAN class="kwrd">else</SPAN>
         {
             <SPAN class="kwrd">return</SPAN> Reverse(strParam.Substring(1)) + strParam.Substring(0, 1);
         }
     }</PRE>

Now, here the author might be able to earn a pass.  It’s possible that somewhere in his book he talks about recursive functions, and uses this as an example.  Then, if might be OK.  I mean, it is the only reason I can think of that someone might want a function which reverses a string – despite ubiquity of string reversing functions in libraries like this.  But, he presented it on his blog as a collection of string functions, so we’ll have to judge them on that basis.  

Again we have lots of string manipulation to accomplish something simple --- where there are already methods built into the framework to handle such things:

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String Reverse(String strParam)
{
     <SPAN class="kwrd">byte</SPAN>[] rev = Encoding.ASCII.GetBytes(strParam);
     Array.Reverse(rev);
     <SPAN class="kwrd">return</SPAN> Encoding.ASCII.GetString(rev);
}</PRE>
**UPDATE:** One commentator (quite rightly) noted that my Reverse() method would only work for strings of ASCII characters and will fail if there are any Unicode characters.   I knew that at the time, but I was hoping no one else would notice.  The problem was I needed a method which converted a string into an array of characters, and being unable to find one in the CLR, I substituted a string to byte array method instead.   I guess this is one of those times where you just have to step away for a while and come back to it, because now, I found the right method in a few seconds:

<SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String Reverse(String strParam)</P>
<DIV class="csharpcode"><PRE>{</PRE><PRE class="alt">     <SPAN class="kwrd">char</SPAN>[] rev = strParam.ToCharArray();</PRE><PRE>     Array.Reverse(rev);</PRE><PRE class="alt">     <SPAN class="kwrd">return</SPAN> <SPAN class="kwrd">new</SPAN> String(rev);</PRE><PRE>}</PRE>

And while my first version was a big improvement over the original, this is a big improvement over my first version, so you get double the benefits!

Next, we have a simple function to count the occurrences of a substring. 

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> <SPAN class="kwrd">int</SPAN> CharCount(String strSource, String strToCount)
{
    <SPAN class="kwrd">int</SPAN> iCount = 0;
    <SPAN class="kwrd">int</SPAN> iPos = strSource.IndexOf(strToCount);
    <SPAN class="kwrd">while</SPAN> (iPos != -1)
    {
        iCount++;
        strSource = strSource.Substring(iPos + 1);
        iPos = strSource.IndexOf(strToCount);
    }
    <SPAN class="kwrd">return</SPAN> iCount;}</PRE>

The revision isn’t much different but the subtle difference is important.  Instead of creating a new, shorter string to search, we tell it to just start looking after the last match.  Instead we now merely look at the string.</P>

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> <SPAN class="kwrd">int</SPAN> CharCount(String strSource, String strToCount)
{
    <SPAN class="kwrd">int</SPAN> iCount = 0;
    <SPAN class="kwrd">int</SPAN> iPos = strSource.IndexOf(strToCount);
    <SPAN class="kwrd">while</SPAN> (iPos != -1)
    {
        iCount++;
        iPos = strSource.IndexOf(strToCount, iPos+1);
    }
    <SPAN class="kwrd">return</SPAN> iCount;
}</PRE>

The next one has a special problem --- It doesn’t do what it claims to do!

<PRE class="csharpcode"><SPAN class="rem">// Trim the string to contain only a single whitepace between words</SPAN></PRE><PRE class="csharpcode"><SPAN class="rem"></SPAN><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String ToSingleSpace(String strParam)
{
    <SPAN class="kwrd">int</SPAN> iPosition = strParam.IndexOf(<SPAN class="str">" "</SPAN>);
    <SPAN class="kwrd">if</SPAN> (iPosition == -1)
    {
        <SPAN class="kwrd">return</SPAN> strParam;
    }
    <SPAN class="kwrd">else</SPAN>
    {
        <SPAN class="kwrd">return</SPAN> ToSingleSpace(strParam.Substring(0, iPosition) +        strParam.Substring(iPosition + 1));
    }
}</PRE>

Now, it says that it should remove repeated space, so that there is only one space between words. However, what it actually does it to remove all spaces.  This gives us a problem: Should my rewritten function do what it claims to do, or what it actually does?  I decided to give you one of each.

Duplicating the result is quite straightforward:

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String RemoveAllSpaces(String strParam)
{
    <SPAN class="kwrd">return</SPAN> strParam.Replace(<SPAN class="str">" "</SPAN>, <SPAN class="str">""</SPAN>);
}</PRE>

Writing a function to do what it is supposed to is a little more involved, but still simple:

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> String ToSingleSpace(String strParam)
{
    var sb = <SPAN class="kwrd">new</SPAN> StringBuilder(strParam.Length);
    var prevWS =<SPAN class="kwrd">true</SPAN>;
    <SPAN class="kwrd">foreach</SPAN>(var c <SPAN class="kwrd">in</SPAN> strParam)
    {
        <SPAN class="kwrd">if</SPAN> (Char.IsWhiteSpace(c))
        {
            <SPAN class="kwrd">if</SPAN> (!prevWS)
                sb.Append(<SPAN class="str">' '</SPAN>);
            prevWS = <SPAN class="kwrd">true</SPAN>;
        }
        <SPAN class="kwrd">else</SPAN>
        {
            sb.Append(c);
            prevWS = <SPAN class="kwrd">false</SPAN>;
        }
    }
    <SPAN class="kwrd">return</SPAN> sb.ToString();
}</PRE>

We create a string builder the size of our source string, which would be the maximum size our trimmed string could be, and we set prevWS to true.  This way, it will remove all leading whitespace.  Then we just step through the string, character by character, appending that character to our new string if it’s not a whitespace character, and appending a space for the first whitespace character found.  Note that this reduces all forms of whitespace (tabs, newlines, spaces etc) to a single space.  The original just worked on spaces. 

Finally, we have a function to determine is a string is a palindrome. 

<PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> <SPAN class="kwrd">bool</SPAN> IsPalindrome(String strParam)
{
    <SPAN class="kwrd">int</SPAN> iLength, iHalfLen;
    iLength = strParam.Length - 1;
    iHalfLen = iLength / 2;
    <SPAN class="kwrd">for</SPAN> (<SPAN class="kwrd">int</SPAN> iIndex = 0; iIndex &lt;= iHalfLen; iIndex++)
    {
        <SPAN class="kwrd">if</SPAN> (strParam.Substring(iIndex, 1) != strParam.Substring(iLength - iIndex, 1))
        {
            <SPAN class="kwrd">return</SPAN> <SPAN class="kwrd">false</SPAN>;
        }
    }
    <SPAN class="kwrd">return</SPAN> <SPAN class="kwrd">true</SPAN>;
}</PRE>
<P>Here the change is subtle (ignore the length calculations at the start, which are a trivial micro-optimization).</P><PRE class="csharpcode"><SPAN class="kwrd">public</SPAN> <SPAN class="kwrd">static</SPAN> <SPAN class="kwrd">bool</SPAN> IsPalindrome(String strParam)
{
    var iLength = strParam.Length;
    var iHalfLen = iLength / 2;
    iLength --;
    <SPAN class="kwrd">for</SPAN> (<SPAN class="kwrd">int</SPAN> iIndex = 0; iIndex &lt; iHalfLen; iIndex++)
    {
        <SPAN class="kwrd">if</SPAN> (strParam[iIndex] != strParam[iLength - iIndex])
        {
            <SPAN class="kwrd">return</SPAN> <SPAN class="kwrd">false</SPAN>;
        }
    }
    <SPAN class="kwrd">return</SPAN> <SPAN class="kwrd">true</SPAN>;
}</PRE>

In this function, instead of comparing one-character long strings, I compare individual characters.  That change, by itself, cause a 4X speed improvement.

So, there you have it.  The article had more, but the other were trivial, and I couldn’t make them any better.  And in case you think I’m all talk here, each of those rewrites was benchmarked to run 2 to 5 times faster than the original.