using FluentValidation;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Validations.common;
using System;

namespace OpenBooks.Application.Validations.Usuarios
{
    public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateDto>
    {
        public UsuarioCreateValidator()
        {
            RuleFor(x => x.NombreCompleto).NombreCompleto();
            RuleFor(x => x.NombreUsuario).NombreUsuario();
            RuleFor(x => x.Correo).Correo();
            RuleFor(x => x.Contrasena).Contrasena();
            RuleFor(x => x.RolId)
                .NotNull().WithMessage("El rol es obligatorio")
                .GreaterThan(0).WithMessage("El rol seleccionado no es válido");
            RuleFor(x => x.Telefono).Telefono();
            RuleFor(x => x.Genero).Genero();
            RuleFor(x => x.FechaNacimiento).FechaNacimiento();
        }
    }

    public class UsuarioUpdateValidator : AbstractValidator<UsuarioUpdateDto>
    {
        public UsuarioUpdateValidator()
        {
            RuleFor(x => x.NombreCompleto)
                .NombreCompleto();

            RuleFor(x => x.NombreUsuario)
                .NombreUsuario();

            RuleFor(x => x.Correo)
                .Correo();

            RuleFor(x => x.Contrasena)
                .Contrasena();

            RuleFor(x => x.FechaNacimiento)
                .FechaNacimientoNullable();

            RuleFor(x => x.Genero)
                .Genero();

            RuleFor(x => x.Telefono)
                .Telefono();

            RuleFor(x => x.RolId)
                .GreaterThan(0)
                .When(x => x.RolId.HasValue)
                .WithMessage("El rol no es válido");
        }
    }

    public class UsuarioUpdatePerfilValidator : AbstractValidator<UsuarioUpdatePerfilDto>
    {
        public UsuarioUpdatePerfilValidator()
        {
            RuleFor(x => x.NombreCompleto)
                .NombreCompleto();

            RuleFor(x => x.NombreUsuario)
                .NombreUsuario();

            RuleFor(x => x.Genero)
                .Genero();

            RuleFor(x => x.Telefono)
                .Telefono();
        }
    }
}
