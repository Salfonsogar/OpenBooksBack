using FluentValidation;
using OpenBooks.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }
}
