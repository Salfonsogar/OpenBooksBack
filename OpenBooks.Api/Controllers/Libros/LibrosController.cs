using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Api.Controllers.Libros
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibrosController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibroCreateDto dto)
        {
            var result = await _libroService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data },
                result.Data
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LibroUpdateDto dto)
        {
            var result = await _libroService.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] LibroPatchDto dto)
        {
            var result = await _libroService.PatchAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _libroService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetCards([FromQuery] PaginationParams pagination)
        {
            var result = await _libroService.GetCardsAsync(pagination);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _libroService.DeleteAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return NoContent();
        }
    }
}
