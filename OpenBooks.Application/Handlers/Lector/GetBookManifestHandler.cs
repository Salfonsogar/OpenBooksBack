using MediatR;
using OpenBooks.Application.Commands.Lector;
using OpenBooks.Application.Common;
using OpenBooks.Application.DTOs.Lector;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Services.Lector;
using OpenBooks.Application.Validations.Lector;

namespace OpenBooks.Application.Handlers.Lector
{
    public class GetBookManifestHandler : IRequestHandler<GetBookManifestCommand, Result<BookManifestDto>>
    {
        private readonly IEpubParser _epubParser;
        private readonly ILibroRepository _repository;
        private readonly GetBookManifestValidator _validator;


        public GetBookManifestHandler(IEpubParser epubParser, ILibroRepository repository, GetBookManifestValidator validator)
        {
            _epubParser = epubParser;
            _repository = repository;
            _validator = validator;
        }
        public async Task<Result<BookManifestDto>> Handle(GetBookManifestCommand request, CancellationToken ct)
        {

            var validationResult = await _validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<BookManifestDto>.Failure(errors);
            }
            var libro = await _repository.GetByIdAsync(request.BookId);
            if (libro == null)
                return Result<BookManifestDto>.Failure("Libro no encontrado");

            if (libro.Archivo is null || libro.Archivo.Length == 0)
                return Result<BookManifestDto>.Failure("El libro no contiene un EPUB válido");

            var opfResult = _epubParser.Parse(libro.Archivo);
            if (!opfResult.IsSuccess)
                return Result<BookManifestDto>.Failure(opfResult.Error ?? "Error al parsear EPUB");

            var opf = opfResult.Data!;

            var links = new List<LinkDto>
            {
                new LinkDto
                {
                    Rel = "self",
                    Href = $"/api/books/{request.BookId}/manifest",
                    Type = "application/webpub+json",
                }
            };
            var tocResource = opf.Resources
                .FirstOrDefault(r => r.MediaType == "application/x-dtbncx+xml");

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
                    Identifier = opf.Identifier,
                    Title = opf.Title,
                    Language = opf.Language,
                    Author = opf.Authors
                },
                Links = links,
                ReadingOrder = opf.Spine.Select(i => new LinkDto
                {
                    Href = i.Href,
                    Type = i.MediaType,
                    Title = i.Title
                }).ToList(),
                Resources = opf.Resources.Select(i => new LinkDto
                {
                    Href = i.Href,
                    Type = i.MediaType
                }).ToList(),
                Toc = BuildToc(opf.Toc)
            };

            return Result<BookManifestDto>.Success(manifest);
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
