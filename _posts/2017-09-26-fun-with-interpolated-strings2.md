---
layout: post
title: Fun with Interpolated Strings, Part II -- Putting FormattableString to work. 
tags: code programming csharp
---

Ok, I know, I promised a big new article -- and it is coming -- but, I had another idea I thought I give you.

So, let's return to everyone's favorite C#7 feature : Interpolated strings.

Now, one of the coolest things about them, is that they are completely compiler magic.  When you write

    var life="Hello";
    var universe="World";
    var everything=42;
	  
    var s =$"{life}, {universe} #{everything}!";
    //  s == "Hello, World #42!"
	  
The IL code generated is exactly as if you'd written:

      var life="Hello";
	  var universe="World";
	  var everything=42;
	  
	  var s =String.Format("{0}, {1} #{2}!", life, universe, everything);
	  //  s == "Hello, World #42!"
	 
Except not exactly.  It actually turns the interpolated string into a `FormattableString` object. So, it's more like:

      var life="Hello";
	  var universe="World";
	  var everything=42;
	  
	  var s =FormattableStringFactory.Create("{0}, {1} #{2}!", 
	                new object{life, universe, everything}).ToString();
	  //  s == "Hello, World #42!"


>> Ok, that's actually a lie.  Under some circumstances, the compiler will just optimize it down to just the `String.Format` call.  But, let's talk about the non-optimized use of the `FormattableString` class.

FormattableString has a couple properties, `Format` (string) and `ArgumentCount` (int), and a couple methods, `GetArgument(int)` and `GetArguments()`, which return an object and an array or object, respectively.

To start, we can manipulate those, to have some fun:

    void Main()
    {
       var one = 1;
       var two = 2;
       var three = 3;
 
       var abc = $"One =  {one}, Two = {two}, three = {three}";
	   // abc =   "One =  1, Two = 2, three = 3";
 
       var cba = Reverse($"One = {one}, Two = {two}, three = {three}");
	   // cba =   "One =  3, Two = 2, three = 1" 
    }
 
    string Reverse(FormattableString fstr)
    {
       object[] rargs = fstr.GetArguments();
       Array.Reverse(rargs);
       return string.Format(fstr.Format, rargs);
    }
 

But that's just child's play.  Let's try something a bit more serious.   In LINQ2SQL, a parameterized ExecuteQuery method, wants a string which looks a lot like a String.Format:

     context.ExecuteQuery("select * from table where ID = {0}", id);
	 
Normally, if we tried using an interpolated string here, it would yield a simple string, and we'd lose the advantage of parameters in the query.  But with a simple overload:

    IEnumerable<T> ExecuteQuery<T>(this DataContext db, FormattableString sql)
    {
       return db.ExecuteQuery<T>(sql.Format, sql.GetArguments());
    }

we can then use 

     context.ExecuteQuery($"select * from table where ID = {id}");
	 
and we have convenience and safety. But can we take this further?  

An ADO.NET parameterized query takes a difference looking SQL statement ("`select * from table where ID = @p0`") and needs it's parameters stuffed into `SqlParameter` object.  So, a simple query becomes:
 
       var custId = "CHOPS";
       var shipper = 2;
       
       
       var conn = new SqlConnection(@"Data Source=(local);Database=Northwind");
       conn.Open();
 
       
       using (var cmd = new SqlCommand())
       {
              cmd.Connection = conn;
              cmd.CommandText = "select ShippedDate from Orders where CustomerId = @Custid and ShipVia = @Shipper";
              cmd.Parameters.AddWithValue("Custid", custId);
              cmd.Parameters.AddWithValue("Shipper", shipper);
              var results = cmd.ExecuteReader();
       }
	   
Now, we could refactor that with the help of an extension method, and get it to:

       var results = conn.ExecuteQuery("select ShippedDate from Orders where CustomerId = @P0 and ShipVia = @P1", custId, shipper);
	   
But, now we have to use the same parameter names that our method is going to assign, so that implementation detail leaks out.

Now, by leveraging FormattableString, we can reduce that to just:

    var conn = new SqlConnection(@"server=...");
    conn.Open();
    var results = conn.ExecuteQuery($"select ShippedDate from Orders where CustomerId = {custId} and ShipVia = {shipper}");

So, we get the ease of hard-coding the values, but with all the goodness of using parameters.			  
			  

Here's how that was done:
 
    static readonly string[] fillers = { "@P0", "@P1", "@P2", "@P3", "@P4", "@P5", "@P6", "@P7", "@P8", "@P9"};
	  
    public static class Extensions
    { 
        public static SqlDataReader ExecuteQuery(this SqlConnection connection, FormattableString sql)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = connection; 
                cmd.CommandText = String.Format(sql.Format, fillers);
                for (int i = 0; i < sql.ArgumentCount; ++i)
                {
                    cmd.Parameters.AddWithValue("P"+i, sql.GetArgument(i));
                }
                return cmd.ExecuteReader();
            }
        }
    }

 
 
