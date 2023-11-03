---
layout: post
title: A  Literary Clock in Blazor
tags: code programming blazor clock dotnet
---

This post is for day 13  of the [2022 C# Advent Calendar](https://www.csadvent.christmas/) operated by [Matthew Groves](https://crosscuttingconcerns.com/). Thanks for letting me participate!

When I last blogged (um.. four *months* ago...), we discussed putting time-related quotes into your PowerShell prompt (Click the left arrow at the top of the page to see that one), and I mentioned a Blazor version.  It's time to present that.

Some background: back in 2011, [The Guardian](https://www.theguardian.com/books/table/2011/apr/21/literary-clock?CMP=twt_gu) built a list of literary quotes containing a time of day, from reader contributions, and made the lists publically available.  They been re-used on a [number](https://www.instructables.com/Literary-Clock-Made-From-E-reader/) of [projects](http://jenevoldsen.com/literature-clock/).

One such project is [https://github.com/JohannesNE/literature-clock](https://github.com/JohannesNE/literature-clock), which uses Javascript to present the appropriate quotes on a webpage.  The repository also maintains a pipe delimited list of all the quotes, and has a program written in R to read that file and generate individual json files for each minute.

That's all well & good, but I do C#, so I figured I'd port that in Blazer (while making the R program into a C# console app).


After that app runs, for each minute (well, for 958 of the 1440 minutes in a day), there's a json file with a name in the form "HH_MM.json", which holds an array of objects which look like this:

<script src="https://gist.github.com/jamescurran/11d9dea47ef0741128fcc0959afb2ce1.js"> </script>

Not sure how fast the R version is, but on my PC, the C# version takes 13 seconds to produce the 958 files.

So, let's look at the Blazor page, a piece at a time. We need to do something once every minute, so on start, we set up a timer, which calls the method which does the real work:

	protected override  Task OnInitializedAsync()
	{
		_timer = new Timer(obj => OnTimerCallback(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
		return Task.CompletedTask;
	}
	private void OnTimerCallback()
	{
		_ = InvokeAsync(() =>
		{
			UpdateText(DateTime.Now);
		});
	}

The reason we pass in the time will become clear in a minute.  In `UpdateText`, we read the quote file for the passed time:  

	protected async  void UpdateText(DateTime time) 
	{
		var strTime =time.ToString("HH_mm");
		Quote[] quotes = await Http.GetFromJsonAsync<Quote[]>($"times/{strTime}.json") ?? Array.Empty<Quote>();

But, since not every minute has a quote, if we don't find any for the current minute, we'll hold on to and use the previous minutes -- or, if we don't have any already (i.e., we just started), we just go backwards in time until we find one:

		if (quotes.Any())
			_quotes = quotes;
		else if (_quotes == null)
		{
			UpdateText(time.AddMinutes(-1));
			return;
		}

Then we just randomly pick a quote from the array

		_quote = _quotes[Rnd.Next(quotes.Length)];

And bind it to the page:

		<div id="main_text">
			<blockquote id="lit_quote">@_quote.Prefix<em>@_quote.Quote_time_case</em>@_quote.Suffix</blockquote>
			<cite>
				-
				<span id="book">@_quote.Title</span>,
				<span id="author">@_quote.Author</span>
			</cite>
		</div>

The javascript version includes a formula to adjust the font size to accomodate the widely differing quote lengths.  This is easily transaled into C#:

		var quote_len = _quote.Quote_first.Length +
		                 _quote.Quote_time_case.Length +
		                 _quote.Quote_last.Length;

		_fontSize = 7.000864 - 0.01211676 * quote_len + 0.00001176814 * quote_len * quote_len - 1.969435e-9 * quote_len * quote_len * quote_len;

And Blazor:

        <style>
        	#lit_quote {
        		font-size: @(_fontSize)vw
        	}
        </style>

You can see the clock in action (along with two other online clocks of mine) here: [https://noveltheory.com/clock/](https://noveltheory.com/clock/)


The full project repository (which includes a folk of the original Javascript version) is here [https://github.com/jamescurran/literature-clock](https://github.com/jamescurran/literature-clock).



