using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Lector
{
    public interface IResaltadoRepository : IGenericRepository<Resaltado>
    {
        Task<List<Resaltado>> GetByLibroUsuarioIdAsync(int libroUsuarioId, CancellationToken ct = default);
        Task<bool> ExistsByRangeAsync(int libroUsuarioId, string locatorStart, string locatorEnd, CancellationToken ct = default);
    }
}
