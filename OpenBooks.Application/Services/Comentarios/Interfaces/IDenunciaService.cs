using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;

namespace OpenBooks.Application.Services.Comentarios.Interfaces
{
    public interface IDenunciaService
    {
        Task<Result<DenunciaResponseDto>> CreateAsync(DenunciaCreateDto dto, int usuarioDenuncianteId);
        Task<Result> DeleteAsync(int id, int usuarioSolicitanteId);
        Task<Result<DenunciaResponseDto>> GetByIdAsync(int id);
        Task<Result<PagedResult<DenunciaResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<IEnumerable<DenunciaResponseDto>>> GetByUsuarioDenunciadoAsync(int usuarioId);
    }
}
