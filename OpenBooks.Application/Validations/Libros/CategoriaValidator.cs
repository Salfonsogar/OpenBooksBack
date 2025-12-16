using FluentValidation;
using OpenBooks.Application.DTOs.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Libros
{
    public class CategoriaCreateValidator
        : AbstractValidator<CategoriaCreateDto>
    {
        public CategoriaCreateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");
        }
    }
    public class CategoriaUpdateValidator
        : AbstractValidator<CategoriaUpdateDto>
    {
        public CategoriaUpdateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");
        }
    }
    public class CategoriaPatchValidator : AbstractValidator<CategoriaPatchDto>
    {
        public CategoriaPatchValidator()
        {
            RuleFor(x => x.Nombre)
                .MaximumLength(100)
                .When(x => x.Nombre != null)
                .WithMessage("El nombre no puede superar los 100 caracteres");
        }
    }
}
