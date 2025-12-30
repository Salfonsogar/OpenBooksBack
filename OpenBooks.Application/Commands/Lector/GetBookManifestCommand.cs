using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;

namespace OpenBooks.Application.Commands.Lector
{
    public record GetBookManifestCommand(int BookId) : IRequest<Result<BookManifestDto>>;
}