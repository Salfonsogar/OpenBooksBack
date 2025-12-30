using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

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
