static class Util
{
	/// <summary>
	/// Retries the specified code block.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="retries">The number of retries to attempt before failing</param>
	/// <param name="secsDelay">The number of seconds to wait between retries</param>
	/// <param name="errorReturn">The error return.</param>
	/// <param name="onError">The Exception handling method</param>
	/// <param name="code">The code.</param>
	/// <returns></returns>
	/// <remarks>
	/// OK, this has very weird semantics.  A call will look something like this:
	/// <example><code><![CDATA[
	/// var msg = SystemClass.Retry(5, 1,
	///               ( ) => "Error",
	///               AppLog.Log,
	///               ( ) =>
	///               {
	///                       Console.WriteLine(DateTime.Now.ToLongTimeString());
	///                       if ((DateTime.Now.Second & 7) != 7)
	///                           throw new Exception("Bad");
	///                       return DateTime.Now.ToString();
	///               });
	/// ]]></code></example>
	/// <list type="bullet">
	/// <item><description>
	/// The third parameter is a function (or lambda expresion) taking no parameters, and returning
	/// the value to return in case of failure. (it done as a function to avoid evaluating it unless needed).
	/// </description></item>
	/// <item><description>
	/// The (optional) fourth parameter is a function (or lambda expresion) taking an exception as a parameter, and 
	///   returning nothing.  Can be used to log the exception on failure.
	/// </description></item>
	/// <item><description>
	/// The last parameter is a function (or lambda expresion) taking no parameters, which preforms the
	/// actual work which may need to be retried.
	/// </description></item>
	/// </list>
	/// </remarks>
	static public T Retry<T>(int retries, int secsDelay, Func<T> errorReturn, Action<Exception> onError, Func<T> code)
	{
		Exception ex = null;
		do
		{
			try
			{
				return code();
			}
			catch (Exception dde)
			{
				ex = dde;
				retries--;
				Thread.Sleep(secsDelay * 1000);
			}
		} while (retries > 0);
		onError(ex);
		return errorReturn();
	}

	static public T Retry<T>(int retries, int secsDelay, Func<T> errorReturn, Func<T> code)
	{
		return Retry(retries, secsDelay, errorReturn, ex => { }, code);
	}
}