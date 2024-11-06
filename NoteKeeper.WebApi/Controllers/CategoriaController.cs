using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/categorias")]
[ApiController]
public class CategoriaController : ControllerBase
{
	private readonly ServicoCategoria servicoCategoria;
	private readonly IMapper mapeador;

	public CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador)
	{
		this.servicoCategoria = servicoCategoria;
		this.mapeador = mapeador;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var resultado = await servicoCategoria.SelecionarTodosAsync();

		if (resultado.IsFailed)
			return StatusCode(500);

		var viewModel = mapeador.Map<ListarCategoriaViewModel[]>(resultado.Value);
		
		return Ok(viewModel);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var categoriaResult = await servicoCategoria.SelecionarPorIdAsync(id);

		if (categoriaResult.IsFailed)
			return StatusCode(500);

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
		var notaResult = await servicoCategoria.SelecionarPorIdAsync(id);

		if (notaResult.IsFailed)
			return StatusCode(500);

		if (notaResult.IsSuccess && notaResult.Value is null)
			return NotFound(notaResult.Errors);

		var notaEditada = mapeador.Map(notaVm, notaResult.Value);

		var edicaoResult = await servicoCategoria.EditarAsync(notaEditada);

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
