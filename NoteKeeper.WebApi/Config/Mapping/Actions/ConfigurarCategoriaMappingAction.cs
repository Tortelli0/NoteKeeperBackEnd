using AutoMapper;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Config.Mapping.Actions;

public class ConfigurarCategoriaMappingAction(IRepositorioCategoria repositorioCategoria)
	: IMappingAction<FormsNotaViewModel, Nota>
{
	public void Process(FormsNotaViewModel source, Nota destination, ResolutionContext context)
	{
		var idCategoria = source.CategoriaId;

		destination.Categoria = repositorioCategoria.SelecionarPorId(idCategoria);
	}
}