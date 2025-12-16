using FluentValidation;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.Validations.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Auth
{
    public class UsuarioRegisterValidator : AbstractValidator<UsuarioRegisterDto>
    {
        public UsuarioRegisterValidator()
        {
            RuleFor(x => x.NombreCompleto).NombreCompleto();
            RuleFor(x => x.NombreUsuario).NombreUsuario();
            RuleFor(x => x.Correo).Correo();
            RuleFor(x => x.Contrasena).Contrasena();
        }
    }
}
