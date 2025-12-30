using FluentValidation;
using OpenBooks.Application.DTOs.Comentarios;

namespace OpenBooks.Application.Validations.Comentarios
{
    public class DenunciaCreateValidator : AbstractValidator<DenunciaCreateDto>
    {
        public DenunciaCreateValidator()
        {
            RuleFor(x => x.UsuarioDenunciadoId).GreaterThan(0).WithMessage("Usuario denunciado inválido");
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }
}
