using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        Task<Result<UsuarioResponseDto>> CreateAsync(UsuarioCreateDto dto);
        Task<Result<IEnumerable<UsuarioResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<UsuarioResponseDto>> GetByIdAsync(int id);
        Task<Result> DeleteAsync(int id);
        Task<Result> PatchAsync(int id, UsuarioUpdateDto dto);
        Task<Result> PatchPerfilAsync(int id, UsuarioUpdatePerfilDto dto);
    }
}
