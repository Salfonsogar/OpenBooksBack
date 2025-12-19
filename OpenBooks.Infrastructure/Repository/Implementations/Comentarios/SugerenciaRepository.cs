using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Comentarios;
using OpenBooks.Domain.Entities.Comentarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Comentarios
{
    public class SugerenciaRepository : GenericRepository<Sugerencia>, ISugerenciaRepository
    {
        public SugerenciaRepository(OpenBooksContext context) : base(context) { }

        public async Task<IEnumerable<Sugerencia>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
