using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Comentarios;
using OpenBooks.Domain.Entities.Comentarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Comentarios
{
    public class DenunciaRepository : GenericRepository<Denuncia>, IDenunciaRepository
    {
        public DenunciaRepository(OpenBooksContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Denuncia>> GetDenunciasRealizadasPorUsuario(int usuarioId)
        {
            return await _dbSet
                .Where(d => d.UsuarioDenuncianteId == usuarioId)
                .Include(d => d.UsuarioDenunciadoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Denuncia>> GetDenunciasRecibidasPorUsuario(int usuarioId)
        {
            return await _dbSet
                .Where(d => d.UsuarioDenunciadoId == usuarioId)
                .Include(d => d.UsuarioDenuncianteId)
                .ToListAsync();
        }
    }
}
