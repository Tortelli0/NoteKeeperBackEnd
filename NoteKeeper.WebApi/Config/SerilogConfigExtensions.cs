using Serilog;

namespace NoteKeeper.WebApi.Config;

public static class SerilogConfigExtensions
{
    public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder loggin)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithClientIp()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.NewRelicLogs(
                endpointUrl: "https://log-api.newrelic.com/log/v1",
                applicationName: "note-keeper-api",
                licenseKey: "01f7d3606465626868e6e2eae298d3c2FFFFNRAL"
            )
            .CreateLogger();

        loggin.ClearProviders();

        services.AddLogging(builder => builder.AddSerilog(dispose: true));
    }
}
