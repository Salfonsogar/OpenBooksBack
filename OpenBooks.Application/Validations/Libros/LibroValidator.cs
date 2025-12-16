using FluentValidation;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Validations.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Libros
{
    public class LibroCreateValidator : AbstractValidator<LibroCreateDto>
    {
        public LibroCreateValidator()
        {
            RuleFor(x => x.Titulo).Titulo();
            RuleFor(x => x.Autor).Autor();
            RuleFor(x => x.Descripcion).Descripcion();
            RuleFor(x => x.FechaPublicacion).FechaPublicacion();

            RuleFor(x => x.CategoriasIds)
                .NotNull().WithMessage("Debe especificar al menos una categoría")
                .Must(c => c.Count > 0).WithMessage("Debe seleccionar al menos una categoría");

            RuleForEach(x => x.CategoriasIds)
                .GreaterThan(0).WithMessage("El id de la categoría no es válido");

            RuleFor(x => x.Archivo)
                .NotNull().WithMessage("El archivo del libro es obligatorio");
        }
    }
    public class LibroUpdateValidator : AbstractValidator<LibroUpdateDto>
    {
        public LibroUpdateValidator()
        {
            RuleFor(x => x.Titulo).Titulo();
            RuleFor(x => x.Autor).Autor();
            RuleFor(x => x.Descripcion).Descripcion();
            RuleFor(x => x.FechaPublicacion).FechaPublicacion();

            RuleFor(x => x.CategoriasIds)
                .NotNull().WithMessage("Debe especificar al menos una categoría")
                .Must(c => c.Count > 0).WithMessage("Debe seleccionar al menos una categoría");

            RuleForEach(x => x.CategoriasIds)
                .GreaterThan(0).WithMessage("El id de la categoría no es válido");
        }
    }
    public class LibroPatchValidator : AbstractValidator<LibroPatchDto>
    {
        public LibroPatchValidator()
        {
            When(x => x.Titulo != null, () =>
            {
                RuleFor(x => x.Titulo!)
                    .NotEmpty().WithMessage("El título no puede estar vacío")
                    .MaximumLength(150);
            });

            When(x => x.Autor != null, () =>
            {
                RuleFor(x => x.Autor!)
                    .NotEmpty().WithMessage("El autor no puede estar vacío")
                    .MaximumLength(100);
            });

            When(x => x.Descripcion != null, () =>
            {
                RuleFor(x => x.Descripcion!)
                    .NotEmpty().WithMessage("La descripción no puede estar vacía")
                    .MaximumLength(1000);
            });

            When(x => x.FechaPublicacion.HasValue, () =>
            {
                RuleFor(x => x.FechaPublicacion!.Value)
                    .LessThanOrEqualTo(DateTime.Today)
                    .WithMessage("La fecha de publicación no puede ser futura");
            });

            When(x => x.CategoriasIds != null, () =>
            {
                RuleForEach(x => x.CategoriasIds!)
                    .GreaterThan(0)
                    .WithMessage("El id de la categoría no es válido");
            });
        }
    }
}
