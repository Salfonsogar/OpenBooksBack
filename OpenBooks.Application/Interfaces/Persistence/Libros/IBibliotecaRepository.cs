using OpenBooks.Domain.Entities.Libros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Libros
{
    public interface IBibliotecaRepository : IGenericRepository<Biblioteca>
    {
        Task<Biblioteca?> GetByUsuarioIdAsync(int usuarioId);
    }
}
