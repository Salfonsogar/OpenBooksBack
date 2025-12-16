using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.common
{
    public static class UsuarioRules
    {
        public static IRuleBuilderOptions<T, string> NombreCompleto<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El nombre completo es obligatorio")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres")
                .MaximumLength(80).WithMessage("El nombre no puede superar los 80 caracteres");

        public static IRuleBuilderOptions<T, string> NombreUsuario<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El nombre de usuario es obligatorio")
                .MinimumLength(4).WithMessage("El nombre de usuario debe tener al menos 4 caracteres")
                .MaximumLength(30).WithMessage("El nombre de usuario no puede superar los 30 caracteres")
                .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("El nombre de usuario solo puede contener letras, números, puntos y guiones bajos");

        public static IRuleBuilderOptions<T, string> Contrasena<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
                .MaximumLength(100).WithMessage("La contraseña es demasiado larga")
                .Matches(@"[A-Z]+").WithMessage("La contraseña debe contener al menos una letra mayúscula")
                .Matches(@"[a-z]+").WithMessage("La contraseña debe contener al menos una letra minúscula")
                .Matches(@"\d+").WithMessage("La contraseña debe contener al menos un número")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]+").WithMessage("La contraseña debe contener al menos un carácter especial (!@#$%^&*()-+=)")
                .Matches(@"^\S+$").WithMessage("La contraseña no debe contener espacios");

        public static IRuleBuilderOptions<T, string> Correo<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

        public static IRuleBuilderOptions<T, string> Telefono<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El teléfono es obligatorio")
                .MaximumLength(15).WithMessage("El teléfono no puede superar los 15 caracteres")
                .Matches(@"^[0-9+\-\s]+$").WithMessage("El teléfono solo puede contener números y símbolos válidos");

        public static IRuleBuilderOptions<T, string> Genero<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El género es obligatorio")
                .MaximumLength(20).WithMessage("El género no puede superar los 20 caracteres");

        public static IRuleBuilderOptions<T, DateTime> FechaNacimiento<T>(this IRuleBuilder<T, DateTime> rule) =>
            rule.NotNull().WithMessage("La fecha de nacimiento es obligatoria")
                .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual");
        public static IRuleBuilderOptions<T, DateTime?> FechaNacimientoNullable<T>(this IRuleBuilder<T, DateTime?> rule) =>
            rule.Must(date => !date.HasValue || date.Value < DateTime.Today)
                .WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual");
    }
}
