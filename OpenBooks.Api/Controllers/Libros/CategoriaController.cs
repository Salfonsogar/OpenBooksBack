using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Api.Controllers.Libros
{
        [ApiController]
        [Route("api/[controller]")]
        public class CategoriaController : ControllerBase
        {
            private readonly ICategoriaService _service;
            public CategoriaController(ICategoriaService service)
            {
                _service = service;
            }
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            [HttpGet("paged")]
            public async Task<IActionResult> GetAllPaged([FromQuery] PaginationParams pagination)
            {
                var result = await _service.GetAllAsync(pagination);
                return Ok(result);
            }
            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await _service.GetByIdAsync(id);
                if (!result.IsSuccess) return NotFound(result.Error);
                return Ok(result);
            }
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CategoriaCreateDto dto)
            {
                var result = await _service.CreateAsync(dto);
                if (!result.IsSuccess) return BadRequest(result.Error);
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }
            [HttpPut("{id:int}")]
            public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateDto dto)
            {
                var result = await _service.UpdateAsync(id, dto);
                if (!result.IsSuccess) return BadRequest(result.Error);
                return Ok(result.Data);
            }
            [HttpPatch("{id:int}")]
            public async Task<IActionResult> Patch(int id, [FromBody] CategoriaPatchDto dto)
            {
                var result = await _service.PatchAsync(id, dto);
                if (!result.IsSuccess) return BadRequest(result.Error);
                return Ok(result.Data);
            }
            [HttpDelete("{id:int}")]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _service.DeleteAsync(id);
                if (!result.IsSuccess) return NotFound(result.Error);
                return NoContent();
            }
        }
}
