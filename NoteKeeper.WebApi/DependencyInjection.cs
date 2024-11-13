using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config.Mapping.Actions;
using NoteKeeper.WebApi.Config.Mapping;
using Serilog;
using NoteKeeper.WebApi.Filters;

namespace NoteKeeper.WebApi;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
	{
		var connectionString = config["SQL_SERVER_CONNECTION_STRING"];

		services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
		{
			optionsBuilder.UseSqlServer(connectionString, dbOptions =>
			{
				dbOptions.EnableRetryOnFailure();
			});
		});
	}

	public static void ConfigureCoreServices(this IServiceCollection services)
	{
		services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
		services.AddScoped<ServicoCategoria>();
		services.AddScoped<IRepositorioNota, RepositorioNotaOrm>();
		services.AddScoped<ServicoNota>();
	}

	public static void ConfigureAutoMapper(this IServiceCollection services)
	{
		services.AddScoped<ConfigurarCategoriaMappingAction>();

		services.AddAutoMapper(config =>
		{
			config.AddProfile<CategoriaProfile>();
			config.AddProfile<NotaProfile>();
		});
	}

	public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder loggin, IConfiguration config)
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
				licenseKey: "RELIC_KEY"
			)
			.CreateLogger();

		loggin.ClearProviders();

		services.AddLogging(builder => builder.AddSerilog(dispose: true));
	}

	public static void ConfigureControllersWithFilter(this IServiceCollection services)
	{
		services.AddControllers(options =>
		{
			options.Filters.Add<ResponseWrapperFilter>();
		});
	}

	public static void ConfigureCors(this IServiceCollection services, string politicaCors)
	{
		services.AddCors(options =>
		{
			options.AddPolicy(name: politicaCors, policy =>
			{
				policy
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod();
			});
		});
	}
}
