using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;

namespace OpenBooks.Api.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class SancionController : ControllerBase
    {
        private readonly ISancionService _service;

        public SancionController(ISancionService service)
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
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var result = await _service.GetByUsuarioIdAsync(usuarioId);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] SancionCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] SancionUpdateDto dto)
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
