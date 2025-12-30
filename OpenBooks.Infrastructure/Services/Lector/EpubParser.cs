using System.IO.Compression;
using System.Xml.Linq;
using OpenBooks.Application.Services.Lector;
using OpenBooks.Application.Common;

namespace OpenBooks.Infrastructure.Services.Lector
{
    public class EpubParser : IEpubParser
    {
        public Result<ParsedOpf> Parse(byte[] epubContent)
        {
            if (epubContent == null || epubContent.Length == 0)
                return Result<ParsedOpf>.Failure("El contenido del EPUB está vacío");

            try
            {
                using var ms = new MemoryStream(epubContent);
                using var zip = new ZipArchive(ms, ZipArchiveMode.Read);

                // 1️⃣ container.xml
                var containerEntry = zip.GetEntry("META-INF/container.xml");
                if (containerEntry == null)
                    return Result<ParsedOpf>.Failure("container.xml no encontrado");

                XDocument containerXml;
                using (var s = containerEntry.Open())
                    containerXml = XDocument.Load(s);

                // 2️⃣ OPF path
                var rootFileElement = containerXml
                    .Descendants()
                    .FirstOrDefault(e => e.Name.LocalName == "rootfile");

                if (rootFileElement == null)
                    return Result<ParsedOpf>.Failure("No se encontró el elemento rootfile en container.xml");

                var opfPath = rootFileElement.Attribute("full-path")?.Value;
                if (string.IsNullOrEmpty(opfPath))
                    return Result<ParsedOpf>.Failure("El atributo full-path del rootfile es inválido");

                var opfDir = Path.GetDirectoryName(opfPath)?.Replace("\\", "/") ?? "";

                // 3️⃣ OPF
                var opfEntry = zip.GetEntry(opfPath);
                if (opfEntry == null)
                    return Result<ParsedOpf>.Failure("OPF no encontrado");

                XDocument opfXml;
                using (var s = opfEntry.Open())
                    opfXml = XDocument.Load(s);

                XNamespace opfNs = opfXml.Root!.Name.Namespace;
                XNamespace dcNs = "http://purl.org/dc/elements/1.1/";

                // 4️⃣ Metadata
                var identifierEl = opfXml.Descendants(dcNs + "identifier").FirstOrDefault();
                var titleEl = opfXml.Descendants(dcNs + "title").FirstOrDefault();

                if (identifierEl == null || string.IsNullOrWhiteSpace(identifierEl.Value))
                    return Result<ParsedOpf>.Failure("Identificador (dc:identifier) no encontrado en OPF");

                if (titleEl == null || string.IsNullOrWhiteSpace(titleEl.Value))
                    return Result<ParsedOpf>.Failure("Título (dc:title) no encontrado en OPF");

                var identifier = identifierEl.Value;
                var title = titleEl.Value;
                var language = opfXml.Descendants(dcNs + "language").FirstOrDefault()?.Value ?? "und";

                var authors = opfXml.Descendants(dcNs + "creator")
                    .Select(x => x.Value)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                // 5️⃣ Manifest
                var items = opfXml
                    .Descendants(opfNs + "item")
                    .ToList();

                var manifest = new Dictionary<string, (string Href, string MediaType, string? Properties)>(StringComparer.OrdinalIgnoreCase);

                foreach (var i in items)
                {
                    var id = i.Attribute("id")?.Value;
                    var href = i.Attribute("href")?.Value;
                    var media = i.Attribute("media-type")?.Value;

                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(href) || string.IsNullOrEmpty(media))
                        continue;

                    manifest[id] = (href, media, i.Attribute("properties")?.Value);
                }

                // 6️⃣ Spine
                var spineIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                var spine = opfXml
                    .Descendants(opfNs + "itemref")
                    .Select(i =>
                    {
                        var idref = i.Attribute("idref")?.Value ?? "";
                        spineIds.Add(idref);

                        if (!manifest.TryGetValue(idref, out var item))
                            return null;

                        var href = string.IsNullOrEmpty(opfDir) ? item.Href : $"{opfDir}/{item.Href}";

                        return new OpfItem
                        {
                            Href = href,
                            MediaType = item.MediaType
                        };
                    })
                    .Where(x => x != null)
                    .Cast<OpfItem>()
                    .ToList();

                if (!spine.Any())
                    return Result<ParsedOpf>.Failure("El spine del OPF está vacío");

                // 7️⃣ Resources (manifest - spine)
                var resources = manifest
                    .Where(m => !spineIds.Contains(m.Key))
                    .Select(m =>
                    {
                        var href = string.IsNullOrEmpty(opfDir) ? m.Value.Href : $"{opfDir}/{m.Value.Href}";
                        return new OpfItem
                        {
                            Href = href,
                            MediaType = m.Value.MediaType,
                            Rel = m.Value.Properties?.Contains("nav") == true ? "contents" : null
                        };
                    })
                    .ToList();

                // 8️⃣ TOC (nav.xhtml or ncx)
                var toc = new List<OpfTocItem>();
                string? navPath = null;

                var navItem = manifest.Values
                    .FirstOrDefault(m => m.Properties?.Contains("nav") == true);

                if (navItem != default)
                {
                    navPath = string.IsNullOrEmpty(opfDir)
                        ? navItem.Href
                        : $"{opfDir}/{navItem.Href}";

                    var navEntry = zip.GetEntry(navPath);
                    if (navEntry != null)
                    {
                        XDocument navXml;
                        using (var s = navEntry.Open())
                            navXml = XDocument.Load(s);

                        XNamespace xhtml = "http://www.w3.org/1999/xhtml";

                        var nav = navXml
                            .Descendants(xhtml + "nav")
                            .FirstOrDefault(n =>
                                n.Attributes().Any(a => a.Value.Contains("toc")));

                        var ol = nav?.Element(xhtml + "ol");
                        if (ol != null)
                            toc = ParseTocOl(ol, xhtml);
                    }
                }
                else
                {
                    var ncxItem = manifest.Values
                        .FirstOrDefault(m => m.MediaType == "application/x-dtbncx+xml");

                    if (ncxItem != default)
                    {
                        var ncxPath = string.IsNullOrEmpty(opfDir)
                            ? ncxItem.Href
                            : $"{opfDir}/{ncxItem.Href}";

                        var ncxEntry = zip.GetEntry(ncxPath);
                        if (ncxEntry != null)
                        {
                            XDocument ncxXml;
                            using (var s = ncxEntry.Open())
                                ncxXml = XDocument.Load(s);

                            toc = ParseNcx(ncxXml);
                        }
                    }
                }

                var parsed = new ParsedOpf
                {
                    Identifier = identifier,
                    Title = title,
                    Language = language,
                    Authors = authors,
                    Spine = spine,
                    Resources = resources,
                    Toc = toc,
                    NavPath = navPath
                };

                return Result<ParsedOpf>.Success(parsed);
            }
            catch (InvalidDataException ex)
            {
                return Result<ParsedOpf>.Failure($"EPUB inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<ParsedOpf>.Failure($"Error al parsear EPUB: {ex.Message}");
            }
        }

