  ---
    layout: default
    title: Creating an Entity Framework connection from another one.
    ---

  <p>Database connection strings used to be simple.  Well, simple, once you learned the arcane syntax,  But, at least they had stayed the same for about a decade.  But with the EntityFramework, they took on an even more arcane “connection string – within –a –connection string” format.  And while the inner connection string related to your database, the wrapping connection string was intimately tied to the entity context.</p>
  
<p>So, in that past, one App.config connect string entry was good for all your DB needs for one database.  Now, if you have several different EF contexts, all referring to tables in the same physical database, you need to have several connection strings, with the outer string different and the inner part the same --- until you need to connect to a different database, then you leave the existing outer strings and change all the inner string in parallel.  This really seems to me to be a design flaw on Microsoft’s part, but let’s see if we can work something to make it easier.  We’d really like to have one connection string for all our EF needs.  </p>
  
<p>If you’re using more than one EF context, you’re probably using one for most things – your core business objects – and others for more cross-project, framework needs (auditing, logging etc.).   So, let’s say you have one connection string set up for the main context, then all we need do is write a method which takes one EF context, extracts the database connection information, and uses that to build a new EF context.</p>
  
<p>And the Entity Framework meets us halfway, providing the EntityConnectionStringBuilder class.  We just create an EntityConnectionStringBuilder object, set three properties, and boom—we have our new connection string.  And one of those properties (<b>Provider</b>) is pretty much fixed (usually, "System.Data.SqlClient", but even if it’s for a different vender, you’ll probably be standardized on one database vendor).   The second (<b>Metadata</b>) is basically fixed for the context (I assume there’s sometime a need to vary that, but I haven’t seen one).  That just leaves <b>ProviderConnectionString, </b>which is where things get tricky.  </p>
  
<p>While that EntityContext object does contain the needed connection string, it’s buried three levels down,  in a property not exposed by the IDbConnection interface that we are given.  We have to cast a property to an EntityConnection object to be able to access it.</p>
  
<p>Putting all that together, here’s the code:</p>
  <div class="csharpcode">   
<pre class="alt"><span class="lnum">   1:  </span><span class="kwrd">public</span> SecondEFContext(FirstEFContext context1)    // new ctor defined in partial class</pre>
  
<pre><span class="lnum">   2:  </span>{</pre>
  
<pre class="alt"><span class="lnum">   3:  </span>            var eb = <span class="kwrd">new</span> EntityConnectionStringBuilder();</pre>
  
<pre><span class="lnum">   4:  </span>            eb.Provider = <span class="str">"System.Data.SqlClient"</span>;</pre>
  
<pre class="alt"><span class="lnum">   5:  </span>            var context1Conn = context1.Connection <span class="kwrd">as</span> EntityConnection;</pre>
  
<pre><span class="lnum">   6:  </span>            eb.ProviderConnectionString = context1Conn.StoreConnection.ConnectionString;</pre>
  
<pre class="alt"><span class="lnum">   7:  </span>            eb.Metadata = <span class="str">@"res://*/SecondEF.csdl|res://*/SecondEF.ssdl|res://*/SecondEF.msl"</span>;</pre>
  
<pre><span class="lnum">   8:  </span> </pre>
  
<pre class="alt"><span class="lnum">   9:  </span>            var entC = <span class="kwrd">new</span> EntityConnection(eb.ToString());</pre>
  
<pre><span class="lnum">  10:  </span> </pre>
  
<pre class="alt"><span class="lnum">  11:  </span>            <span class="kwrd">return</span> <span class="kwrd">new</span> SecondEFContext(entC);</pre>
  
<pre><span class="lnum">  12:  </span>}</pre>
</div>
<a href="http://dotnetshoutout.com/Honest-Illusion-Creating-an-Entity-Framework-connection-from-another-one"><img style="border:0px currentColor;" alt="Shout it" src="http://dotnetshoutout.com/image.axd?url=http%3A%2F%2Fhonestillusion.com%2Fblogs%2Fblog_0%2Farchive%2F2011%2F05%2F02%2Fcreating-an-entity-framework-connection-from-another-one.aspx" /></a>
<a href="http://www.dotnetkicks.com/kick/?url=http%3a%2f%2fhonestillusion.com%2fblogs%2fblog_0%2farchive%2f2011%2f05%2f02%2fcreating-an-entity-framework-connection-from-another-one.aspx"><img border="0" alt="kick it on DotNetKicks.com" src="http://honestillusion.com/controlpanel/blogs/http%3A%2F%2Fwww.dotnetkicks.com%2FServices%2FImages%2FKickItImageGenerator.ashx%3Furl%3Dhttp%253a%252f%252fhonestillusion.com%252fblogs%252fblog_0%252farchive%252f2011%252f05%252f02%252fcreating-an-entity-framework-connection-from-another-one.aspx" /></a>