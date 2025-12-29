using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Api.Controllers.Libros
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecaController : ControllerBase
    {
        private readonly IBibliotecaService _service;
        private readonly IMediator _mediator;

        public BibliotecaController(IBibliotecaService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyLibrary()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.GetByUsuarioIdAsync(userId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("libros/{libroId:int}")]
        [Authorize]
        public async Task<IActionResult> AddLibro(int libroId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.AddLibroAsync(userId, libroId);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("ya está", StringComparison.OrdinalIgnoreCase))
                    return Conflict(err);
                if (err.Contains("no existe", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            return NoContent();
        }

        [HttpDelete("libros/{libroId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveLibro(int libroId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.RemoveLibroAsync(userId, libroId);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                    err.Contains("no se encuentra", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            await _mediator.Send(new DeleteProgressCommand(libroId, userId));

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMyLibrary()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.DeleteBibliotecaAsync(userId);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}
