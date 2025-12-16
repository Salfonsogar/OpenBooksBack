using OpenBooks.Domain.Entities.Comentarios;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Comentarios
{
    public interface IResenaRepository : IGenericRepository<Resena>
    {
        Task<IEnumerable<Resena>> GetByLibroIdAsync(int libroId);
        Task<Resena?> GetByUsuarioYLibro(int usuarioId, int libroId);
    }
}
