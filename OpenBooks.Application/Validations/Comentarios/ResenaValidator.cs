using FluentValidation;
using OpenBooks.Application.DTOs.Comentarios;

namespace OpenBooks.Application.Validations.Comentarios
{
    public class ResenaCreateValidator : AbstractValidator<ResenaCreateDto>
    {
        public ResenaCreateValidator()
        {
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("UsuarioId inválido");
            RuleFor(x => x.LibroId).GreaterThan(0).WithMessage("LibroId inválido");
            RuleFor(x => x.Puntuacion).InclusiveBetween(1, 5).WithMessage("La puntuación debe estar entre 1 y 5");
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }

    public class ResenaUpdateValidator : AbstractValidator<ResenaUpdateDto>
    {
        public ResenaUpdateValidator()
        {
            RuleFor(x => x.Puntuacion).InclusiveBetween(1, 5).WithMessage("La puntuación debe estar entre 1 y 5");
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres");
        }
    }
}
