  ---
    layout: default
    title: Dev102's Challenge #13 : Brackets
    ---

  <p>Many people skipped last week's challenge (like I had planned to).  As it turned out, I was the only blogger to responded.</p>  <p><a href="http://www.dev102.com/2008/07/21/a-programming-job-interview-challenge-13-brackets/">For this week's challenge,</a> they've gone back to a platform-neutral algorithm question:</p>  <blockquote>   <p><em>Your input is a string which is composed from bracket characters. The allowed characters are:’(', ‘)’, ‘['. ']‘, ‘{’, ‘}’, ‘&lt;’ and ‘&gt;’. Your mission is to determine whether the brackets structure is legal or not.</em></p> </blockquote>  <p>The simple sentence answer is "use a stack, pushing on an open character, and popping on a close character".  There are a few other things to look out for, but that's the basic concept.  For the actual code, I bypassed any kind of library Stack class, since we wanted the most efficient and for the very limited need of this function, I could jury-rig a faster one out of a char array.  Complexity is speed: O(N), space O(N)</p>  <pre class="c#">static bool TestBrackets(string testcase)
{
    // create a very simple stack.  
    // Since we push on open &amp; pop an close, stack need only be half the
    // size of the input string.  The +1 is needed because we only check
    // for too many opens after we've pushed.
    char[] stack = new char[(testcase.Length / 2) +1];
    
    int SP=0;
    foreach(char chr in testcase)
    {
        switch(chr)
        {
            // For each open character, push the close char.
            case '[':
                stack[SP++] = ']';
                break;
            case '&lt;':
                stack[SP++] = '&gt;';
                break;
            case '{':
                stack[SP++] = '}';
                break;
            case '(':
                stack[SP++] = ')';
                break;
                
            case ']':
            case '&gt;':
            case '}':
            case ')':
                // check for stack underflow (too many closes)
                // or a character we weren't expecting (bad  nesting)
                if (SP==0 || stack[--SP] != chr)
                    return false;  
                break;
        }
        // Check for stack overflow (too many opens)
        if (SP==stack.Length)
            return false;
    }
    // Finally, it's good if we've closed everything we've opened.
    return SP==0;
}</pre>