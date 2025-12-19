using System.IO.Compression;
using System.Xml.Linq;
using OpenBooks.Application.Services.Lector;

namespace OpenBooks.Infrastructure.Services.Lector
{
    public class EpubParser : IEpubParser
    {
        public ParsedOpf Parse(byte[] epubContent)
        {
            if (epubContent == null || epubContent.Length == 0)
                throw new InvalidDataException("El contenido del EPUB está vacío");

            using var ms = new MemoryStream(epubContent);
            using var zip = new ZipArchive(ms, ZipArchiveMode.Read);

            // 1️⃣ container.xml
            var containerEntry = zip.GetEntry("META-INF/container.xml")
                ?? throw new InvalidDataException("container.xml no encontrado");

            XDocument containerXml;
            using (var s = containerEntry.Open())
                containerXml = XDocument.Load(s);

            // 2️⃣ OPF path
            var rootFileElement = containerXml
                .Descendants()
                .First(e => e.Name.LocalName == "rootfile");

            var opfPath = rootFileElement.Attribute("full-path")!.Value;
            var opfDir = Path.GetDirectoryName(opfPath)?.Replace("\\", "/") ?? "";

            // 3️⃣ OPF
            var opfEntry = zip.GetEntry(opfPath)
                ?? throw new InvalidDataException("OPF no encontrado");

            XDocument opfXml;
            using (var s = opfEntry.Open())
                opfXml = XDocument.Load(s);

            XNamespace opfNs = opfXml.Root!.Name.Namespace;
            XNamespace dcNs = "http://purl.org/dc/elements/1.1/";

            // 4️⃣ Metadata
            var identifier = opfXml.Descendants(dcNs + "identifier").First().Value;
            var title = opfXml.Descendants(dcNs + "title").First().Value;
            var language = opfXml.Descendants(dcNs + "language").FirstOrDefault()?.Value ?? "und";

            var authors = opfXml.Descendants(dcNs + "creator")
                .Select(x => x.Value)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            // 5️⃣ Manifest
            var manifest = opfXml
                .Descendants(opfNs + "item")
                .ToDictionary(
                    i => i.Attribute("id")!.Value,
                    i => new
                    {
                        Href = i.Attribute("href")!.Value,
                        MediaType = i.Attribute("media-type")!.Value,
                        Properties = i.Attribute("properties")?.Value
                    });

            // 6️⃣ Spine
            var spineIds = new HashSet<string>();

            var spine = opfXml
                .Descendants(opfNs + "itemref")
                .Select(i =>
                {
                    var idref = i.Attribute("idref")!.Value;
                    spineIds.Add(idref);

                    var item = manifest[idref];

                    return new OpfItem
                    {
                        Href = item.Href,
                        MediaType = item.MediaType
                    };
                })
                .ToList();

            if (!spine.Any())
                throw new InvalidDataException("El spine del OPF está vacío");

            // 7️⃣ Resources (manifest - spine)
            var resources = manifest
                .Where(m => !spineIds.Contains(m.Key))
                .Select(m => new OpfItem
                {
                    Href = m.Value.Href,
                    MediaType = m.Value.MediaType,
                    Rel = m.Value.Properties?.Contains("nav") == true ? "contents" : null
                })
                .ToList();

            // 8️⃣ TOC (nav.xhtml)
            var toc = new List<OpfTocItem>();
            string? navPath = null;

            // EPUB 3 → nav.xhtml
            var navItem = manifest.Values
                .FirstOrDefault(m => m.Properties?.Contains("nav") == true);

            if (navItem != null)
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
                // EPUB 2 → toc.ncx
                var ncxItem = manifest.Values
                    .FirstOrDefault(m => m.MediaType == "application/x-dtbncx+xml");

                if (ncxItem != null)
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


            return new ParsedOpf
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
