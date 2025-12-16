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
    public class ResenaRepository : GenericRepository<Resena>, IResenaRepository
    {
        public ResenaRepository(OpenBooksContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Resena>> GetByLibroIdAsync(int libroId)
        {
            return await _dbSet
                .Where(r => r.LibroId == libroId)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        public async Task<Resena?> GetByUsuarioYLibro(int usuarioId, int libroId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.LibroId == libroId);
        }
    }
}
