using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Libros.Interfaces
{
    public interface ICategoriaService
    {
        Task<Result<IEnumerable<CategoriaResponseDto>>> GetAllAsync();
        Task<Result<PagedResult<CategoriaResponseDto>>> GetAllAsync(PaginationParams pagination);
        Task<Result<CategoriaResponseDto>> GetByIdAsync(int id);
        Task<Result<CategoriaResponseDto>> CreateAsync(CategoriaCreateDto dto);
        Task<Result<CategoriaResponseDto>> UpdateAsync(int id, CategoriaUpdateDto dto);
        Task<Result<CategoriaResponseDto>> PatchAsync(int id, CategoriaPatchDto dto);
        Task<Result> DeleteAsync(int id);
    }

}
