---
layout: post
title: Joins - LINQ's critical, overlooked feature.
categories: 
tags: 
---
As i was planning my rewrite of NJTheater.com I looked at a couple different Object-Relationship Mappers (mostly code generators which examined a database schema and produced one class per table to read and write rows to it.  All seemed particularly lacking because of this.  Then I find LINQ, and realized that I had found exact the system I was looking for.

To understand the problem, consider the main page of NJTheater.com, which on your average day looks something like this:

<strong>The Blue Bird (11/27/2007 - 12/31/2007)      
by Shakespeare Theatre of New Jersey (at F.M. Kirby Theater in Madison) </strong>

<strong>Doubt (11/27/2007 - 12/23/2007)      
by George Street Playhouse (at George Street Playhouse in New Brunswick) </strong>

<strong>Seussical: The Musical (12/1/2007 - 12/23/2007)      
by Bergen County Players (at Little Firehouse Theatre in Oradell) </strong>

The database is heavily normalized, so to retrieve that data, I need an SQL query that looks like this: 

<font face="Courier New">Select pl.Title, p.StartDate, p.EndDate, t.Name, v.Name, v.City      <br /></font><font face="Courier New">From Productions P      <br /></font><font face="Courier New">inner join Troupes T on p.TroupeID = t.TroupeID      <br /></font><font face="Courier New">inner join Venues V on p.VenueID = v.VenueID      <br /></font><font face="Courier New">inner join Plays pl on p.PlayID = pl.PlayID      <br /></font><font face="Courier New">where p. FirstDate &gt;= GetDate() and p.FirstDate &lt;= GetDate() + 7</font></p> p&gt;The actual query is a bit more complicated, but we'll go with that for now.   <p>That's one SQL statement which returns 3 to 30 rows (depending on the week) which holds all the data to be displayed.</p>  <p>Which brings us to the problem.  Most ORM systems would have me translate that into something like</p>  <p><font face="Courier New">ProductionList productions = db.Productions.WhereBetween("StartDate", datetime.Now, DateTime.Now.AddDay(7));</font>

And then use it like this:
<font face="Courier New">foreach (Production prod in productions)      <br />     Print prod.Play.Title +" ( " +prod.StartDate + " - " + prod.EndDate + ")"       <br />     Print "by " + prod.Troupe.Name " (at " + prod.Venue.Name + " in "+ prod.Venue.City +")"</font></p>  <p>(I made that syntax up, but it's fairly typical.)

Which brings us to the problem.  The first line would generate one SQL query  which would look something like this:

<font face="Courier New">Select p.*      <br /></font><font face="Courier New">From Productions P      <br /></font><font face="Courier New">where p. FirstDate &gt;= GetDate() and p.FirstDate &lt;= GetDate() + 7</font>

and would return about 15 records.  However, when it goes into the loop to render the actual page, then we sudden have a lot more SQL queries:

<font face="Courier New">Select title from play where playid = 123;      <br />select name from troupe where troupeid=131;       <br />select name, city  from venue where venueid=102;</font></p>

<font face="Courier New">Select title from play where playid = 143;    <br />select name from troupe where troupeid=134;     <br />select name, city  from venue where venueid=202;</font>   

<p>etc etc.  We go from one query returning N rows, to 3N+1 queries.  We've just massively increased the amount of work needed to be done to render the front (and most popular)page on the web site.</p>  <p>Which brings us back to LINQ.    The LINQ query I use is:

var productions = from prod in db.Productions    <br />        where prod.StartDate &lt; DateTime.Now.AddDays(7)     <br />        &amp;&amp; prod.StartDate &gt; DateTime.Now     <br />        orderby prod.Play.Title     <br />        select new  <br />               {     <br />                   Title = prod.Play.Title,     <br />                   Troupe = prod.Troupe.Name,     <br />                   Venue = prod.Venue.Name,     <br />                   City = prod.Venue.City,     <br />                   StartDate = prod.StartDate ,     <br />                   EndDate = prod.EndDate </p>  <p>               };

 <font face="Courier New">foreach (Production prod in productions)    <br />     Print prod.Title +" ( " +prod.StartDate + " - " + prod.EndDate + ")"     <br />     Print "by " + prod.Troupe " (at " + prod.Venue + " in "+ prod.City +")"</font>   
 
 The first thing you should notice is that except for the verbose select clause, this syntax is rather close to the one for my theoretic ORM.
 
 A more subtle difference is that I've slipped in a orderby based on one of the JOINs, which I'm not sure how I'd  do in the ORM.
 
 <strong>But the most important thing you should notice is that this will produce a single SQL statement, one that is virtual identical to the hand-crafted one I started this article with.</strong>
 
 <font face="ver">If you don't believe me, here's output from LINQPad:</font>
 
 SELECT [t].[Title], [t2].[ Name ] AS [Troupe], [t3].[ Name ] AS [Venue], [t3].[City],    <br />       [t0].[FirstPerformance] AS [StartDate], [t0].[LastPerformance] AS [EndDate]     <br />FROM [Productions] AS [t0]     <br />INNER JOIN [Plays] AS [t1] ON [t1].[PlayID] = [t0].[PlayID]     <br />INNER JOIN [Troupes] AS [t2] ON [t2].[TroupeID] = [t0].[TroupeID]     <br />LEFT OUTER JOIN [Venues] AS [t3] ON [t3].[VenueID] = [t0].[VenueID]     <br />WHERE ([t0].[FirstPerformance] &lt; @p0) AND ([t0].[LastPerformance] &gt; @p1)     <br />ORDER BY [t1].[Title]     <br />    <br />-- @p0: Input DateTime (Size = 0; Prec = 0; Scale = 0) [12/25/2007 7:00:54 PM]     <br />-- @p1: Input DateTime (Size = 0; Prec = 0; Scale = 0) [12/18/2007 7:00:54 PM]     <br />-- Context: SqlProvider(Sql2005) Model: AttributedMetaModel Build: 3.5.21022.8     <br />
 
 <a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2007%2f12%2f18%2fjoins-linq-s-critical-overlooked-feature.aspx"><img alt="kick it on DotNetKicks.com" src="http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2007%2f12%2f18%2fjoins-linq-s-critical-overlooked-feature.aspx" border="0" /></a>