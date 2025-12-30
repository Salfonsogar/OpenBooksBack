using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Comentarios.Interfaces
{
    public interface ISugerenciaService
    {
        Task<Result<SugerenciaResponseDto>> CreateAsync(SugerenciaCreateDto dto);
        Task<Result> DeleteAsync(int id, int usuarioSolicitanteId);
        Task<Result<SugerenciaResponseDto>> GetByIdAsync(int id);
        Task<Result<PagedResult<SugerenciaResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<IEnumerable<SugerenciaResponseDto>>> GetByUsuarioIdAsync(int usuarioId);
    }
}
