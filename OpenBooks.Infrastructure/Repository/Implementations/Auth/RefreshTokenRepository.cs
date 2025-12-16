using Microsoft.EntityFrameworkCore;
using OpenBooks.Domain.Entities.Auth;
using OpenBooks.Infrastructure.Repository.Interfaces.Auth;
using OpenBooksBack.Infrastructure.Data;
using OpenBooksBack.Infrastructure.Repositories;
using static System.Net.Mime.MediaTypeNames;

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
