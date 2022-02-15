using Tgs.Interceptors;
using Tgs.ServiceUtility;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInterceptionService(this IServiceCollection services)
		=> services
			.AddSingleton<DiagnosticsInterceptorOptions>();

	public static IServiceCollection AddInterceptionService(this IServiceCollection services, DiagnosticsInterceptorOptions options)
		=> services
			.AddSingleton(options);

	public static IServiceCollection AddServiceUtiltiyInterceptors(this IServiceCollection services)
	{
		services.TryDecorate(typeof(IGetService<,>), typeof(GetServiceInterceptor<,>));
		services.TryDecorate(typeof(IQueryService<,>), typeof(QueryServiceInterceptor<,>));
		services.TryDecorate(typeof(ICommandService<>), typeof(CommandServiceInterceptor<>));

		return services;
	}
}
