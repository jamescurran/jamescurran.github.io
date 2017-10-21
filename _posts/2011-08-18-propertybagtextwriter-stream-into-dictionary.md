---
layout: post
title: PropertyBagTextWriter (Stream into Dictionary)
tags: code c# .net programming dotnet csharp castle monorail codeproject
---

It's been too long since I posted some .NET code, and I've been itching to.  (Actually, I really want to write more about politics, but I figured if I don't show some code soon, I'm gonna lost my techy audience)  Fortunately, I've got a backlog of things I've been meaning to write about.  Today's is the `PropertyBagTextWriter`.
  
The original purpose of this is for a particular use in combination with Castle Monorail and Linq2SQL, but it has been made general purpose, so you may find a use for it in other environments. 
  
Now, when Linq2SQL was still in beta, the `DataContext` object had a property which held, as a string, the SQL generated from the Linq query.  As I was writing a Monorail website, I often assigned that property to a value in the PorpertyBag (which is just a IDictionary, not even a IDictionary&lt;K,V&gt;  -- `PropertyBag["SQL"] = db.Log;`), and write it in  an HTML comment on the webpage, so I could see if I was getting what I expected.  However, the designers eventually realized that a string property wasn't good enough, as the Linq query could produce several SQL statement, some of which would be based on the response from the earlier ones.  So, they replaced it with a property which can be set to a `TextWriter` and have the SQL output written there.  So, to use it the way I was before, I needed a `TextWriter`-ish object, which would set it's output to a value in a Dictionary  (`db.Log = new PropertyBagTextWriter("SQL", PropertyBag);` )   The important point here is that it's self-contained.  Once we set the property to the PropertyBagTextWriter object,  we should never have to interact with it again.  The value should just appear in the dictionary when it's ready.

The code itself is fairly straightforward.  Start by deriving a new class from `StringWriter`, which is usually the best way to create a customized `TextWriter`.  That way, it'll handle the details of gathering and formatting the data from the stream, and all we have to deal with is the string at the end.

    class ProperyBagTextWriter : StringWriter
    {

Next, we're going to need to know the dictionary that the output will be stored in,  and the key, so we accept those in the constructor, and hold on to them for later:

    public ProperyBagTextWriter(string key, IDictionary bag)
    {
        this.key = key;
        this.bag = bag;
    }
    string key;
    IDictionary bag;

Then the key point:   When we get a Flush() call, we save the text we gathered so far into the dictionary under that key:

    public override void Flush()
    {
        base.Flush();
        bag[key] = base.ToString();
   }

However, since we can't count on the Flush always being called when we need it, we'll force a flush at other times, like during the Dispose() and after writing a line:

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            Flush();
    }
    public override void Write(char\[\] buffer, int index, int count)
    {
        base.Write(buffer, index, count);
        Flush();
    }


That all there is to it.  Besides the Linq2Sql log, I've also used it for the output from a XSLT transformation.


Source Code: I've decided to get with the times, and create (well, actually "use"... I created it a while ago), a GitHub account.  So, you can find this class, code from my future posts, and when I get around to it, code from my older post, at  [http://github.com/jamescurran/HonestIllusion](http://github.com/jamescurran/HonestIllusion)

