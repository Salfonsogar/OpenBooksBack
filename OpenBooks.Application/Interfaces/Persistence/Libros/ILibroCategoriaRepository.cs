using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface ILibroCategoriaRepository : IGenericRepository<LibroCategoria>
    {
        Task<IEnumerable<Libro>> GetLibrosByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<Categoria>> GetCategoriasByLibroIdAsync(int libroId);
        Task<LibroCategoria?> GetByIdsAsync(int libroId, int categoriaId);
    }
}
