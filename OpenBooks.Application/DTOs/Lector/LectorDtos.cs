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
}
