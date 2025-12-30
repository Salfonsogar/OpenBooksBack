using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Commands.Auth;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.Services.Auth.Interfaces;

namespace OpenBooks.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;

        public AuthController(
            IAuthService authService,
            IMediator mediator)
        {
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(
                actionName: "GetById",
                controllerName: "Usuario",
                routeValues: new { id = result.Data!.Id },
                value: result.Data
            );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequestDto dto,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest("RefreshToken es requerido.");

            var command = new GenerateRefreshTokenCommand(dto.RefreshToken);
            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess)
                return Unauthorized(result.Error);

            return Ok(result.Data);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Email, token y nueva contraseña son obligatorios");
            }

            var cmd = new ResetPasswordCommand
            {
                Email = request.Email,
                Token = request.Token,
                NewPassword = request.NewPassword
            };

            var result = await _mediator.Send(cmd);

            if (!result)
                return BadRequest("Token inválido o no se pudo restablecer la contraseña");

            return NoContent();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("El email es obligatorio");

            var cmd = new ForgotPasswordCommand { Email = request.Email };
            var result = await _mediator.Send(cmd);

            if (!result)
                return BadRequest("No se pudo procesar la solicitud");
            return NoContent();
        }
    }
}
