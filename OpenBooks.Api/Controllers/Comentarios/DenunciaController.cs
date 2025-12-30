using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using OpenBooks.Application.Services.Comentarios.Interfaces;

namespace OpenBooks.Api.Controllers.Comentarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _service;

        public DenunciaController(IDenunciaService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var result = await _service.GetAllAsync(pagination);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Data);
        }

        [HttpGet("usuario/{usuarioId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByUsuarioDenunciado(int usuarioId)
        {
            var result = await _service.GetByUsuarioDenunciadoAsync(usuarioId);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] DenunciaCreateDto dto)
        {
            if (dto == null) return BadRequest("Request inválido");

            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var denuncianteId))
                return Unauthorized();

            var result = await _service.CreateAsync(dto, denuncianteId);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("Ya has denunciado", StringComparison.OrdinalIgnoreCase))
                    return Conflict(err);
                if (err.Contains("no existe", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.DeleteAsync(id, userId);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("No autorizado", StringComparison.OrdinalIgnoreCase))
                    return Unauthorized(err);
                if (err.Contains("no encontrada", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            return NoContent();
        }
    }
}
