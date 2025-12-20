using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;

namespace OpenBooks.Application.Commands.Lector
{
    public record GetBookResourceCommand(int BookId, string ResourcePath) : IRequest<Result<BookResourceDto>>;
}
