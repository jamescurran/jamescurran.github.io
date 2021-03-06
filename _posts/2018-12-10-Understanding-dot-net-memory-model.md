---
layout: post
title: Here, There and Everywhere - A Simple Look at .NET Memory Model
tags: code programming csharp dotnet
---

This post is for day 10  of the [2018 C# Advent Calendar](https://crosscuttingconcerns.com/The-Second-Annual-C-Advent) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

This is actually a generic .NET article, not specifically a C# one, but the examples will be in C#, so it counts.

Often, when I'm in a job interview, I'm asked about the .NET memory model.  As I explain it, the interviewers eyes begin to glaze over, and I realize that they are really just listening for me to say the magic words **Stack** and **Heap**.  It's seems everyone knows the .NET memory model uses a Stack and a Heap, but not everyone understands what they are.

First of all, those are incredibly bad names for those two areas.  Why is The Stack called the Stack?  Why The Heap?  If The Stack was a heap and The Heap a stack, would you know the difference?   Let's take a breif historical side note,  a quick review of how we got here.  

> Back in the early 1970's, when the C language was being defined, the original compiler out of AT&T was written for a stack-based processor, meaning the CPU had a register dedicated to managing a push-down stack, which held the return address for function calls, and the content of registers that were "pushed" onto the stack.  Since it was a handy temporary storage area, the compiler designers decided to store function local variables there.  Now, they were accessed directly, instead of going through the pop-push mechanism of a stack, so while they were on "the stack" they weren't really part of "a stack".
>
> Now, that was for _temporary_ storage.  For more long-term memory needs, we needed to allocate memory.  In C, this was done in the *malloc()* library function (which was part of the C Run-time library, which was completely separate from the C compiler).  It had to manage the very limited memory space (64 KB would be considered a lot of memory back in those days), so they choose to use a Heap data structure (a type of tree), to manage RAM being allocated and deallocated.  
>
> Flash forward 40 years, and as we progressed from C to C++ to Java to C#, the essence of RAM memory management is now quite different, but the names of the areas have persisted, even though the Stack may not be a stack, and the Heap almost certainly isn't a Heap.

So, let's get rid of those names, and give these areas of memory new, more descriptive names.

I prefer to call the place for local storage (aka "The Stack") as "*Over Here*".  And, naturally, the place for longer-term storage ("The Heap") is called "*Over There*"  (And Yes, I could have used the classic WWII song "Over There" for this blog post title, but I perfer the Beatles).

Now, on to our actual subject.  Consider this code:

        void SomeMethod()
        {
            int anInt = 1234;;
            double aDouble = 1.234;;
            string aString = "Hello World";
            Node aNode = new Node();

            // rest of method is unimportant
        }

 Method local value type data, which intrinsically has a short lifspan, is stored "over here": a memory space that's easy to allocated and deallocate, and can be accessed quickly.  It's on the desk, not in the bookcase, to use the analogy from my [last blog article](https://honestillusion.com/2018/11/01/throwing-the-book-at-em-knowing-your-coumpter-storage.html).  Exactly where it is and how it's organized (stack or heap or linked-list or whatever) isn't important.  So, in our example `anInt` and `aDouble` go there.

But that's for value types.  What about reference types?  They are split up-- the actual data of the object is stored "over there", while a "reference"  (some would say a "pointer" to drag in more ancient terminology from C implementation details) is stored "over here".  The reference tells us where to find the real data.  I like using the analogy of a business card -- it's not the actual person, but a much smaller thing which tells you who they are and where you can find them.  So, `aString` and `aNode` are split like this.

However -- it's not nearly that simple.   For a rather basic type, like `System.String`, then saying "reference over here, data over there" in fine.  But what a about a more complex type, like `Node` in our example?  How is that stored in memory?  Let's consider this class definition:

        class Node
        {
            int anInt;
            double aDouble;
            string aString;
            Node nextNode;

            public Node()
            {
                anInt = 1234;;
                aDouble = 1.234;;
                aString = "Hello World";
                nextNode = new Node();
            }

            // rest of class is unimportant
            // oh, and ignore the fact that the ctor will recurse
            // until we run out of memory.
        }

 Here's where the whole Stack/Heap terminology falls apart.  When you new up an object, you are creating -- Over There -- the object's own personal *over here*.  It holds the `anInt` and `aDouble` directly, and references to the string, in separate Over Theres.  All of these live Over There (that is, in "The Heap"), but it acts just like if it were *Over Here*.   And, of course, `nextNode` reference a new object, living elsewhere *over there*, which contains it's own little *over here* for it's data.

And, of course the master Over Here (i.e, The Stack) has a reference pointing to that adhoc Over Here that's actually living Over There.

---

OK, personal promotion time:  If this year is anything like last year's advent post, this blog will see a brief 10,000+% increase in traffic, so let to take this opportunity to plug a couple other posts here.  First of all is my [immediate previous article](https://honestillusion.com/2018/11/01/throwing-the-book-at-em-knowing-your-coumpter-storage.html) which I reference above, and is also regarding computer memory.

The other is one I wrote eight years ago, as the start of a on-line parody of soap operas set in the .NET world, called "[Naked Came the Null Delegate](https://honestillusion.com/2010/10/09/naked-came-the-null-delegate-chapter-1-i-disposable.html)".  It's serialized across the blogosphere, with several notably authors (Charles Petzold, Jon Skeet et al) contributing.  (and I'm more than willing to accept new chapters....)   (Note: in the last eight years, some of the links to the next chapter at the end of each post have broken.   The master list [here](https://nakedcamethenulldelegate.wordpress.com/2010/10/09/the-story/) has been updated with the correct links.
