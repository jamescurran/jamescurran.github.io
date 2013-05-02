---
layout: post
title: Code Tuneup - DataConvert
categories: csharp dotnet code-tuneup
tags: csharp dotnet code-tuneup
---

(Ya'know, I think I'll make "Code Tuneup" a regular feature on Honest Illusion -- If a site that goes months without a posting can be said to have "regular features")

CODE Magazine, one of the last hard-core programming magazines left, just put out it's May-June 2013 issue.   One of the article is Paul. D Sheriff's  [Creating Collections of Entity Objects](http://www.code-magazine.com/article.aspx?quickid=1305031&page=1) (Note: unless you're a subscriber, you'll only be able to see the first 1/3rd of the article online.)

Now, while most of the article is quite good, Paul starts with some basic routines and build to more advanced one, but he doesn't take it quite far enough.

It's seems he's a bit timid when using generics, using them for some parts of a method, but not others, not unleashing their full potential .  Let's see what we can do about that.

Our first sample is this:
        

	public static T ConvertTo<T>(object value, object defaultValue)
		where T : struct
        {
            if (value.Equals(DBNull.Value))
                return (T)defaultValue;
            else
                return (T)value;
        }

Background: In the article, we are reading data from a database, and putting it into strongly typed class.  This method is part of the low-level code to convert a boxed value into an hard number.   It would be used like:

	foreach (DataRow dr in dt.Rows)
	{
		entity = new Product();

		entity.IntroductionDate =
			DataConvert.ConvertTo<DateTime>(dr["IntroductionDate"], default(DateTime));
					 
		entity.Cost =
			DataConvert.ConvertTo<decimal>(dr["Cost"],  default(decimal));         

Now the odd thing about the way Paul wrote it, is that we *know* the second parameter needs to be the same type as we are returning, *and* that it will always be a value type.  Nevertheless, it's being passed as an object, requiring a pointless boxing & unboxing.

So, we know it's type T, let's use that:

	public static T ConvertTo<T>(object value, T defaultValue)

Now, the immediate advantage is that if we do something dumb like 

		entity.Cost =
			DataConvert.ConvertTo<decimal>(dr["Cost"],  default(int));         

This now becomes a compiler error, instead of a run-time error as it would be if defaultValue were an object.  But more importantly, we can now have the compiler predict datatype.  So, we *could* write that line as:

		entity.Cost =
			DataConvert.ConvertTo(dr["Cost"],  default(decimal)); 
			
And the compiler will figure out that `T` is `decimal`.  But that would be confusing for readers, so let's go another way:

	public static T ConvertTo<T>(object value, T defaultValue = default(T))
		where T : struct
	{...}
	
Now, by making `defaultValue` a default parameter (!!) we can leave it off, and the compiler will fill in the right value:

		entity.Cost =
			DataConvert.ConvertTo<decimal>(dr["Cost"]); 

We'd only have to specify `defaultValue` if we wanted something beside the default value for the type:

		entity.Cost =
			DataConvert.ConvertTo<decimal>(dr["Cost"], -99.99); 

Ok, now let's work on the DataConvert part.  It's ugly and requires too much typing. (All the best programmers are very lazy...), so let's get rid of it.  The easiest way is with a extension method.  The obvious way would be to refactor it so we could write that line as:

		entity.Cost =
			dr["Cost"].ConvertTo<decimal>(); 
            
but that would require putting the extension method on object, which is evil, so let's go another way -- adding it to DataRow:

	public static T ConvertTo<T>(this DataRow dr, string colName,, T defaultValue = default(T))
		where T : struct
        {
            object value = dr[colName];
            // etc 
            
Now we'd write that line as:

		entity.Cost = dr.ConvertTo<decimal>("Cost");

But wait, now the name is confusing, OK, one more change:

<script src="https://gist.github.com/jamescurran/5495409.js">     </script>

With that, the line becomes:

		entity.Cost = dr.ReadAs<decimal>("Cost");

----

Next, Paul gives us a longer function to read in a collection of objects from a database:

    public class ManagerBase
    {
      public List<T> BuildCollection<T>(Type typ, SqlDataReader rdr)
      {
        List<T> ret = new List<T>();
        T entity;

        // Get all the properties in Entity Class
        PropertyInfo[] props = typ.GetProperties();

        while (rdr.Read())
        {
          // Create new instance of Entity
          entity = Activator.CreateInstance<T>();

          // Set all properties from the column names
          // NOTE: This assumes your column names are the 
          //       same name as your class property names
          foreach (PropertyInfo col in props)
          {
            if (rdr[col.Name].Equals(DBNull.Value))
              col.SetValue(entity, null, null);
            else
              col.SetValue(entity, rdr[col.Name], null);
          }

          ret.Add(entity);
        }

        return ret;
      }
    }

(note: Here Paul has a rather cool technique using reflection to fill the object, which to understand you really should read the [article](http://www.code-magazine.com/article.aspx?quickid=1305031&page=1). )

So, what needs to be fixed fix?  Again, the problem is using generics, but not using them enough.  You'll see that we have a type parameter, `T`, and a parameter which is a Type, `typ`.  They have to refer to the  same type, or bad things happen.  So, why are we specifying the same type two different ways?  

We only use `typ` once, in the line:

        PropertyInfo[] props = typ.GetProperties();

We could restate that as:

        PropertyInfo[] props = typeof(T).GetProperties();
 
and eliminate the need for `typ` altogether.  
 
Another change I'd like to make is this line:
 
	 entity = Activator.CreateInstance<T>();
to:

    entity =  new T();
         
The change has no effect on the method at all (the two versions would even generate the same IL code), but I like the second version better because I figure most programmers would be more familiar with the `new` syntax rather than the `CreateInstance` syntax.  But when I did that, Visual Studio showed me another reason it should be changed --- It gave me a red squiggly line.

Both lines require that T have a public constructor that takes no parameters.  But with the new syntax, the compiler can realize that restriction; the `CreateInstance` syntax hid it. So, we need to add:

	where T : new()
	
to the method signature.  And while we are messing with the signature, there are a couple other things to change.

In Paul's version, this method lives in a common base class you would derive your Data Access classes from.  This is a bit messy.  Why not make it an extension method too?  We can add it to` SqlDataReader` or better yet, `IDataReader`.  And, again it'll need a new name to better convey it usage:

<script src="https://gist.github.com/jamescurran/5494793.js">    </script>

So now instead of writing:

	ret = BuildCollection<Product>(typeof(Product), rdr);

We'd use it by writing:

	ret = rdr.ReadCollection<Product>();

