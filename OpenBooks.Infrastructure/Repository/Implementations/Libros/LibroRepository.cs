using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Infrastructure.Repository.Interfaces.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class LibroRepository : GenericRepository<Libro>, ILibroRepository
    {
        public LibroRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<Libro?> GetByIdWithCategoriasAsync(int id)
        {
            return await _context.Libros
                .Include(l => l.LibroCategorias)
                    .ThenInclude(lc => lc.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Libro?> GetLibroCompletoAsync(int id)
        {
            return await Query(
                l => l.Id == id,
                l => l.LibroCategorias,
                l => l.Resenas,
                l => l.BibliotecaLibros,
                l => l.EstanteriaLibros
            )
            .FirstOrDefaultAsync();
        }
    }
}
