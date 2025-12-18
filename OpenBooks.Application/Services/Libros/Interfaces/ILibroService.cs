using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Libros.Interfaces
{
    public interface ILibroService
    {
        Task<Result<LibroDetailDto>> CreateAsync(LibroCreateDto dto);
        Task<Result> UpdateAsync(int id, LibroUpdateDto dto);
        Task<Result> PatchAsync(int id, LibroPatchDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<LibroDetailDto>> GetByIdAsync(int id);
        Task<Result<PagedResult<LibroCardDto>>> GetRecommendedAsync(PaginationParams pagination);
        Task<Result<PagedResult<LibroCardDto>>> GetTopRatedAsync(PaginationParams pagination);
        Task<Result<PagedResult<LibroCardDto>>> GetCardsAsync(PaginationParams pagination);
        Task<Result<PagedResult<LibroCardDto>>> SearchAsync(LibroSearchParams searchParams);
    }
}
