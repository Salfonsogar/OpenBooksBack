using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Libros
{
    public interface IBibliotecaLibroRepository : IGenericRepository<BibliotecaLibro>
    {
        Task<IEnumerable<Libro>> GetLibrosByBibliotecaIdAsync(int bibliotecaId);
        Task<BibliotecaLibro?> GetByIdsAsync(int bibliotecaId, int libroId);
    }
}
