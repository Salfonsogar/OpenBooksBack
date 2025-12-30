using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Usuarios
{
    public interface ISancionRepository : IGenericRepository<Sancion>
    {
        Task<IEnumerable<Sancion>> GetSancionesUsuario(int usuarioId);
    }
}
