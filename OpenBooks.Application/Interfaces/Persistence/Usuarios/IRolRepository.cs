using OpenBooks.Domain.Entities.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Application.Interfaces.Persistence.Usuarios
{
    public interface IRolRepository : IGenericRepository<Rol>
    {
        Task<Rol?> GetByNameAsync(string name);
        new Task<Rol?> GetByIdAsync(int id);
        Task<IEnumerable<Rol>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
