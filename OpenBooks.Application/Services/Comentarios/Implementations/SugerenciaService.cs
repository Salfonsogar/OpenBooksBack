using FluentValidation;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Comentarios;
using OpenBooks.Application.Services.Comentarios.Interfaces;
using OpenBooks.Domain.Entities.Comentarios;

namespace OpenBooks.Application.Services.Comentarios.Implementations
{
    public class SugerenciaService : ISugerenciaService
    {
        private readonly IUnitOfWork _unit;
        private readonly IValidator<SugerenciaCreateDto> _createValidator;

        public SugerenciaService(IUnitOfWork unit, IValidator<SugerenciaCreateDto> createValidator)
        {
            _unit = unit;
            _createValidator = createValidator;
        }

        public async Task<Result<SugerenciaResponseDto>> CreateAsync(SugerenciaCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return Result<SugerenciaResponseDto>.Failure(validation.Errors.First().ErrorMessage);

            try
            {
                var usuario = await _unit.Usuarios.GetByIdAsync(dto.UsuarioId);
                if (usuario == null)
                    return Result<SugerenciaResponseDto>.Failure("Usuario no existe");

                var sugerencia = new Sugerencia
                {
                    UsuarioId = dto.UsuarioId,
                    Descripcion = dto.Descripcion,
                    Fecha = DateTime.UtcNow
                };

                await _unit.Sugerencias.AddAsync(sugerencia);
                await _unit.CommitAsync();

                var resp = new SugerenciaResponseDto
                {
                    Id = sugerencia.Id,
                    UsuarioId = sugerencia.UsuarioId,
                    Descripcion = sugerencia.Descripcion,
                    Fecha = sugerencia.Fecha,
                    NombreUsuario = usuario.NombreUsuario
                };

                return Result<SugerenciaResponseDto>.Success(resp);
            }
            catch (Exception ex)
            {
                return Result<SugerenciaResponseDto>.Failure($"Error al crear sugerencia: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id, int usuarioSolicitanteId)
        {
            try
            {
                var s = await _unit.Sugerencias.GetByIdAsync(id);
                if (s == null)
                    return Result.Failure("Sugerencia no encontrada");

                // permitir eliminar solo al autor; si necesitas admins, añade validación adicional
                if (s.UsuarioId != usuarioSolicitanteId)
                    return Result.Failure("No autorizado");

                _unit.Sugerencias.Remove(s);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar sugerencia: {ex.Message}");
            }
        }

        public async Task<Result<SugerenciaResponseDto>> GetByIdAsync(int id)
        {
            try
            {
                var s = await _unit.Sugerencias.GetByIdAsync(id);
                if (s == null)
                    return Result<SugerenciaResponseDto>.Failure("Sugerencia no encontrada");

                var usuario = await _unit.Usuarios.GetByIdAsync(s.UsuarioId);

                var dto = new SugerenciaResponseDto
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    Descripcion = s.Descripcion,
                    Fecha = s.Fecha,
                    NombreUsuario = usuario?.NombreUsuario
                };

                return Result<SugerenciaResponseDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<SugerenciaResponseDto>.Failure($"Error al obtener sugerencia: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<SugerenciaResponseDto>>> GetAllAsync(PaginationParams pagination)
        {
            try
            {
                var query = _unit.Sugerencias
                    .Query()
                    .OrderByDescending(s => s.Fecha)
                    .Select(s => new SugerenciaResponseDto
                    {
                        Id = s.Id,
                        UsuarioId = s.UsuarioId,
                        Descripcion = s.Descripcion,
                        Fecha = s.Fecha,
                        NombreUsuario = s.Usuario.NombreUsuario
                    });

                var paged = query.ToPagedResult(pagination.Page, pagination.PageSize);

                return Result<PagedResult<SugerenciaResponseDto>>.Success(paged);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<SugerenciaResponseDto>>.Failure($"Error al listar sugerencias: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<SugerenciaResponseDto>>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var list = await _unit.Sugerencias.GetByUsuarioIdAsync(usuarioId);
                var dto = list.Select(s => new SugerenciaResponseDto
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    Descripcion = s.Descripcion,
                    Fecha = s.Fecha,
                    NombreUsuario = s.Usuario?.NombreUsuario
                }).ToList();

                return Result<IEnumerable<SugerenciaResponseDto>>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<SugerenciaResponseDto>>.Failure($"Error al obtener sugerencias del usuario: {ex.Message}");
            }
        }
    }
}
