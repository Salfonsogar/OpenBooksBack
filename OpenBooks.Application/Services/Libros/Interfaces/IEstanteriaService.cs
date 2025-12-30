using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Application.Services.Libros.Interfaces
{
    public interface IEstanteriaService
    {
        Task<Result<EstanteriaDto>> CreateAsync(int usuarioId, EstanteriaCreateDto dto);
        Task<Result<EstanteriaDto>> GetByIdAsync(int id);
        Task<Result<IEnumerable<EstanteriaDto>>> GetByUsuarioIdAsync(int usuarioId);
        Task<Result> UpdateAsync(int usuarioId, int id, EstanteriaUpdateDto dto);
        Task<Result> DeleteAsync(int usuarioId, int id);
        Task<Result> AddLibroAsync(int usuarioId, int estanteriaId, int libroId);
        Task<Result> RemoveLibroAsync(int usuarioId, int estanteriaId, int libroId);
    }
}
