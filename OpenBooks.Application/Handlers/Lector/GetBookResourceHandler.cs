using System.Collections.Concurrent;
using System.IO.Compression;
using System.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Services.Lector;

namespace OpenBooks.Application.Handlers.Lector
{
    public class GetBookResourceHandler : IRequestHandler<GetBookResourceCommand, Result<BookResourceDto>>
    {
        private readonly IEpubParser _epubParser;
        private readonly ILibroRepository _repository;
        private readonly IMemoryCache _cache;

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public GetBookResourceHandler(IEpubParser epubParser, ILibroRepository repository, IMemoryCache cache)
        {
            _epubParser = epubParser;
            _repository = repository;
            _cache = cache;
        }

        public async Task<Result<BookResourceDto>> Handle(GetBookResourceCommand request, CancellationToken ct)
        {
            var libro = await _repository.GetByIdAsync(request.BookId);
            if (libro == null)
                return Result<BookResourceDto>.Failure("Libro no encontrado");

            if (libro.Archivo is null || libro.Archivo.Length == 0)
                return Result<BookResourceDto>.Failure("El libro no contiene un EPUB válido");

            var hash = ComputeHash(libro.Archivo);
            var cacheKey = $"epub:parsed:{request.BookId}:{hash}";

            if (!_cache.TryGetValue(cacheKey, out ParsedOpf? parsed))
            {
                var semaphore = _locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
                await semaphore.WaitAsync(ct);
                try
                {
                    if (!_cache.TryGetValue(cacheKey, out parsed))
                    {
                        var opfResult = _epubParser.Parse(libro.Archivo);
                        if (!opfResult.IsSuccess)
                            return Result<BookResourceDto>.Failure(opfResult.Error ?? "Error al parsear EPUB");

                        parsed = opfResult.Data!;

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                            SlidingExpiration = TimeSpan.FromMinutes(10),
                            Size = 1
                        };
                        _cache.Set(cacheKey, parsed, cacheEntryOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                    _locks.TryRemove(cacheKey, out _);
                }
            }

            if (parsed == null)
                return Result<BookResourceDto>.Failure("No se pudo obtener la información del manifiesto");

            var candidates = new List<OpfItem>();
            if (parsed.Spine != null) candidates.AddRange(parsed.Spine);
            if (parsed.Resources != null) candidates.AddRange(parsed.Resources);

            var resourcePath = request.ResourcePath.TrimStart('/');
            var match = candidates
                .FirstOrDefault(i =>
                    string.Equals(i.Href, resourcePath, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(i.Href.TrimStart('/'), resourcePath, StringComparison.OrdinalIgnoreCase) ||
                    i.Href.EndsWith(resourcePath, StringComparison.OrdinalIgnoreCase));

            if (match == null)
            {
                match = candidates.FirstOrDefault(i =>
                    i.Href.EndsWith(Uri.UnescapeDataString(resourcePath), StringComparison.OrdinalIgnoreCase));
            }

            if (match == null)
                return Result<BookResourceDto>.Failure("Recurso no encontrado en el manifiesto del libro");

            using var ms = new MemoryStream(libro.Archivo);
            using var zip = new ZipArchive(ms, ZipArchiveMode.Read, leaveOpen: false);

            string[] tryNames = new[]
            {
                match.Href,
                match.Href.TrimStart('/'),
                Uri.UnescapeDataString(match.Href),
                Uri.UnescapeDataString(match.Href).TrimStart('/'),
                resourcePath,
                resourcePath.TrimStart('/'),
                Uri.UnescapeDataString(resourcePath),
                Uri.UnescapeDataString(resourcePath).TrimStart('/')
            };

            ZipArchiveEntry? entry = null;
            foreach (var name in tryNames.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                entry = zip.GetEntry(name);
                if (entry != null) break;
            }

            if (entry == null)
                return Result<BookResourceDto>.Failure("El recurso no existe dentro del archivo EPUB");

            using var entryStream = entry.Open();
            using var outMs = new MemoryStream();
            await entryStream.CopyToAsync(outMs, ct);

            var content = outMs.ToArray();
            var mediaType = string.IsNullOrWhiteSpace(match.MediaType) ? "application/octet-stream" : match.MediaType;
            var fileName = Path.GetFileName(entry.FullName);

            var dto = new BookResourceDto
            {
                Content = content,
                MediaType = mediaType,
                FileName = fileName
            };

            return Result<BookResourceDto>.Success(dto);
        }

        private static string ComputeHash(byte[] data)
        {
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(data);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
