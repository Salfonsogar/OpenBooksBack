using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBooks.Application.Commands.Auth;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Auth;
using OpenBooks.Application.DTOs.Usuarios;
using OpenBooks.Application.Services.Usuarios.Interfaces;

namespace OpenBooks.Api.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IMediator _mediator;

        public UsuarioController(IUsuarioService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioCreateDto dto)
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, UsuarioUpdateDto dto)
        {
            var result = await _service.PatchAsync(id, dto);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok("Usuario actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok("Usuario eliminado correctamente");
        }

        [HttpPatch("perfil")]
        [Authorize]
        public async Task<IActionResult> PatchPerfil(UsuarioUpdatePerfilDto dto)
        {
            var id = int.Parse(User.FindFirst("id")?.Value);

            var result = await _service.PatchPerfilAsync(id, dto);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok("Perfil actualizado.");
        }
    }
}
