using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Libros
{
    public interface ICategoriaRepository : IGenericRepository<Categoria>
    {
        Task<Categoria?> GetByNombreAsync(string nombre);
        Task<IEnumerable<Categoria>> GetAllWithLibrosAsync();
    }
}
