using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Api.Dtos.Libros;
using OpenBooks.Application.Common;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.DTOs.Libros;
using OpenBooks.Application.Handlers.Lector;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Api.Controllers.Libros
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroService _libroService;
        private readonly IMediator _mediator;

        public LibrosController(ILibroService libroService, IMediator mediator)
        {
            _libroService = libroService;
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromBody] LibroCreateRequest request)
        {
            using var portadaMs = new MemoryStream();
            await request.Portada.CopyToAsync(portadaMs);

            using var archivoMs = new MemoryStream();
            await request.Archivo.CopyToAsync(archivoMs);
            var dto = new LibroCreateDto
            {
                Titulo = request.Titulo,
                Autor = request.Autor,
                Descripcion = request.Descripcion,
                FechaPublicacion = request.FechaPublicacion,
                CategoriasIds = request.CategoriasIds,
                Portada = portadaMs.ToArray(),
                Archivo = archivoMs.ToArray()
            };
            var result = await _libroService.CreateAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
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
        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended([FromQuery]PaginationParams pagination)
        {
            var result = await _libroService.GetRecommendedAsync(pagination);
            return Ok(result);
        }
        [HttpGet("TopRated")]
        public async Task<IActionResult> GetTopRated([FromQuery] PaginationParams pagination)
        {
            var result = await _libroService.GetTopRatedAsync(pagination);
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] LibroSearchParams searchParams)
        {
            var result = await _libroService.SearchAsync(searchParams);
            return Ok(result);
        }
        [HttpGet("{id:int}/manifest")]
        [Produces("application/webpub+json")]
        public async Task<IActionResult> GetManifest(int id)
        {
            var result = await _mediator.Send(new GetBookManifestCommand(id));

            if (!result.IsSuccess)
            {
                var err = result.Error ?? "Error desconocido";
                if (err.Contains("no encontrado", StringComparison.OrdinalIgnoreCase))
                    return NotFound(err);

                return BadRequest(err);
            }

            return Ok(result.Data);
        }
    }
}
