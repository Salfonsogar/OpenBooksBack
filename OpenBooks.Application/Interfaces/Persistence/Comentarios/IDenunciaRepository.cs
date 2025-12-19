using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Application.Interfaces.Persistence.Comentarios
{
    public interface IDenunciaRepository : IGenericRepository<Denuncia>
    {
        Task<IEnumerable<Denuncia>> GetDenunciasRealizadasPorUsuario(int usuarioId);
        Task<IEnumerable<Denuncia>> GetDenunciasRecibidasPorUsuario(int usuarioId);
    }
}
