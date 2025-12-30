using FluentValidation;
using OpenBooks.Application.DTOs.Comentarios;

namespace OpenBooks.Application.Validations.Comentarios
{
    public class SugerenciaCreateValidator : AbstractValidator<SugerenciaCreateDto>
    {
        public SugerenciaCreateValidator()
        {
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("UsuarioId inválido");
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }
}
