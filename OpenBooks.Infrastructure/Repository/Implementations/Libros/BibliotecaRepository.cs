using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Libros;
using OpenBooks.Infrastructure.Repository.Interfaces.Libros;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Implementations.Libros
{
    public class BibliotecaRepository : GenericRepository<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(OpenBooksContext context) : base(context) { }

        public async Task<Biblioteca?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(b => b.Estanterias)
                    .ThenInclude(e => e.EstanteriaLibros)
                        .ThenInclude(el => el.Libro)
                .Include(b => b.BibliotecaLibros)
                    .ThenInclude(bl => bl.Libro)
                .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);
        }
    }
}
