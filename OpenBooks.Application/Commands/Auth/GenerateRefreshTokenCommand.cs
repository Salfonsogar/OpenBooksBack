using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;

namespace OpenBooks.Application.Commands.Auth
{
    public record GenerateRefreshTokenCommand(string RefreshToken)
        : IRequest<Result<RefreshTokenResponseDto>>;
}
