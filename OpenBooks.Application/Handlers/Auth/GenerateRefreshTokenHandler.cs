using MediatR;
using OpenBooks.Application.Commands.Auth;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.Services.Auth.Interfaces;

public class GenerateRefreshTokenHandler :
    IRequestHandler<GenerateRefreshTokenCommand, Result<RefreshTokenResponseDto>>
{
    private readonly ITokenRefreshService _service;

    public GenerateRefreshTokenHandler(ITokenRefreshService service)
    {
        _service = service;
    }

    public async Task<Result<RefreshTokenResponseDto>> Handle(
        GenerateRefreshTokenCommand request,
        CancellationToken ct)
    {
        return await _service.RefreshAsync(request.RefreshToken);
    }
}
