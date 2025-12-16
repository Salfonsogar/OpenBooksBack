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
    public class DenunciaRepository : GenericRepository<Denuncia>, IDenunciaRepository
    {
        public DenunciaRepository(OpenBooksContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Denuncia>> GetDenunciasRealizadasPorUsuario(int usuarioId)
        {
            return await _dbSet
                .Where(d => d.UsuarioDenuncianteId == usuarioId)
                .Include(d => d.UsuarioDenunciadoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Denuncia>> GetDenunciasRecibidasPorUsuario(int usuarioId)
        {
            return await _dbSet
                .Where(d => d.UsuarioDenunciadoId == usuarioId)
                .Include(d => d.UsuarioDenuncianteId)
                .ToListAsync();
        }
    }
}
