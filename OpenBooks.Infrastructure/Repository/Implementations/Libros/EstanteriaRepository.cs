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
    public class EstanteriaRepository : GenericRepository<Estanteria>, IEstanteriaRepository
    {
        public EstanteriaRepository(OpenBooksContext context) : base(context) { }

        public async Task<Estanteria?> GetByIdWithLibrosAsync(int id)
        {
            return await _dbSet
                .Include(e => e.EstanteriaLibros)
                    .ThenInclude(el => el.Libro)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
