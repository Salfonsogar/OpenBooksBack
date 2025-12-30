using MediatR;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Commands.Lector
{
    public record UpdateProgressCommand(int LibroId, int UsuarioId, LocatorDto CurrentLocator, DateTime ClientTimestamp)
        : IRequest<Result<ProgressDto>>;

    public record DeleteProgressCommand(int LibroId, int UsuarioId) : IRequest<Result>;
}
