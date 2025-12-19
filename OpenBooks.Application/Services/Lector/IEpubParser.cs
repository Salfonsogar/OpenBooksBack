using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Services.Lector
{
    public interface IEpubParser
    {
        ParsedOpf Parse(byte[] epubContent);
    }
    public class ParsedOpf
    {
        public string Identifier { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();

        public List<OpfItem> Spine { get; set; } = new();

        public List<OpfItem> Resources { get; set; } = new();

        public List<OpfTocItem> Toc { get; set; } = new();
        public string? NavPath { get; init; }
    }


    public class OpfItem
    {
        public string Href { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Rel { get; set; }
    }
    public class OpfTocItem
    {
        public string Title { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public List<OpfTocItem>? Children { get; set; }
    }

}
