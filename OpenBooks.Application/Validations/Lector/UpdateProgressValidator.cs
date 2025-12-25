using FluentValidation;
using OpenBooks.Application.Commands.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Lector
{
    public class UpdateProgressValidator : AbstractValidator<UpdateProgressCommand>
    {
        public UpdateProgressValidator()
        {
            RuleFor(x => x.LibroId).GreaterThan(0).WithMessage("El identificador del libro debe ser mayor que 0");
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("El identificador del usuario debe ser mayor que 0");
            RuleFor(x => x.CurrentLocator).NotNull().WithMessage("El locator actual es obligatorio");
            RuleFor(x => x.CurrentLocator.Href).NotEmpty().WithMessage("El campo href del locator es obligatorio");
            RuleFor(x => x.ClientTimestamp).NotEmpty().WithMessage("El timestamp del cliente es obligatorio");
        }
    }
}
