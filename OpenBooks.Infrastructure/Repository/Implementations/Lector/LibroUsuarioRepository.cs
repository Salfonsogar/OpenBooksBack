using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Lector;
using OpenBooks.Domain.Entities.Lector;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Implementations.Lector
{
    public class LibroUsuarioRepository : GenericRepository<LibroUsuario>, ILibroUsuarioRepository
    {
        public LibroUsuarioRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<LibroUsuario?> GetByUsuarioAndLibroAsync(int usuarioId, int libroId, CancellationToken ct = default)
        {
            return await _context.Set<LibroUsuario>()
                .AsNoTracking()
                .Include(lu => lu.Marcadores)
                .Include(lu => lu.Resaltados)
                .FirstOrDefaultAsync(lu => lu.UsuarioId == usuarioId && lu.LibroId == libroId, ct);
        }

        public async Task<LibroUsuario?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<LibroUsuario>()
                .AsNoTracking()
                .Include(lu => lu.Marcadores)
                .Include(lu => lu.Resaltados)
                .FirstOrDefaultAsync(lu => lu.Id == id, ct);
        }

        public async Task<LibroUsuario> AddAsync(LibroUsuario libroUsuario, CancellationToken ct = default)
        {
            await _context.Set<LibroUsuario>().AddAsync(libroUsuario, ct);
            return libroUsuario;
        }

        public Task UpdateAsync(LibroUsuario libroUsuario, CancellationToken ct = default)
        {
            _context.Set<LibroUsuario>().Update(libroUsuario);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int usuarioId, int libroId, CancellationToken ct = default)
        {
            return await _context.Set<LibroUsuario>()
                .AsNoTracking()
                .AnyAsync(lu => lu.UsuarioId == usuarioId && lu.LibroId == libroId, ct);
        }
    }
}
