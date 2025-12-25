using MediatR;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;
using OpenBooks.Domain.Entities.Lector;
using System.Text.Json;

namespace OpenBooks.Application.Handlers.Lector
{
    public class CreateResaltadoHandler : IRequestHandler<CreateResaltadoCommand, Result<ResaltadoDto>>
    {
        private readonly IUnitOfWork _unit;

        public CreateResaltadoHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<ResaltadoDto>> Handle(CreateResaltadoCommand request, CancellationToken ct)
        {
            if (request.LocatorStart == null || request.LocatorEnd == null)
                return Result<ResaltadoDto>.Failure("Los locators (start/end) son obligatorios");

            if (string.IsNullOrWhiteSpace(request.SelectedText))
                return Result<ResaltadoDto>.Failure("El texto seleccionado es obligatorio");

            LibroUsuario? libroUsuario;
            try
            {
                libroUsuario = await _unit.LibroUsuarios.GetByUsuarioAndLibroAsync(request.UsuarioId, request.LibroId, ct);
            }
            catch (Exception ex)
            {
                return Result<ResaltadoDto>.Failure($"Error al acceder a la base de datos: {ex.Message}");
            }

            try
            {
                if (libroUsuario == null)
                {
                    libroUsuario = new LibroUsuario
                    {
                        LibroId = request.LibroId,
                        UsuarioId = request.UsuarioId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unit.LibroUsuarios.AddAsync(libroUsuario);
                    await _unit.CommitAsync(); 
                }

                var locatorStartJson = JsonSerializer.Serialize(request.LocatorStart);
                var locatorEndJson = JsonSerializer.Serialize(request.LocatorEnd);

                if (await _unit.Resaltados.ExistsByRangeAsync(libroUsuario.Id, locatorStartJson, locatorEndJson, ct))
                    return Result<ResaltadoDto>.Failure("Ya existe un resaltado en ese rango");

                var href = request.LocatorStart.Href ?? string.Empty;

                var resaltado = new Resaltado
                {
                    LibroUsuarioId = libroUsuario.Id,
                    LocatorStart = locatorStartJson,
                    LocatorEnd = locatorEndJson,
                    Href = href,
                    Progression = request.LocatorStart.Locations?.Progression,
                    SelectedText = request.SelectedText,
                    Context = request.Context,
                    Note = request.Note,
                    Color = request.Color,
                    Type = request.Type,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unit.Resaltados.AddAsync(resaltado);
                await _unit.CommitAsync();

                var dto = new ResaltadoDto
                {
                    Id = resaltado.Id,
                    LibroUsuarioId = resaltado.LibroUsuarioId,
                    LocatorStart = request.LocatorStart,
                    LocatorEnd = request.LocatorEnd,
                    SelectedText = resaltado.SelectedText,
                    Context = resaltado.Context,
                    Note = resaltado.Note,
                    Color = resaltado.Color,
                    Type = resaltado.Type,
                    CreatedAt = resaltado.CreatedAt,
                    UpdatedAt = resaltado.UpdatedAt
                };

                return Result<ResaltadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<ResaltadoDto>.Failure($"Error al crear resaltado: {ex.Message}");
            }
        }
    }

    public class GetResaltadosHandler : IRequestHandler<GetResaltadosQuery, Result<List<ResaltadoDto>>>
    {
        private readonly IUnitOfWork _unit;

        public GetResaltadosHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<List<ResaltadoDto>>> Handle(GetResaltadosQuery request, CancellationToken ct)
        {
            try
            {
                var libroUsuario = await _unit.LibroUsuarios.GetByUsuarioAndLibroAsync(request.UsuarioId, request.LibroId, ct);
                if (libroUsuario == null)
                    return Result<List<ResaltadoDto>>.Success(new List<ResaltadoDto>());

                var resaltados = await _unit.Resaltados.GetByLibroUsuarioIdAsync(libroUsuario.Id, ct);

                var list = resaltados.Select(r =>
                {
                    LocatorDto start = JsonSerializer.Deserialize<LocatorDto>(r.LocatorStart) ?? new LocatorDto();
                    LocatorDto end = JsonSerializer.Deserialize<LocatorDto>(r.LocatorEnd) ?? new LocatorDto();

                    return new ResaltadoDto
                    {
                        Id = r.Id,
                        LibroUsuarioId = r.LibroUsuarioId,
                        LocatorStart = start,
                        LocatorEnd = end,
                        SelectedText = r.SelectedText,
                        Context = r.Context,
                        Note = r.Note,
                        Color = r.Color,
                        Type = r.Type,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    };
                }).ToList();

                return Result<List<ResaltadoDto>>.Success(list);
            }
            catch (Exception ex)
            {
                return Result<List<ResaltadoDto>>.Failure($"Error al obtener resaltados: {ex.Message}");
            }
        }
    }

    public class DeleteResaltadoHandler : IRequestHandler<DeleteResaltadoCommand, Result>
    {
        private readonly IUnitOfWork _unit;

        public DeleteResaltadoHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result> Handle(DeleteResaltadoCommand request, CancellationToken ct)
        {
            try
            {
                var resaltado = await _unit.Resaltados.GetByIdAsync(request.ResaltadoId);
                if (resaltado == null)
                    return Result.Failure("Resaltado no encontrado");
                var libroUsuario = await _unit.LibroUsuarios.GetByIdWithDetailsAsync(resaltado.LibroUsuarioId, ct);
                if (libroUsuario == null || libroUsuario.UsuarioId != request.UsuarioId || libroUsuario.LibroId != request.LibroId)
                    return Result.Failure("No autorizado o resaltado no pertenece al usuario/libro");

                _unit.Resaltados.Remove(resaltado);
                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar resaltado: {ex.Message}");
            }
        }
    }

    public class UpdateResaltadoHandler : IRequestHandler<UpdateResaltadoCommand, Result<ResaltadoDto>>
    {
        private readonly IUnitOfWork _unit;

        public UpdateResaltadoHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<ResaltadoDto>> Handle(UpdateResaltadoCommand request, CancellationToken ct)
        {
            try
            {
                var resaltado = await _unit.Resaltados.GetByIdAsync(request.ResaltadoId);
                if (resaltado == null)
                    return Result<ResaltadoDto>.Failure("Resaltado no encontrado");

                var libroUsuario = await _unit.LibroUsuarios.GetByIdWithDetailsAsync(resaltado.LibroUsuarioId, ct);
                if (libroUsuario == null || libroUsuario.UsuarioId != request.UsuarioId || libroUsuario.LibroId != request.LibroId)
                    return Result<ResaltadoDto>.Failure("No autorizado o resaltado no pertenece al usuario/libro");

                var modified = false;
                if (request.Note != null && request.Note != resaltado.Note)
                {
                    resaltado.Note = request.Note;
                    modified = true;
                }
                if (request.Color != null && request.Color != resaltado.Color)
                {
                    resaltado.Color = request.Color;
                    modified = true;
                }
                if (request.Type != null && request.Type != resaltado.Type)
                {
                    resaltado.Type = request.Type;
                    modified = true;
                }

                if (!modified)
                    return Result<ResaltadoDto>.Failure("Nada que actualizar");

                resaltado.UpdatedAt = DateTime.UtcNow;
                _unit.Resaltados.Update(resaltado);
                await _unit.CommitAsync();

                var start = System.Text.Json.JsonSerializer.Deserialize<LocatorDto>(resaltado.LocatorStart) ?? new LocatorDto();
                var end = System.Text.Json.JsonSerializer.Deserialize<LocatorDto>(resaltado.LocatorEnd) ?? new LocatorDto();

                var dto = new ResaltadoDto
                {
                    Id = resaltado.Id,
                    LibroUsuarioId = resaltado.LibroUsuarioId,
                    LocatorStart = start,
                    LocatorEnd = end,
                    SelectedText = resaltado.SelectedText,
                    Context = resaltado.Context,
                    Note = resaltado.Note,
                    Color = resaltado.Color,
                    Type = resaltado.Type,
                    CreatedAt = resaltado.CreatedAt,
                    UpdatedAt = resaltado.UpdatedAt
                };

                return Result<ResaltadoDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<ResaltadoDto>.Failure($"Error al actualizar resaltado: {ex.Message}");
            }
        }
    }
}
