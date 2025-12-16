using Microsoft.AspNetCore.Identity;
using OpenBooks.Infrastructure.Services.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Services.Auth.Implementations
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public ResetPasswordService(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception("Usuario no existe.");

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception("Usuario no existe.");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return true;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var url = $"https://app.com/reset-password?email={email}&token={token}";

            var html = $@"
                <p>Recuperación de contraseña</p>
                <p>Haz clic en este enlace para continuar:</p>
                <a href=""{url}"">Recuperar contraseña</a>
            ";

            await _emailService.SendEmailAsync(email, "Recuperar contraseña", html);

            return true;
        }

    }
}
