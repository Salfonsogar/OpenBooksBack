using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Services.Auth
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string From { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }

}
