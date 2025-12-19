using MediatR;
using OpenBooks.Application.Commands.Auth;
using OpenBooks.Application.Interfaces.Services.Auth.Interfaces;

namespace OpenBooks.Application.Handlers.Auth
{
    public class ForgotPasswordHandler
        : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly  IResetPasswordService _service;

        public ForgotPasswordHandler(IResetPasswordService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _service.ForgotPasswordAsync(request.Email);
        }
    }
}
