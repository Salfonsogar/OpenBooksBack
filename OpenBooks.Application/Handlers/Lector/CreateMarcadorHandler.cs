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
    public class CreateMarcadorHandler : IRequestHandler<CreateMarcadorCommand, Result<MarcadorDto>>
    {
        private readonly IUnitOfWork _unit;

        public CreateMarcadorHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<MarcadorDto>> Handle(CreateMarcadorCommand request, CancellationToken ct)
        {
            LibroUsuario? libroUsuario;
            try
            {
                libroUsuario = await _unit.LibroUsuarios.GetByUsuarioAndLibroAsync(request.UsuarioId, request.LibroId, ct);
            }
            catch (Exception ex)
            {
                return Result<MarcadorDto>.Failure($"Error al acceder a la base de datos: {ex.Message}");
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
                    await _unit.CommitAsync(); // commit para obtener Id
                }

                // comprobar duplicado por href
                var href = request.Locator.Href ?? string.Empty;
                if (await _unit.Marcadores.ExistsByHrefAsync(libroUsuario.Id, href, ct))
                    return Result<MarcadorDto>.Failure("Ya existe un marcador en esta ubicación");

                var marcador = new Marcador
                {
                    LibroUsuarioId = libroUsuario.Id,
                    Label = request.Label,
                    Locator = JsonSerializer.Serialize(request.Locator),
                    Href = href,
                    Progression = request.Locator.Locations?.Progression,
                    Metadata = request.Metadata,
                    CreatedAt = DateTime.UtcNow
                };

                await _unit.Marcadores.AddAsync(marcador);
                await _unit.CommitAsync();

                var dto = new MarcadorDto
                {
                    Id = marcador.Id,
                    LibroUsuarioId = marcador.LibroUsuarioId,
                    Label = marcador.Label,
                    Locator = request.Locator,
                    Metadata = marcador.Metadata,
                    CreatedAt = marcador.CreatedAt
                };

                return Result<MarcadorDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<MarcadorDto>.Failure($"Error al crear marcador: {ex.Message}");
            }
        }
    }
}
