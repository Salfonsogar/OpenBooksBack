using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Interfaces.Services.Auth.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}
