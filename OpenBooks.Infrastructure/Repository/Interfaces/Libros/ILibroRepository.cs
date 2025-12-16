using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Libros
{
    public interface ILibroRepository : IGenericRepository<Libro>
    {
        Task<Libro?> GetLibroCompletoAsync(int id);
        Task<Libro?> GetByIdWithCategoriasAsync(int id);
    }
}