        // 🔁 Recursivo para TOC jerárquico
        private static List<OpfTocItem> ParseTocOl(XElement ol, XNamespace xhtml)
        {
            return ol.Elements(xhtml + "li")
                .Select(li =>
                {
                    var link = li.Element(xhtml + "a");

                    var item = new OpfTocItem
                    {
                        Title = link?.Value.Trim() ?? "",
                        Href = link?.Attribute("href")?.Value ?? ""
                    };

                    var childOl = li.Element(xhtml + "ol");
                    if (childOl != null)
                        item.Children = ParseTocOl(childOl, xhtml);

                    return item;
                })
                .Where(i => !string.IsNullOrWhiteSpace(i.Href))
                .ToList();
        }
        private static List<OpfTocItem> ParseNcx(XDocument ncxXml)
        {
            XNamespace ncx = "http://www.daisy.org/z3986/2005/ncx/";

            return ncxXml
                .Descendants(ncx + "navPoint")
                .Select(p => new OpfTocItem
                {
                    Title = p.Element(ncx + "navLabel")?
                                .Element(ncx + "text")?
                                .Value ?? "",
                    Href = p.Element(ncx + "content")?
                                .Attribute("src")?
                                .Value ?? ""
                })
                .Where(t => !string.IsNullOrWhiteSpace(t.Href))
                .ToList();
        }

    }
}
