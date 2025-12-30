using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using OpenBooks.Application.Services.Comentarios.Interfaces;

namespace OpenBooks.Api.Controllers.Comentarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResenaController : ControllerBase
    {
        private readonly IResenaService _service;

        public ResenaController(IResenaService service)
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
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Data);
        }

        [HttpGet("libro/{libroId:int}")]
        public async Task<IActionResult> GetByLibro(int libroId)
        {
            var result = await _service.GetByLibroIdAsync(libroId);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [HttpGet("usuario/{usuarioId:int}/libro/{libroId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByUsuarioAndLibro(int usuarioId, int libroId)
        {
            var result = await _service.GetByUsuarioAndLibroAsync(usuarioId, libroId);
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ResenaCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("Ya existe", StringComparison.OrdinalIgnoreCase))
                    return Conflict(err);

                return BadRequest(err);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] ResenaUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.IsSuccess) return NotFound(result.Error);
            return NoContent();
        }
    }
}
