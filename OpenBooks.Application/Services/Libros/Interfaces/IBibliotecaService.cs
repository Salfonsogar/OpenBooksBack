using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;

namespace OpenBooks.Application.Services.Libros.Interfaces
{
    public interface IBibliotecaService
    {
        Task<Result<BibliotecaDto>> GetByUsuarioIdAsync(int usuarioId);
        Task<Result> AddLibroAsync(int usuarioId, int libroId);
        Task<Result> RemoveLibroAsync(int usuarioId, int libroId);
        Task<Result> DeleteBibliotecaAsync(int usuarioId);
    }
}
