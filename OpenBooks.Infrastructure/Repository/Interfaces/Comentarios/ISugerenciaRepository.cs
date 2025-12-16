using OpenBooks.Domain.Entities.Comentarios;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Comentarios
{
    public interface ISugerenciaRepository : IGenericRepository<Sugerencia>
    {
        Task<IEnumerable<Sugerencia>> GetByUsuarioIdAsync(int usuarioId);
    }
}
