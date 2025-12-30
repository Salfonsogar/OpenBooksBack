using MediatR;
using OpenBooks.Application.Commands.Auth;
using OpenBooks.Application.Interfaces.Services.Auth.Interfaces;

namespace OpenBooks.Application.Handlers.Auth
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IResetPasswordService _Service;

        public ResetPasswordHandler(IResetPasswordService service)
        {
            _Service = service;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var decodedToken = Uri.UnescapeDataString(request.Token);

            return await _Service.ResetPasswordAsync(
                request.Email,
                decodedToken,
                request.NewPassword
            );
        }
    }
}
