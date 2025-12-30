using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Usuarios.Interfaces
{
    public interface ISancionService
    {
        Task<Result<SancionResponseDto>> CreateAsync(SancionCreateDto dto);
        Task<Result> UpdateAsync(int id, SancionUpdateDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<SancionResponseDto>> GetByIdAsync(int id);
        Task<Result<PagedResult<SancionResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<IEnumerable<SancionResponseDto>>> GetByUsuarioIdAsync(int usuarioId);
    }
}
