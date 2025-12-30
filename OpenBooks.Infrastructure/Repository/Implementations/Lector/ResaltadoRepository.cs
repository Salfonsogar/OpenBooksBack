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
    public class ResaltadoRepository : GenericRepository<Resaltado>, IResaltadoRepository
    {
        public ResaltadoRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<List<Resaltado>> GetByLibroUsuarioIdAsync(int libroUsuarioId, CancellationToken ct = default)
        {
            return await _context.Set<Resaltado>()
                .AsNoTracking()
                .Where(r => r.LibroUsuarioId == libroUsuarioId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<Resaltado?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<Resaltado>()
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<Resaltado> AddAsync(Resaltado resaltado, CancellationToken ct = default)
        {
            await _context.Set<Resaltado>().AddAsync(resaltado, ct);
            return resaltado;
        }

        public Task UpdateAsync(Resaltado resaltado, CancellationToken ct = default)
        {
            _context.Set<Resaltado>().Update(resaltado);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Resaltado resaltado, CancellationToken ct = default)
        {
            _context.Set<Resaltado>().Remove(resaltado);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByRangeAsync(int libroUsuarioId, string locatorStart, string locatorEnd, CancellationToken ct = default)
        {
            return await _context.Set<Resaltado>()
                .AsNoTracking()
                .AnyAsync(r =>
                    r.LibroUsuarioId == libroUsuarioId &&
                    r.LocatorStart == locatorStart &&
                    r.LocatorEnd == locatorEnd, ct);
        }
    }
}
