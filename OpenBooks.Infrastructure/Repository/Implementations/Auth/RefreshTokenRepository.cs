using Microsoft.EntityFrameworkCore;
using OpenBooks.Application.Interfaces.Persistence.Auth;
using OpenBooks.Domain.Entities.Auth;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;

namespace OpenBooks.Infrastructure.Repository.Implementations.Auth
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(OpenBooksContext context): base(context)
        {
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await Query(r => r.Token == token).FirstOrDefaultAsync();
        }
    }

}
