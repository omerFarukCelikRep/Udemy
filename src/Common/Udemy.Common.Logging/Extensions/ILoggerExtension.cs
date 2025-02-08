using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Udemy.Common.Logging.Extensions;

public static class ILoggerExtension
{
    private const string Path = "../Logs/Logs.txt";

    public static void UseSerilog(this IHostBuilder hostBuilder, params string[] headers) => hostBuilder.UseSerilog((context, loggerConfiguration) =>
                                                                         {
                                                                             loggerConfiguration.MinimumLevel.Debug();
                                                                             loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                                                                             loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error);
                                                                             loggerConfiguration.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
                                                                             loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Debug);

                                                                             loggerConfiguration.WriteTo.Console(formatter: new CompactJsonFormatter(), restrictedToMinimumLevel: LogEventLevel.Debug);

                                                                             loggerConfiguration.WriteTo.File(path: Path,
                                                                                                              formatter: new CompactJsonFormatter(),
                                                                                                              restrictedToMinimumLevel: LogEventLevel.Debug,
                                                                                                              rollingInterval: RollingInterval.Day,
                                                                                                              rollOnFileSizeLimit: true);

                                                                             loggerConfiguration.Enrich.WithMachineName();
                                                                             loggerConfiguration.Enrich.WithProcessId();
                                                                             loggerConfiguration.Enrich.WithThreadId();
                                                                             loggerConfiguration.Enrich.WithClientIp();
                                                                             loggerConfiguration.Enrich.WithCorrelationId();
                                                                             foreach (string header in headers)
                                                                             {
                                                                                 loggerConfiguration.Enrich.WithRequestHeader(header);
                                                                             }
                                                                         });
}
