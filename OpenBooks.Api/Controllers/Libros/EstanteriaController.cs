using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Api.Controllers.Libros
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstanteriaController : ControllerBase
    {
        private readonly IEstanteriaService _service;

        public EstanteriaController(IEstanteriaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyEstanterias()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.GetByUsuarioIdAsync(userId);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] EstanteriaCreateDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.CreateAsync(userId, dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] EstanteriaUpdateDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.UpdateAsync(userId, id, dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.DeleteAsync(userId, id);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }

        [HttpPost("{id:int}/libros/{libroId:int}")]
        [Authorize]
        public async Task<IActionResult> AddLibro(int id, int libroId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.AddLibroAsync(userId, id, libroId);
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

        [HttpDelete("{id:int}/libros/{libroId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveLibro(int id, int libroId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _service.RemoveLibroAsync(userId, id, libroId);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("no encontrado", StringComparison.OrdinalIgnoreCase) ||
                    err.Contains("no se encuentra", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            return NoContent();
        }
    }
}
