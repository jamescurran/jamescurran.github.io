---
layout: post
title: Code Tune-up - LINQ example with GroupBy clause on a property in object
tags: code c# .net programming dotnet csharp code-tuneup
published: true
---
This is not really a "Code tuneup" like the ones I've done before.  It's more of a "Explain what the other guy is doing".

I recently came upon a blog post for an interesting bit of code-  ["LINQ example with GroupBy clause on a property in object"](http://geekswithblogs.net/Vipin/archive/2013/06/06/linq-example-with-groupby-clause-on-a-property-in-object.aspx)  but unfortunately, the whole "article" was just a single line of code without any context, which I don't see helping anyone.  

Here's the line of code:

<script src="https://gist.github.com/jamescurran/5763353.js">   </script>

The first thing that's a bit odd about it, is that while the article title talks about something being done "on a Property", no object properties are being used here.   Everything is referenced by the Field<> method, which suggests the next odd thing here --- LINQ being used on an untyped ADO.NET DataTable.  One would think, if you are going to use LINQ to process the data, you might as well use Linq2Sql or the Entity Framework to retrieve the data as a typed object.

This is also suggested by the use of AsEnumerable() on the source object.  Why is this needed?  Because the `Rows` property of `DataTable` is a `IEnumerable` but not a `IEnumerable<T>`.  The Linq methods (like `GroupBy`, which we use here), only work with strongly-typed `IEnumerable<T>` objects.  These are the hazards of using ADO.NET which hasn't changed much since .NET v1.1, before Generics.  Alternately, we could use `ListData.Rows.Cast<DataRow>()` instead of `ListData.AsEnumerable()` but it's pretty much a toss-up which is better.  I prefer the `Cast<DataRow>` method as it makes the functionality more explicit, but it really doesn't matter which you choose.

However, the next place he uses `AsEnumerable()` is completely unnecessary.  The `grp` object is already a strongly-typed `IEnumerable<T>`   It can be just deleted from the code and it will have no effect.   (Now that I've used the phrase "strongly-typed `IEnumerable<T>`" I should point out that it's redundant -- all `IEnumerable<T>`s are, by their very nature, strongly typed)

But before you delete the `AsEnumerable()` -- hold off for a second - there's more we may want to delete.

If we give ourselves a DataTable to work with (via the GetDataTable() method, given below), and run the query, we get the output:

	IEnumerable<> (3 items)
		brokerID 	listOfPolicies 
		1001 		IEnumerable<> (3 items) 
				list 
				01-012345 
				02-345678 
				03-567890 
 
		1002 		IEnumerable<> (1 item)
				list 
				01-012346 
 
		1003 		IEnumerable<> (2 items)
				list 
				01-012347 
				02-023456 
 
(Output The .Dump() extension method is a feature of LINQPad (www.linqpad.com), which is the necessary tool for writing code like this)
    


One problem with understanding what's going on there, is that there is a lot going on.  The data is being structured one way, and then immediately re-structured anyway.  Let's take this step by step, starting with a simple group-by:

	var ListData = GetDataTable();
    var brokerPolicies = from pol in ListData.AsEnumerable()
                     group pol by pol.Field<int>("intUserID");

    brokerPolices.Dump();
    
GetDataTable() just creates a simple DataTable in memory, so we don't have to bother dragging an actual database into this eample.
    
   Running this, `brokerPolices` is of type `IEnumerable<IGrouping<Int32,DataRow>>`. in other words, a collection of IGrouping object.  And an IGrouping is also just a collection, in this case, of DataRow, with a key associated with collection.  Here that key is an int.

The data itself look like this:
 
    Key=  1001 
	intUserID 	intPolicySequence 	intPolicyNumber
	1001 		1 			12345 
	1001 		2 			345678 
	1001 		3 			567890 
 
    Key=  1002 
	intUserID 	intPolicySequence 	intPolicyNumber 
	1002 		1 			12346 
 
    Key=  1003 
	intUserID 	intPolicySequence	intPolicyNumber
	1003 		1 			12347 
	1003 		2 			23456 
 
To understand the output from the LINQ statement, note that the bit between the `group` and the `by` in the expression, functions much like a LINQ `select` clause, and the part after the `by` gives us the key.    (The final piece -- the `into`, which we didn't use here, functions like a LINQ `let` -- putting the current result into a temporary variable to be used again later in the query.)

You'll note in the output that since we grouped the whole record by the UserID,  the ID value appears redundantly as both the Key, and inside the records.

But, as I just said, we don't have to use the query variable there -- we can build a new object right in the middle of the `group by`. 

	var brokerPolicies = from pol in ListData.Rows.Cast<DataRow>()
			group  new { PolicySequence = pol.Field<short>("intPolicySequence"), PolicyNumber = pol.Field<int>("intPolicyNumber") }
			by pol.Field<int>("intUserID");

Which gives us the output:

	Key=  1001 
		PolicySequence PolicyNumber
		1 		12345 
		2 		345678 
		3 		567890 
 
	Key=  1002 
		PolicySequence PolicyNumber 
		1 		12346 
	
	Key=  1003 
		PolicySequence	PolicyNumber
		1 		12347 
		2 		23456 

That gives us nice strongly-typed objects to work with, but our final goal was a formatted string, so let's make that group object:

<script src="https://gist.github.com/jamescurran/5763365.js">   </script>

and that gives us basically what we had in the original:

	Key=  1001 
		"01-012345"
		"02-345678"
		"03-567890"
 
	Key=  1002 
		"01-012346"
 
	Key=  1003 
		"01-012347" 
		"02-023456 "
 

As Promised, here's the GetDataTable() code.

<script src="https://gist.github.com/jamescurran/5763372.js">   </script>
