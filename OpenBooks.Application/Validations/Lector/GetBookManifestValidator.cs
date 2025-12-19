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
}