
using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config.Mapping;
using NoteKeeper.WebApi.Config.Mapping.Actions;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		// Configuração de Injeção de Dependência
		var builder = WebApplication.CreateBuilder(args);

		var connectionString = builder.Configuration.GetConnectionString("SqlServer");

		builder.Services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
		{
			optionsBuilder.UseSqlServer(connectionString);
		});

		builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
		builder.Services.AddScoped<ServicoCategoria>();

		builder.Services.AddScoped<IRepositorioNota, RepositorioNotaOrm>();
		builder.Services.AddScoped<ServicoNota>();
		
		builder.Services.AddScoped<ConfigurarCategoriaMappingAction>();

		builder.Services.AddAutoMapper(config =>
		{
			config.AddProfile<CategoriaProfile>();
			config.AddProfile<NotaProfile>();
		});

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		// Middlewares de execução da API
		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
