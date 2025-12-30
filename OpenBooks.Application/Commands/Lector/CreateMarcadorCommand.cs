using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;

namespace OpenBooks.Application.Commands.Lector
{
    public record CreateMarcadorCommand(
        int LibroId,
        int UsuarioId,
        string? Label,
        LocatorDto Locator,
        string? Metadata
    ) : IRequest<Result<MarcadorDto>>;
}
