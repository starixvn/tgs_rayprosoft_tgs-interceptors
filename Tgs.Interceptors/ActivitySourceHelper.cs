using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tgs.Interceptors;

public static class ActivitySourceHelper
{
	private static readonly ConcurrentDictionary<string, ActivitySource> _ActivitySources = new();

	public static ActivitySource GetActivitySource(string name)
		=> _ActivitySources.GetOrAdd(name, key => new ActivitySource(key));

	public static ActivitySource GetActivitySource<T>()
		=> GetActivitySource(typeof(T).FullName);
}
