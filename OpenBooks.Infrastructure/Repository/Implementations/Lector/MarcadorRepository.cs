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
    public class MarcadorRepository : GenericRepository<Marcador>, IMarcadorRepository
    {
        public MarcadorRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<List<Marcador>> GetByLibroUsuarioIdAsync(int libroUsuarioId, CancellationToken ct = default)
        {
            return await _context.Set<Marcador>()
                .AsNoTracking()
                .Where(m => m.LibroUsuarioId == libroUsuarioId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<Marcador?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Marcador>()
                .FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<Marcador> AddAsync(Marcador marcador, CancellationToken ct = default)
        {
            await _context.Set<Marcador>().AddAsync(marcador, ct);
            return marcador;
        }

        public Task UpdateAsync(Marcador marcador, CancellationToken ct = default)
        {
            _context.Set<Marcador>().Update(marcador);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Marcador marcador, CancellationToken ct = default)
        {
            _context.Set<Marcador>().Remove(marcador);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByHrefAsync(int libroUsuarioId, string href, CancellationToken ct = default)
        {
            return await _context.Set<Marcador>()
                .AsNoTracking()
                .AnyAsync(m => m.LibroUsuarioId == libroUsuarioId && m.Href == href, ct);
        }
    }
}
