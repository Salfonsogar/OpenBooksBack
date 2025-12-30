using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Services.Auth.Interfaces
{
    public interface IResetPasswordService
    {
        Task<bool> UserExistsAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> ForgotPasswordAsync(string email);
    }
}
