using OpenBooks.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Services.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string email);
        string GenerateRefreshToken();
    }

}
