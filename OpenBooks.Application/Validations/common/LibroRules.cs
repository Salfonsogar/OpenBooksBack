using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.common
{
    public static class LibroRules
    {
        public static IRuleBuilderOptions<T, string> Titulo<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El título es obligatorio")
                .MaximumLength(150).WithMessage("El título no puede superar los 150 caracteres");

        public static IRuleBuilderOptions<T, string> Autor<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("El autor es obligatorio")
                .MaximumLength(100).WithMessage("El autor no puede superar los 100 caracteres");

        public static IRuleBuilderOptions<T, string> Descripcion<T>(this IRuleBuilder<T, string> rule) =>
            rule.NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(1000).WithMessage("La descripción no puede superar los 1000 caracteres");

        public static IRuleBuilderOptions<T, DateTime> FechaPublicacion<T>(this IRuleBuilder<T, DateTime> rule) =>
            rule.NotEmpty().WithMessage("La fecha de publicación es obligatoria")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de publicación no puede ser futura");
    }
}
