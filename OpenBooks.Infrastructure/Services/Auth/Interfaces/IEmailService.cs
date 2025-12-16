using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Services.Auth.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}
