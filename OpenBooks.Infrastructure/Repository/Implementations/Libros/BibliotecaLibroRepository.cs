using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class BibliotecaLibroRepository : GenericRepository<BibliotecaLibro>, IBibliotecaLibroRepository
    {
        public BibliotecaLibroRepository(OpenBooksContext context) : base(context) { }

        public async Task<IEnumerable<Libro>> GetLibrosByBibliotecaIdAsync(int bibliotecaId)
        {
            return await _context.Libros
                .Where(l => l.BibliotecaLibros.Any(bl => bl.BibliotecaId == bibliotecaId))
                .ToListAsync();
        }

        public async Task<BibliotecaLibro?> GetByIdsAsync(int bibliotecaId, int libroId)
        {
            return await _dbSet.FindAsync(bibliotecaId, libroId);
        }
    }
}
