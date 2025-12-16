using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; } = string.Empty;
    }
}
