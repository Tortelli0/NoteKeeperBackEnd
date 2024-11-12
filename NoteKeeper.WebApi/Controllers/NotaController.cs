using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/notas")]
[ApiController]
public class NotaController(ServicoNota servicoNota, IMapper mapeador) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var notasResult = await servicoNota.SelecionarTodosAsync();

		var viewModel = mapeador.Map<ListarNotaViewModel[]>(notasResult.Value);

		return Ok(viewModel);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var notaResult = await servicoNota.SelecionarPorIdAsync(id);

		if (notaResult.IsSuccess && notaResult.Value is null)
			return NotFound(notaResult.Errors);

		var viewModel = mapeador.Map<VisualizarNotaViewModel>(notaResult.Value);

		return Ok(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Post(InserirNotaViewModel notaVm)
	{
		var nota = mapeador.Map<Nota>(notaVm);

		var notaResult = await servicoNota.InserirAsync(nota);

		if (notaResult.IsFailed)
			return BadRequest(notaResult.Errors);

		return Ok(notaVm);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(Guid id, EditarNotaViewModel notaVm)
	{
		var selecaoNotaOriginal = await servicoNota.SelecionarPorIdAsync(id);

		if (selecaoNotaOriginal.IsFailed)
			return NotFound(selecaoNotaOriginal.Errors);

		var notaEditada = mapeador.Map(notaVm, selecaoNotaOriginal.Value);

		var edicaoResult = await servicoNota.EditarAsync(notaEditada);

		if (edicaoResult.IsFailed)
			return BadRequest(edicaoResult.Errors);

		return Ok(notaVm);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var notaResult = await servicoNota.ExcluirAsync(id);

		if (notaResult.IsFailed)
			return NotFound(notaResult.Errors);

		return Ok();
	}
}
