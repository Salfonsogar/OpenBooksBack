using OpenBooks.Domain.Entities.Usuarios;
using OpenBooksBack.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenBooks.Infrastructure.Repository.Interfaces.Usuarios
{
    public interface IRolRepository : IGenericRepository<Rol>
    {
        Task<Rol?> GetByNameAsync(string name);
        Task<Rol?> GetByIdAsync(int id);
        Task<IEnumerable<Rol>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
