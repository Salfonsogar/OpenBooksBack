using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<Result<UsuarioResponseDto>> RegisterAsync(UsuarioRegisterDto dto);
    }
}
