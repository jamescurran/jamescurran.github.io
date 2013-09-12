---
layout: post
title: DEV102's Programming Job Interview Challenge 8
categories:  code c# .net dotnet csharp
tags:  code c# .net dotnet csharp
---
<style>
td { text-align:center; }
</style>

I skipped [last week's DEV102 challenge](http://www.dev102.com/net/a-programming-job-interview-challenge-7-coins-of-the-round-table/).   I didn't think my answer was right.  Turns out that it was. I was assuming that it had a limitation that would disqualify it.  I assumed that my solution would only work if you placed the coins in a tight grid with each newly-placed coin touching an existing coin.  As a practical matter, this is true.  It would be virtually impossible to properly place a coin mirroring the freely-placed previous coin without resorting to a tape measure and protractor.  And the placement would have to be exact for it to work.  So I suppressed my solution for lack of practicality, when all they really wanted was a theoretic solution. 

Anyway, onward to [this week's challenge, #8](http://www.dev102.com/net/a-programming-job-interview-challenge-8-a-needle-in-a-haystack/) (excerpted):

>You are writing a software component that receives a binary record every 20 millisecond. .... Your component goal is to alert whenever it identifies a specific expression (which is provided at the initialization process) in the stream of records - you are looking for a specific combination of binary records. 

The answer is, of course, a [*Finite State Machine*](http://en.wikipedia.org/wiki/Finite_State_Machine). To explain how one works, we need to come up with a example expression to search for.  Let's say these records come it four formats: Type A, type B, type C and Type D, and we are looking a sequence of records in the following pattern: ABACB (if you'd like, you can assume that there are many record types, and Type D represents "any record that's not type A, B or C").  So, we start in state "0". State 0 can be called "looking for first A record".  At state 0, if we find an A record, we move into state 1 ("Found first A, looking for first B").  If we find any other kind of record, we stay in state 0.  This can be expressed in table form as:

  <div align="center">   
   <table border="1" cellspacing="0" cellpadding="2" align="center">       <tr>         <td> </td>          <td>Next state</td>          <td>when </td>          <td>record </td>          <td>found</td>       </tr>        <tr>         <td>Current State &dArr;</td>          <td>A</td>          <td>B</td>          <td>C</td>          <td>D</td>       </tr>        <tr>         <td>0</td>          <td>1</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>     </table> </div> 
       
Next when we are in state 1, if we find a B record, we move into state 2, but the other transitions are a bit trickier.  If we find a C or D, we're back to state 0 ("looking for 1st A"), but if we find another A, we have to stay in state 1.  Adding that to our graph:
 
<div align="center">   <table border="1" cellspacing="0" cellpadding="2" align="center">       <tr align="center">         <td> </td>          <td>Next state</td>          <td>when </td>          <td>record </td>          <td>found</td>       </tr>        <tr>         <td>Current State &dArr;</td>          <td>A</td>          <td>B</td>          <td>C</td>          <td>D</td>       </tr>        <tr>         <td >0</td>          <td>1</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>1</td>          <td>1</td>          <td>2</td>          <td>0</td>          <td>0</td>       </tr>     </table> </div>  
   
Ok, now, we are in state 2 ("found AB, looking for 2nd A"), Here if we find an A, we move on to state 3 --- anything else, and we're back to state 0.
   
<div align="center">   <table border="1"  cellspacing="0" cellpadding="2" align="center">       <tr>         <td> </td>          <td>Next state</td>          <td>when </td>          <td>record </td>          <td>found</td>       </tr>        <tr>         <td>Current State &dArr;</td>          <td>A</td>          <td>B</td>          <td>C</td>          <td>D</td>       </tr>        <tr>         <td>0</td>          <td>1</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>1</td>          <td>1</td>          <td>2</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>2</td>          <td>3</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>     </table> </div>  
    
State 3 ("found ABA, looking for C"), is a bit tricky again.  If we find a C, naturally, we move into state 4. And if we find a D, were back into state 0.  But, if we an A, we step back to state 1.  And if we find a B, we step back only to state 2 (ie, we've found "ABAB" and the second "AB" may be the start of the pattern we want.)
    
<div align="center">   <table border="1"  cellspacing="0" cellpadding="2" align="center">       <tr>         <td> </td>          <td>Next state</td>          <td>when </td>          <td>record </td>          <td>found</td>       </tr>        <tr>         <td>Current State &dArr;</td>          <td>A</td>          <td>B</td>          <td>C</td>          <td>D</td>       </tr>        <tr>         <td>0</td>          <td>1</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>1</td>          <td>1</td>          <td>2</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>2</td>          <td>3</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>3</td>          <td>1</td>          <td>2</td>          <td>4</td>          <td>0</td>       </tr>     </table> </div>
   
At state 4, we enter the endgame.  We're trying to find "ABACB", and so far we're found "ABAC".  If the next record is a B, we have success ("*Let loose the pigeons!*").  If it's an A, we go to state 1 (as usually). Anything else, and we start over at state 0.
     
<div align="center">   <table  border="1" cellspacing="0" cellpadding="2" align="center">       <tr>         <td> </td>          <td>Next state</td>          <td>when </td>          <td>record </td>          <td>found</td>       </tr>        <tr>         <td>Current State &dArr;</td>          <td>A</td>          <td>B</td>          <td>C</td>          <td>D</td>       </tr>        <tr>         <td>0</td>          <td>1</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>1</td>          <td>1</td>          <td>2</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>2</td>          <td>3</td>          <td>0</td>          <td>0</td>          <td>0</td>       </tr>        <tr>         <td>3</td>          <td>1</td>          <td>2</td>          <td>4</td>          <td>0</td>       </tr>        <tr>         <td>4</td>          <td>1</td>          <td><strong>*</strong></td>          <td>0</td>          <td>0</td>       </tr>     </table> </div> 
       
Now, to put this into C# code, we merely need a simple pre-initialize int array following the structure of the chart we just built, and start with our state at 0.
      

          const int[,] states = new int [5,4]
          {
             {1,0,0,0},
             {1,2,0,0}
             {3,0,0,0}
             {1,2,4,0}
             {1,-1,0,0}
          };
               
          int state = 0;
  
  Then as a new record comes in, we just determine it's type, and update the state:
  
        bool MatchFound(Record newRecord)
       {
         // returns 0,1,2 or 3 for record type A,B,C or D respectively. 
        // can be assumed to be present, as per the spec. 
        
             int type = GetRecordType(newRecord);
             
        // Here's where the magic happens
        // just a simple index into an array.
        
            state = states[state, type];
        
             return state < 0;    // Report success or failure.
           }
        
And that's it.  Total state held between records: one integer.  Total work needed per record to determine pattern: one array lookup and one int comparison. 
  
And the real beauty of this approach is that if we wanted to look for other patterns *at the same time*, it could be done. For example, by just changing the states\[\] array in the above to this
        
             const int[,] states = new int [13,4]
             {
                 {1,5,9,0},
                 {1,2,9,0},
                 {3,6,9,0},
                 {1,2,4,0},
                 {10,-1,9,0},
                 {1, 6, 9, 0},
                 {1, 6, 7, 0},
                 {10, 5, 8, 0},
                 {-2, 5, 9, 0},
                 {10,5, 9, 0},
                 {1, 11, 9, 0},
                 {3, 6, 12, 0},
                {-3, 6, 9, 0}
          };

Then we'd be able to search for ABACB (as before, found when state = -1) and BBCCA (found when state = -2), plus one more pattern (found when state = -3).

###Class Homework

1. (simple) Try to figure out the third pattern that can be found using that state table. (It's a sequence of 5 records using just A B &amp; C)

1. (hopefully not possible)  Try to figure out a sequence of records that would cause the state machine to miss one of those patterns (Note: after one is found, we start over at state 0, so it's not intended to find overlapping sequences, such as "ABACBBCCA".  It'll find the first but not the second.)
  