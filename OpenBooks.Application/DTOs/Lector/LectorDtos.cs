using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenBooks.Application.DTOs.Lector
{
    public class BookManifestDto
    {
        [JsonPropertyName("@context")]
        public string Context { get; set; } = "https://readium.org/webpub-manifest/context.jsonld";
        public MetadataDto Metadata { get; set; } = new();
        public List<LinkDto> Links { get; set; } = new();
        public List<LinkDto> ReadingOrder { get; set; } = new();
        public List<LinkDto> Resources { get; set; } = new();
        public List<TocItemDto> Toc { get; set; } = new();
    }


    public class MetadataDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "en";
        public string Modified { get; set; } = DateTime.UtcNow.ToString("O");
        public List<string> Author { get; set; } = new();
    }


    public class LinkDto
    {
        public string Href { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Rel { get; set; }
        public string? Title { get; set; }
    }


    public class TocItemDto
    {
        public string Href { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<TocItemDto>? Children { get; set; }
    }

    public class BookResourceDto
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string MediaType { get; set; } = "application/octet-stream";
        public string FileName { get; set; } = string.Empty;
    }

    public class LocationsDto
    {
        public int? Position { get; set; }
        public decimal? Progression { get; set; }
        public decimal? TotalProgression { get; set; }
        public string? Fragment { get; set; }
    }

    public class LocatorDto
    {
        public string Href { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Title { get; set; }
        public LocationsDto? Locations { get; set; }
        public string? Text { get; set; }
    }

    public class ProgressDto
    {
        public int LibroUsuarioId { get; set; }
        public LocatorDto? CurrentLocator { get; set; }
        public decimal? Progression { get; set; }
        public DateTime? LastReadAt { get; set; }
    }

    public class MarcadorDto
    {
        public int Id { get; set; }
        public int LibroUsuarioId { get; set; }
        public string? Label { get; set; }
        public LocatorDto Locator { get; set; } = new LocatorDto();
        public string? Metadata { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ResaltadoDto
    {
        public int Id { get; set; }
        public int LibroUsuarioId { get; set; }
        public LocatorDto LocatorStart { get; set; } = new LocatorDto();
        public LocatorDto LocatorEnd { get; set; } = new LocatorDto();
        public string SelectedText { get; set; } = string.Empty;
        public string? Context { get; set; }
        public string? Note { get; set; }
        public string Color { get; set; } = "#ffeb3b";
        public string Type { get; set; } = "highlight";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
