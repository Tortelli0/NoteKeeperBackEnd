using NoteKeeper.WebApi.Config;
using Serilog;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		const string politicaCors = "_minhaPoliticaCors";

		// Configura��o de Inje��o de Depend�ncia
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.ConfigureDbContext(builder.Configuration);

		builder.Services.ConfigureCoreServices();

		builder.Services.ConfigureAutoMapper();

		builder.Services.ConfigureSerilog(builder.Logging, builder.Configuration);

		builder.Services.ConfigureControllersWithFilter();

		builder.Services.ConfigureCors(politicaCors);

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		app.UseGlobalExceptionHandler();

		app.UseSwagger();
		app.UseSwaggerUI();

		var migracaoConcluida = app.AutoMigrateDatabase();

		if (migracaoConcluida) Log.Information("Migra��o do banco de dados conclu�da");

		else Log.Information("Nenhuma migra��o de banco de dados pendente");

		app.UseHttpsRedirection();

		app.UseCors(politicaCors);

		app.UseAuthorization();

		app.MapControllers();

		try
		{
			app.Run();
		}
		catch (Exception ex) 
		{
			Log.Fatal("Ocorreu um erro que ocasionou no fechamento da aplica��o", ex);

			return;
		}
	}
}
