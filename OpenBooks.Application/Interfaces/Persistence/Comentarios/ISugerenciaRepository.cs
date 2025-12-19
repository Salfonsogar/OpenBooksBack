using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Application.Interfaces.Persistence.Comentarios
{
    public interface ISugerenciaRepository : IGenericRepository<Sugerencia>
    {
        Task<IEnumerable<Sugerencia>> GetByUsuarioIdAsync(int usuarioId);
    }
}
