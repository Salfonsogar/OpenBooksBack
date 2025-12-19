using System;
using System.Collections.Generic;
using System.Text;
using OpenBooks.Domain.Entities.Libros;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface IEstanteriaLibroRepository : IGenericRepository<EstanteriaLibro>
    {
        Task<IEnumerable<Libro>> GetLibrosByEstanteriaIdAsync(int estanteriaId);
        Task<EstanteriaLibro?> GetByIdsAsync(int estanteriaId, int libroId);
    }
}
