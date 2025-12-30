using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Domain.Entities.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

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
