using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Usuarios;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Implementations.Usuarios
{
    public class SancionRepository : GenericRepository<Sancion>, ISancionRepository
    {
        public SancionRepository(OpenBooksContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sancion>> GetSancionesUsuario(int usuarioId)
        {
            return await _dbSet
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
