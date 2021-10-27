using System;
using System.Threading;
using System.Threading.Tasks;
using Tgs.ServiceUtility;

namespace Tgs.Interceptors
{
	internal class GetServiceInterceptor<TQuery, TResult> : IGetService<TQuery, TResult>
	{
		private readonly DiagnosticsInterceptorOptions _options;
		private readonly IGetService<TQuery, TResult> _target;

		public GetServiceInterceptor(IGetService<TQuery, TResult> target, DiagnosticsInterceptorOptions options)
		{
			_target = target ?? throw new ArgumentNullException(nameof(target));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public async ValueTask<TResult> GetAsync(TQuery query, CancellationToken cancellationToken = default)
		{
			var typeName = _target.GetType().Name;
			var methodName = nameof(GetAsync);
			var parameters = "query";

			using (var activity = _options.InterceptorActivitySource
				.StartActivity($"{typeName}.{methodName}({parameters})"))
			{
				try
				{
					return await _target.GetAsync(query).ConfigureAwait(false);
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
