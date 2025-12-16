using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Auth.Interfaces
{
    public interface ITokenRefreshService
    {
        Task<Result<RefreshTokenResponseDto>> RefreshAsync(string refreshToken);
    }

}
