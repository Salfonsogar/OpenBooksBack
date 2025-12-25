using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;

namespace OpenBooks.Application.Commands.Lector
{
    public record CreateResaltadoCommand(
        int LibroId,
        int UsuarioId,
        LocatorDto LocatorStart,
        LocatorDto LocatorEnd,
        string SelectedText,
        string? Context,
        string? Note,
        string Color,
        string Type
    ) : IRequest<Result<ResaltadoDto>>;

    public record GetResaltadosQuery(int LibroId, int UsuarioId) : IRequest<Result<List<ResaltadoDto>>>;

    public record DeleteResaltadoCommand(int LibroId, int UsuarioId, int ResaltadoId) : IRequest<Result>;

    public record UpdateResaltadoCommand(
        int LibroId,
        int UsuarioId,
        int ResaltadoId,
        string? Note,
        string? Color,
        string? Type
    ) : IRequest<Result<ResaltadoDto>>;
}
