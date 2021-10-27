using System;
using System.Collections.Generic;
using System.Threading;
using Tgs.ServiceUtility;

namespace Tgs.Interceptors
{
	internal class QueryServiceInterceptor<TQuery, TResult> : IQueryService<TQuery, TResult>
	{
		private readonly DiagnosticsInterceptorOptions _options;
		private readonly IQueryService<TQuery, TResult> _target;

		public QueryServiceInterceptor(IQueryService<TQuery, TResult> target, DiagnosticsInterceptorOptions options)
		{
			_target = target ?? throw new ArgumentNullException(nameof(target));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public IAsyncEnumerable<TResult> QueryAsync(TQuery query, CancellationToken cancellationToken = default)
		{
			var typeName = _target.GetType().Name;
			var methodName = nameof(QueryAsync);
			var parameters = "query";

			using (var activity = _options.InterceptorActivitySource
				.StartActivity($"{typeName}.{methodName}({parameters})"))
			{
				try
				{
					return _target.QueryAsync(query);
				}
				catch (Exception)
				{
					activity?.AddTag("Error", true);

					throw;
				}
			}
		}
	}
}
