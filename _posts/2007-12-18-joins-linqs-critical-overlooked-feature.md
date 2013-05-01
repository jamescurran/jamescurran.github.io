---
layout: post
title: Joins - LINQ's critical, overlooked feature.
categories: csharp programming code linq dotnet
tags: 
---
As i was planning my rewrite of NJTheater.com I looked at a couple different Object-Relationship Mappers (mostly code generators which examined a database schema and produced one class per table to read and write rows to it.  All seemed particularly lacking because of this.  Then I find LINQ, and realized that I had found exact the system I was looking for.

To understand the problem, consider the main page of NJTheater.com, which on your average day looks something like this:

**The Blue Bird (11/27/2007 - 12/31/2007)      
by Shakespeare Theatre of New Jersey (at F.M. Kirby Theater in Madison)**

**Doubt (11/27/2007 - 12/23/2007)      
by George Street Playhouse (at George Street Playhouse in New Brunswick)**

**Seussical: The Musical (12/1/2007 - 12/23/2007)      
by Bergen County Players (at Little Firehouse Theatre in Oradell)**

The database is heavily normalized, so to retrieve that data, I need an SQL query that looks like this: 

<script src="https://gist.github.com/jamescurran/5472612.js">   </script>

The actual query is a bit more complicated, but we'll go with that for now.  

That's one SQL statement which returns 3 to 30 rows (depending on the week) which holds all the data to be displayed.

Which brings us to the problem.  Most ORM systems would have me translate that into something like

     ProductionList productions = db.Productions.WhereBetween("StartDate", datetime.Now, DateTime.Now.AddDay(7));

And then use it like this:

	foreach (Production prod in productions)     
		Print prod.Play.Title +" ( " +prod.StartDate + " - " + prod.EndDate + ")"  
		Print "by " + prod.Troupe.Name " (at " + prod.Venue.Name + " in "+ prod.Venue.City +")"

(I made that syntax up, but it's fairly typical.)

Which brings us to the problem.  The first line would generate one SQL query  which would look something like this:

	Select p.* 
	From Productions P 
	where p. FirstDate >= GetDate() and p.FirstDate <= GetDate() + 7


and would return about 15 records.  However, when it goes into the loop to render the actual page, then we sudden have a lot more SQL queries:

	Select title from play where playid = 123; 
	select name from troupe where troupeid=131; 
	select name, city from venue where venueid=102;

	Select title from play where playid = 143; 
	select name from troupe where troupeid=134; 
	select name, city from venue where venueid=202;

etc etc.  We go from one query returning N rows, to 3N+1 queries.  We've just massively increased the amount of work needed to be done to render the front (and most popular)page on the web site.

Which brings us back to LINQ.    The LINQ query I use is:

	var productions = from prod in db.Productions 
		where prod.StartDate < DateTime.Now.AddDays(7) 
		&& prod.StartDate > DateTime.Now 
		orderby prod.Play.Title 
		select new 
		{ 
			Title = prod.Play.Title, 
			Troupe = prod.Troupe.Name, 
			Venue = prod.Venue.Name, 
			City = prod.Venue.City, 
			StartDate = prod.StartDate , 
			EndDate = prod.EndDate
		};

	foreach (Production prod in productions) 
		Print prod.Title +" ( " +prod.StartDate + " - " + prod.EndDate + ")" 
		Print "by " + prod.Troupe " (at " + prod.Venue + " in "+ prod.City +")"
		
 
 The first thing you should notice is that except for the verbose select clause, this syntax is rather close to the one for my theoretic ORM.
 
 A more subtle difference is that I've slipped in a orderby based on one of the JOINs, which I'm not sure how I'd  do in the ORM.
 
 **But the most important thing you should notice is that this will produce a single SQL statement, one that is virtual identical to the hand-crafted one I started this article with.**
 
 If you don't believe me, here's output from LINQPad:
 
	SELECT [t].[Title], [t2].[ Name ] AS [Troupe], [t3].[ Name ] AS [Venue], [t3].[City], 
	[t0].[FirstPerformance] AS [StartDate], [t0].[LastPerformance] AS [EndDate] 
	FROM [Productions] AS [t0] 
	INNER JOIN [Plays] AS [t1] ON [t1].[PlayID] = [t0].[PlayID] 
	INNER JOIN [Troupes] AS [t2] ON [t2].[TroupeID] = [t0].[TroupeID] 
	LEFT OUTER JOIN [Venues] AS [t3] ON [t3].[VenueID] = [t0].[VenueID] 
	WHERE ([t0].[FirstPerformance] < @p0) AND ([t0].[LastPerformance] > @p1) 
	ORDER BY [t1].[Title] 

	-- @p0: Input DateTime (Size = 0; Prec = 0; Scale = 0) [12/25/2007 7:00:54 PM] 
	-- @p1: Input DateTime (Size = 0; Prec = 0; Scale = 0) [12/18/2007 7:00:54 PM] 
	-- Context: SqlProvider(Sql2005) Model: AttributedMetaModel Build: 3.5.21022.8 
 
 <a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2007%2f12%2f18%2fjoins-linq-s-critical-overlooked-feature.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2007%2f12%2f18%2fjoins-linq-s-critical-overlooked-feature.aspx" border="0" /></a>