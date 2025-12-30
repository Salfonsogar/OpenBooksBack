using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class LibroCategoriaRepository : GenericRepository<LibroCategoria>, ILibroCategoriaRepository
    {
        public LibroCategoriaRepository(OpenBooksContext context) : base(context) { }

        public async Task<IEnumerable<Libro>> GetLibrosByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Libros
                .Where(l => l.LibroCategorias.Any(lc => lc.CategoriaId == categoriaId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasByLibroIdAsync(int libroId)
        {
            return await _context.Categorias
                .Where(c => c.LibroCategorias.Any(lc => lc.LibroId == libroId))
                .ToListAsync();
        }

        public async Task<LibroCategoria?> GetByIdsAsync(int libroId, int categoriaId)
        {
            return await _dbSet.FindAsync(libroId, categoriaId);
        }
    }
}
