using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Services.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string email);
        string GenerateRefreshToken();
    }
}
