using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Usuarios;
using OpenBooks.Infrastructure.Repository.Interfaces.Usuarios;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Usuarios
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(OpenBooksContext context)
            : base(context)
        {
        }

        public async Task<Rol?> GetByNameAsync(string name)
        {
            return await Query(r => r.Nombre.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Rol>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await Query(r => ids.Contains(r.Id))
                .ToListAsync();
        }
    }
}
