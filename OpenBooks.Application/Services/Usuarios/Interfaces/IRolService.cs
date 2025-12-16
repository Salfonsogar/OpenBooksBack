using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;

namespace OpenBooks.Application.Services.Usuarios.Interfaces
{
    public interface IRolService
    {
        Task<Result<RolResponseDto>> CreateAsync(RolCreateDto dto);
        Task<Result<PagedResult<RolResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<RolResponseDto>> GetByIdAsync(int id);
        Task<Result<RolResponseDto>> PatchAsync(int id, RolUpdateDto dto);
        Task<Result> DeleteAsync(int id);
    }
}
