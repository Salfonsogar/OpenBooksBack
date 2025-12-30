using FluentValidation;
using OpenBooks.Application.Commands.Lector;

namespace OpenBooks.Application.Validations.Lector
{
    public class GetBookManifestValidator : AbstractValidator<GetBookManifestCommand>
    {
        public GetBookManifestValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("El identificador del libro debe ser mayor que 0");
        }
    }

    public class GetBookResourceValidator : AbstractValidator<GetBookResourceCommand>
    {
        public GetBookResourceValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("El identificador del libro debe ser mayor que 0");

            RuleFor(x => x.ResourcePath)
                .NotEmpty().WithMessage("Se debe especificar la ruta del recurso");
        }
    }
}