using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface IBibliotecaLibroRepository : IGenericRepository<BibliotecaLibro>
    {
        Task<IEnumerable<Libro>> GetLibrosByBibliotecaIdAsync(int bibliotecaId);
        Task<BibliotecaLibro?> GetByIdsAsync(int bibliotecaId, int libroId);
    }
}
