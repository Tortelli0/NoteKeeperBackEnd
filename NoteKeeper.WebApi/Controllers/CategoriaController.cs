using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.WebApi.ViewModels;
using Serilog;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/categorias")]
[ApiController]
public class CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var resultado = await servicoCategoria.SelecionarTodosAsync();

		var viewModel = mapeador.Map<ListarCategoriaViewModel[]>(resultado.Value);

		Log.Information("Foram selecionados {QuantidadeRegistros}", viewModel.Count());
		
		return Ok(viewModel);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var categoriaResult = await servicoCategoria.SelecionarPorIdAsync(id);

		if (categoriaResult.IsSuccess && categoriaResult.Value is null)
			return NotFound(categoriaResult.Errors);

		var viewModel = mapeador.Map<VisualizarCategoriaViewModel>(categoriaResult.Value);

		return Ok(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Post(InserirCategoriaViewModel categoriaVm)
	{
		var categoria = mapeador.Map<Categoria>(categoriaVm);

		var resultado = await servicoCategoria.InserirAsync(categoria);

		if (resultado.IsFailed)
			return BadRequest(resultado.Errors);

		return Ok(categoriaVm);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(Guid id,EditarCategoriaViewModel notaVm)
	{
		var categoriaResult = await servicoCategoria.SelecionarPorIdAsync(id);

		var categoriaEditada = mapeador.Map(notaVm, categoriaResult.Value);

		var edicaoResult = await servicoCategoria.EditarAsync(categoriaEditada);

		if (edicaoResult.IsFailed)
			return BadRequest(edicaoResult.Errors);

		return Ok(notaVm);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var notaResult = await servicoCategoria.ExcluirAsync(id);

		if (notaResult.IsFailed)
			return NotFound(notaResult.Errors);

		return Ok();
	}
}
