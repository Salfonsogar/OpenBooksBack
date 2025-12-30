using FluentValidation;
using OpenBooks.Application.DTOs.Usuarios;

namespace OpenBooks.Application.Validations.Usuarios
{
    public class SancionCreateValidator : AbstractValidator<SancionCreateDto>
    {
        public SancionCreateValidator()
        {
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("UsuarioId inválido");
            RuleFor(x => x.DuracionDias).GreaterThan(0).WithMessage("La duración debe ser mayor a 0");
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }

    public class SancionUpdateValidator : AbstractValidator<SancionUpdateDto>
    {
        public SancionUpdateValidator()
        {
            RuleFor(x => x.DuracionDias).GreaterThan(0).WithMessage("La duración debe ser mayor a 0");
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }
}
