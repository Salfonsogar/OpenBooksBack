using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Comentarios.Interfaces
{
    public interface IResenaService
    {
        Task<Result<ResenaResponseDto>> CreateAsync(ResenaCreateDto dto);
        Task<Result> UpdateAsync(int id, ResenaUpdateDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<ResenaResponseDto>> GetByIdAsync(int id);
        Task<Result<PagedResult<ResenaResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<IEnumerable<ResenaResponseDto>>> GetByLibroIdAsync(int libroId);
        Task<Result<ResenaResponseDto>> GetByUsuarioAndLibroAsync(int usuarioId, int libroId);
    }
}
