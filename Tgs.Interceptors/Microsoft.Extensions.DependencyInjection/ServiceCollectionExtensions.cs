using Tgs.Interceptors;
using Tgs.ServiceUtility;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInterceptionService(this IServiceCollection services)
			=> services
				.AddSingleton<DiagnosticsInterceptorOptions>();

		public static IServiceCollection AddInterceptionService(this IServiceCollection services, DiagnosticsInterceptorOptions options)
			=> services
				.AddSingleton(options);

		public static IServiceCollection AddServiceUtiltiyInterceptors(this IServiceCollection services)
			=> services
				.Decorate(typeof(IGetService<,>), typeof(GetServiceInterceptor<,>))
				.Decorate(typeof(IQueryService<,>), typeof(QueryServiceInterceptor<,>))
				.Decorate(typeof(ICommandService<>), typeof(CommandServiceInterceptor<>));
	}
}
