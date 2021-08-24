using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Interceptors
{
	internal class CommandServiceInterceptor<TCommand> : ICommandService<TCommand>
	{
		private readonly DiagnosticsInterceptorOptions _options;
		private readonly ICommandService<TCommand> _target;

		public CommandServiceInterceptor(ICommandService<TCommand> target, DiagnosticsInterceptorOptions options)
		{
			_target = target ?? throw new ArgumentNullException(nameof(target));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public async ValueTask ExecuteAsync(TCommand command, CancellationToken cancellationToken = default)
		{
			var typeName = _target.GetType().Name;
			var methodName = nameof(ExecuteAsync);

			using (var activity = _options.InterceptorActivitySource
				.StartActivity($"{typeName}.{methodName}()"))
			{
				try
				{
					await _target.ExecuteAsync(command).ConfigureAwait(false);
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
