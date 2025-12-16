using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Libros
{
    public interface ILibroCategoriaRepository : IGenericRepository<LibroCategoria>
    {
        Task<IEnumerable<Libro>> GetLibrosByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<Categoria>> GetCategoriasByLibroIdAsync(int libroId);
        Task<LibroCategoria?> GetByIdsAsync(int libroId, int categoriaId);
    }
}
