using OpenBooks.Application.DTOs.Lector;

namespace OpenBooks.Api.Dtos.Lector
{
    public class UpdateProgressRequest
    {
        public LocatorDto CurrentLocator { get; set; } = new LocatorDto();
        public DateTime ClientTimestamp { get; set; } = DateTime.UtcNow;
    }
    public class CreateMarcadorRequest
    {
        public string? Label { get; set; }
        public LocatorDto Locator { get; set; } = new LocatorDto();
        public string? Metadata { get; set; }
    }

    public class CreateResaltadoRequest
    {
        public LocatorDto LocatorStart { get; set; } = new LocatorDto();
        public LocatorDto LocatorEnd { get; set; } = new LocatorDto();
        public string SelectedText { get; set; } = string.Empty;
        public string? Context { get; set; }
        public string? Note { get; set; }
        public string Color { get; set; } = "#ffeb3b";
        public string Type { get; set; } = "highlight";
    }

    public class UpdateResaltadoRequest
    {
        public string? Note { get; set; }
        public string? Color { get; set; }
        public string? Type { get; set; }
    }
}
