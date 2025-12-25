using FluentValidation;
using OpenBooks.Application.Commands.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Lector
{
    public class CreateMarcadorValidator : AbstractValidator<CreateMarcadorCommand>
    {
        public CreateMarcadorValidator()
        {
            RuleFor(x => x.LibroId).GreaterThan(0).WithMessage("El identificador del libro debe ser mayor que 0");
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("El identificador del usuario debe ser mayor que 0");
            RuleFor(x => x.Locator).NotNull().WithMessage("El locator es obligatorio");
            RuleFor(x => x.Locator.Href).NotEmpty().WithMessage("El href del locator es obligatorio");
            RuleFor(x => x.Label).MaximumLength(200).When(x => x.Label != null).WithMessage("La etiqueta no puede exceder 200 caracteres");
        }
    }
}
