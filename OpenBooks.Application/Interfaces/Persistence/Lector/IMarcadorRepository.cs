using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Lector
{
    public interface IMarcadorRepository : IGenericRepository<Marcador>
    {
        Task<List<Marcador>> GetByLibroUsuarioIdAsync(int libroUsuarioId, CancellationToken ct = default);
        Task<bool> ExistsByHrefAsync(int libroUsuarioId, string href, CancellationToken ct = default);
    }
}
