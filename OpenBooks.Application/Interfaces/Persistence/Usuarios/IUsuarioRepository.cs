using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Persistence.Usuarios
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetUsuarioConBibliotecaAsync(int id);
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByIdWithRolAsync(int id);
        Task<IEnumerable<Usuario>> GetAllWithRolAsync(int skip, int take);
    }
}
