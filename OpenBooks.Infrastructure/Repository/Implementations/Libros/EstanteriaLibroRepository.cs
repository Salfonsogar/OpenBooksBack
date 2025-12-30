using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class EstanteriaLibroRepository : GenericRepository<EstanteriaLibro>, IEstanteriaLibroRepository
    {
        public EstanteriaLibroRepository(OpenBooksContext context) : base(context) { }

        public async Task<IEnumerable<Libro>> GetLibrosByEstanteriaIdAsync(int estanteriaId)
        {
            return await _context.Libros
                .Where(l => l.EstanteriaLibros.Any(el => el.EstanteriaId == estanteriaId))
                .ToListAsync();
        }

        public async Task<EstanteriaLibro?> GetByIdsAsync(int estanteriaId, int libroId)
        {
            return await _dbSet.FindAsync(estanteriaId, libroId);
        }
    }
}
