using OpenBooks.Domain.Entities.Comentarios;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Comentarios
{
    public interface IDenunciaRepository : IGenericRepository<Denuncia>
    {
        Task<IEnumerable<Denuncia>> GetDenunciasRealizadasPorUsuario(int usuarioId);
        Task<IEnumerable<Denuncia>> GetDenunciasRecibidasPorUsuario(int usuarioId);
    }
}
