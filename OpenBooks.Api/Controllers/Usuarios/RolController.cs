using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;

namespace OpenBooks.Api.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _service;

        public RolController(IRolService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RolCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (!result.IsSuccess)
                return Conflict(result.Error);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id },
                result.Data
            );
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var result = await _service.GetAllAsync(pagination);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok("Rol eliminado correctamente");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(int id, RolUpdateDto dto)
        {
            var result = await _service.PatchAsync(id, dto);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}
