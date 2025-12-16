using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooks.Infrastructure.Repository.Interfaces.Usuarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Implementations.Usuarios
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(OpenBooksContext context)
            : base(context)
        {
        }
        public async Task<Usuario?> GetUsuarioConBibliotecaAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Biblioteca)
                    .ThenInclude(b => b.Estanterias)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await Query(u => u.NombreUsuario == username)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await Query(u => u.Correo == email)
                .FirstOrDefaultAsync();
        }
        public async Task<Usuario?> GetByIdWithRolAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllWithRolAsync(int skip, int take)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
