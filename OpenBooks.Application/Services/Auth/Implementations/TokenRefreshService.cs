using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.Services.Auth.Interfaces;
using OpenBooks.Domain.Entities.Auth;
using OpenBooks.Infrastructure.Services.Auth.Interfaces;
using OpenBooksBack.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Auth.Implementations
{
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly IUnitOfWork _unit;
        private readonly IJwtService _jwt;

        public TokenRefreshService(
            IUnitOfWork unit,
            IJwtService jwt)
        {
            _unit = unit;
            _jwt = jwt;
        }

        public async Task<Result<RefreshTokenResponseDto>> RefreshAsync(string token)
        {
            var existing = await _unit.RefreshTokens.GetByTokenAsync(token);
            if (existing == null)
                return Result<RefreshTokenResponseDto>.Failure("Refresh token inválido.");

            var validation = Validate(existing);
            if (!validation.IsSuccess)
                return Result<RefreshTokenResponseDto>.Failure(validation.Error!);

            existing.EstaRevocado = true;
            _unit.RefreshTokens.Update(existing);

            var accessToken = _jwt.GenerateAccessToken(
                existing.UsuarioId,
                existing.Usuario.Correo
            );

            var newRefreshToken = CreateRefreshToken(existing.UsuarioId);
            await _unit.RefreshTokens.AddAsync(newRefreshToken);

            await _unit.CommitAsync();

            return Result<RefreshTokenResponseDto>.Success(
                new RefreshTokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken.Token
                }
            );
        }
        private RefreshToken CreateRefreshToken(int usuarioId)
        {
            return new RefreshToken
            {
                Token = _jwt.GenerateRefreshToken(),
                UsuarioId = usuarioId,
                Creado = DateTime.UtcNow,
                Expira = DateTime.UtcNow.AddDays(7),
                EstaRevocado = false
            };
        }

        private Result Validate(RefreshToken token)
        {
            if (token.EstaRevocado || token.Expira < DateTime.UtcNow)
                return Result.Failure("Refresh token inválido o expirado.");

            return Result.Success();
        }

    }

}
