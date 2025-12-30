using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Application.Interfaces.Persistence.Comentarios
{
    public interface IResenaRepository : IGenericRepository<Resena>
    {
        Task<IEnumerable<Resena>> GetByLibroIdAsync(int libroId);
        Task<Resena?> GetByUsuarioYLibro(int usuarioId, int libroId);
    }
}
