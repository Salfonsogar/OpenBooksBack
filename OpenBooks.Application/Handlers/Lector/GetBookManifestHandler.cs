using System.Collections.Concurrent;
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
    public class GetBookManifestHandler : IRequestHandler<GetBookManifestCommand, Result<BookManifestDto>>
    {
        private readonly IEpubParser _epubParser;
        private readonly ILibroRepository _repository;
        private readonly IMemoryCache _cache;

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public GetBookManifestHandler(IEpubParser epubParser, ILibroRepository repository, IMemoryCache cache)
        {
            _epubParser = epubParser;
            _repository = repository;
            _cache = cache;
        }

        public async Task<Result<BookManifestDto>> Handle(GetBookManifestCommand request, CancellationToken ct)
        {
            var libro = await _repository.GetByIdAsync(request.BookId);
            if (libro == null)
                return Result<BookManifestDto>.Failure("Libro no encontrado");

            if (libro.Archivo is null || libro.Archivo.Length == 0)
                return Result<BookManifestDto>.Failure("El libro no contiene un EPUB válido");

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
                            return Result<BookManifestDto>.Failure(opfResult.Error ?? "Error al parsear EPUB");

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
                return Result<BookManifestDto>.Failure("No se pudo obtener el manifiesto del libro");

            var links = new List<LinkDto>
            {
                new LinkDto
                {
                    Rel = "self",
                    Href = $"/api/books/{request.BookId}/manifest",
                    Type = "application/webpub+json",
                }
            };

            var tocResource = parsed.Resources.FirstOrDefault(r => r.MediaType == "application/x-dtbncx+xml");
            if (tocResource != null)
            {
                links.Add(new LinkDto
                {
                    Rel = "contents",
                    Href = tocResource.Href,
                    Type = tocResource.MediaType
                });
            }

            var manifest = new BookManifestDto
            {
                Metadata = new MetadataDto
                {
                    Identifier = parsed.Identifier,
                    Title = parsed.Title,
                    Language = parsed.Language,
                    Author = parsed.Authors
                },
                Links = links,
                ReadingOrder = parsed.Spine.Select(i => new LinkDto
                {
                    Href = i.Href,
                    Type = i.MediaType,
                    Title = i.Title
                }).ToList(),
                Resources = parsed.Resources.Select(i => new LinkDto
                {
                    Href = i.Href,
                    Type = i.MediaType
                }).ToList(),
                Toc = BuildToc(parsed.Toc)
            };

            return Result<BookManifestDto>.Success(manifest);
        }

        private static string ComputeHash(byte[] data)
        {
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(data);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        private static List<TocItemDto> BuildToc(IEnumerable<OpfTocItem> tocItems)
        {
            if (tocItems == null || !tocItems.Any())
                return new List<TocItemDto>();

            return tocItems.Select(i => new TocItemDto
            {
                Title = i.Title,
                Href = i.Href,
                Children = i.Children?.Select(c => new TocItemDto
                {
                    Title = c.Title,
                    Href = c.Href,
                    Children = c.Children?.Select(cc => new TocItemDto
                    {
                        Title = cc.Title,
                        Href = cc.Href
                    }).ToList()
                }).ToList()
            }).ToList();
        }
    }
}
