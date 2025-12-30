using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class BibliotecaRepository : GenericRepository<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(OpenBooksContext context) : base(context) { }

        public async Task<Biblioteca?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(b => b.Estanterias)
                    .ThenInclude(e => e.EstanteriaLibros)
                        .ThenInclude(el => el.Libro)
                .Include(b => b.BibliotecaLibros)
                    .ThenInclude(bl => bl.Libro)
                .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);
        }
    }
}
