using System;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;

namespace Tgs.Interceptors;

public class DiagnosticsInterceptor : AbstractInterceptor
{
	[FromServiceContext]
	public DiagnosticsInterceptorOptions Options { get; set; }

	public override async Task Invoke(AspectContext context, AspectDelegate next)
	{
		var typeName = context.ServiceMethod.DeclaringType.Name;
		var methodName = context.ServiceMethod.Name;
		var parameters = string.Join(
			", ",
			context.ServiceMethod.GetParameters().Select(p => p.Name));

		using var activity = Options.InterceptorActivitySource
			.StartActivity($"{typeName}.{methodName}({parameters})");
		
		try
		{
			await next(context);
		}
		catch (Exception)
		{
			_ = (activity?.AddTag("Error", true));

			throw;
		}
	}
}
