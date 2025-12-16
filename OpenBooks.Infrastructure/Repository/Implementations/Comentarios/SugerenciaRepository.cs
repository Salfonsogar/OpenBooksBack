using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Comentarios;
using OpenBooks.Infrastructure.Repository.Interfaces.Comentarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

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
