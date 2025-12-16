using OpenBooks.Domain.Entities.Usuarios;
using OpenBooksBack.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Usuarios
{
    public interface ISancionRepository : IGenericRepository<Sancion>
    {
        Task<IEnumerable<Sancion>> GetSancionesUsuario(int usuarioId);
    }
}
