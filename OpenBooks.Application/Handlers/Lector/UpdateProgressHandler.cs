using MediatR;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;
using OpenBooks.Domain.Entities.Lector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace OpenBooks.Application.Handlers.Lector
{
    public class UpdateProgressHandler : IRequestHandler<UpdateProgressCommand, Result<ProgressDto>>
    {
        private readonly IUnitOfWork _unit;

        public UpdateProgressHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<ProgressDto>> Handle(UpdateProgressCommand request, CancellationToken ct)
        {
            LibroUsuario? libroUsuario;
            try
            {
                libroUsuario = await _unit.LibroUsuarios.GetByUsuarioAndLibroAsync(request.UsuarioId, request.LibroId, ct);
            }
            catch (Exception ex)
            {
                return Result<ProgressDto>.Failure($"Error al acceder a la base de datos: {ex.Message}");
            }

            try
            {
                if (libroUsuario == null)
                {
                    libroUsuario = new LibroUsuario
                    {
                        LibroId = request.LibroId,
                        UsuarioId = request.UsuarioId,
                        CurrentLocator = JsonSerializer.Serialize(request.CurrentLocator),
                        CurrentHref = request.CurrentLocator.Href,
                        Progression = request.CurrentLocator.Locations?.Progression,
                        LastReadAt = request.ClientTimestamp,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unit.LibroUsuarios.AddAsync(libroUsuario);
                }
                else
                {
                    libroUsuario.CurrentLocator = JsonSerializer.Serialize(request.CurrentLocator);
                    libroUsuario.CurrentHref = request.CurrentLocator.Href;
                    libroUsuario.Progression = request.CurrentLocator.Locations?.Progression;
                    libroUsuario.LastReadAt = request.ClientTimestamp;
                    libroUsuario.UpdatedAt = DateTime.UtcNow;

                    _unit.LibroUsuarios.Update(libroUsuario);
                }

                await _unit.CommitAsync();

                var dto = new ProgressDto
                {
                    LibroUsuarioId = libroUsuario.Id,
                    CurrentLocator = request.CurrentLocator,
                    Progression = libroUsuario.Progression,
                    LastReadAt = libroUsuario.LastReadAt
                };

                return Result<ProgressDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<ProgressDto>.Failure($"Error al actualizar progreso: {ex.Message}");
            }
        }
    }

    public class DeleteProgressHandler : IRequestHandler<DeleteProgressCommand, Result>
    {
        private readonly IUnitOfWork _unit;

        public DeleteProgressHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result> Handle(DeleteProgressCommand request, CancellationToken ct)
        {
            try
            {
                var libroUsuario = await _unit.LibroUsuarios.GetByUsuarioAndLibroAsync(request.UsuarioId, request.LibroId, ct);
                if (libroUsuario == null)
                    return Result.Success();

                if (libroUsuario.Marcadores != null && libroUsuario.Marcadores.Any())
                {
                    foreach (var m in libroUsuario.Marcadores.ToList())
                    {
                        _unit.Marcadores.Remove(m);
                    }
                }
                if (libroUsuario.Resaltados != null && libroUsuario.Resaltados.Any())
                {
                    foreach (var r in libroUsuario.Resaltados.ToList())
                    {
                        _unit.Resaltados.Remove(r);
                    }
                }

                _unit.LibroUsuarios.Remove(libroUsuario);

                await _unit.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al eliminar progreso de lectura: {ex.Message}");
            }
        }
    }
}
