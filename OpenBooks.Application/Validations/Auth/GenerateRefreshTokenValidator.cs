using FluentValidation;
using OpenBooks.Application.Commands.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Validations.Auth
{
    public class GenerateRefreshTokenValidator : AbstractValidator<GenerateRefreshTokenCommand>
    {
        public GenerateRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio.");
        }
    }
}
