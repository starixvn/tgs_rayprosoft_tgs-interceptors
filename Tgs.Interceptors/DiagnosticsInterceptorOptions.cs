using System.Diagnostics;

namespace Tgs.Interceptors
{
	public class DiagnosticsInterceptorOptions
	{
		public ActivitySource InterceptorActivitySource { get; set; } =
			new ActivitySource($"Tgs.Interceptors.DiagnosticsInterceptor");
	}
}
