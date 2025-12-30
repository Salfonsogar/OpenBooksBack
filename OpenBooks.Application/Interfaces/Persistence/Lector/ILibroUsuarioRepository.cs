using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Lector
{
    public interface ILibroUsuarioRepository : IGenericRepository<LibroUsuario>
    {
        Task<LibroUsuario?> GetByUsuarioAndLibroAsync(int usuarioId, int libroId, CancellationToken ct = default);
        Task<LibroUsuario?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsAsync(int usuarioId, int libroId, CancellationToken ct = default);
    }
}
