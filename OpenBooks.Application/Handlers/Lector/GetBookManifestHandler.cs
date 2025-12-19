using MediatR;
using OpenBooks.Application.DTOs.Lector;
using OpenBooks.Application.Interfaces.Persistence.Libros;
using OpenBooks.Application.Services.Lector;
using OpenBooks.Application.Services.Libros.Interfaces;

namespace OpenBooks.Application.Handlers.Lector
{
    public record GetBookManifestQuery(int BookId) : IRequest<BookManifestDto>;


    public class GetBookManifestHandler : IRequestHandler<GetBookManifestQuery, BookManifestDto>
    {
        private readonly IEpubParser _epubParser;
        private readonly ILibroRepository _repository;


        public GetBookManifestHandler(IEpubParser epubParser, ILibroRepository repository)
        {
            _epubParser = epubParser;
            _repository = repository;
        }
        public async Task<BookManifestDto> Handle(GetBookManifestQuery request, CancellationToken ct)
        {
            var libro = await _repository.GetByIdAsync(request.BookId)
                ?? throw new KeyNotFoundException("Libro no encontrado");

            if (libro.Archivo is null || libro.Archivo.Length == 0)
                throw new InvalidOperationException("El libro no contiene un EPUB válido");

            var opf = _epubParser.Parse(libro.Archivo);

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


            return new BookManifestDto
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
        }
        private static List<TocItemDto> BuildToc(IEnumerable<OpfTocItem> tocItems)
        {
            if (tocItems == null || !tocItems.Any())
                return new List<TocItemDto>();

            return tocItems.Select(i => new TocItemDto
            {
                Title = i.Title,
                Href = i.Href,
                Children = i.Children != null && i.Children.Any()
                    ? BuildToc(i.Children)
                    : null
            }).ToList();
        }

    }
}
