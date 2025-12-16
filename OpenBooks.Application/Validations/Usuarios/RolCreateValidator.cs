using OpenBooks.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace OpenBooks.Application.Validations.Usuarios
{
    public class RolCreateValidator : AbstractValidator<RolCreateDto>
    {
        public RolCreateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MinimumLength(3);
        }
    }

    public class RolUpdateValidator : AbstractValidator<RolUpdateDto>
    {
        public RolUpdateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
