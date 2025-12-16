using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Infrastructure.Repository.Interfaces.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<Categoria?> GetByNombreAsync(string nombre)
        {
            return await Query(c => c.Nombre.ToLower() == nombre.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Categoria>> GetAllWithLibrosAsync()
        {
            return await Query(
                    null,
                    c => c.LibroCategorias
                )
                .ToListAsync();
        }
    }
}
