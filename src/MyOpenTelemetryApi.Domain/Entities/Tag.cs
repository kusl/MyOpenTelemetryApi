// src/MyOpenTelemetryApi.Domain/Entities/Tag.cs
namespace MyOpenTelemetryApi.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ColorHex { get; set; }

    // Navigation property
    public List<ContactTag> ContactTags { get; set; } = [];
}
