using FluentValidation;

namespace NoteKeeper.Dominio.ModuloNota;

public class ValidadorNota : AbstractValidator<Nota>
{
	public ValidadorNota()
	{
		RuleFor(x => x.Titulo).NotEmpty().WithMessage("O título é obrigatório").MinimumLength(3).WithMessage("O títutlo deve contar no mínimo 3 caracteres")
			.MaximumLength(30).WithMessage("O título deve conter ao máximo 30 caracteres");

		RuleFor(x => x.Conteudo).MaximumLength(100).WithMessage("O conteúdo deve conter ao máximo 100 caracteres");
	}
}