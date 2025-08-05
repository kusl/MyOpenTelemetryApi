// src/MyOpenTelemetryApi.Domain/Entities/ContactTag.cs (Join table)
namespace MyOpenTelemetryApi.Domain.Entities;

public class ContactTag
{
    public Guid ContactId { get; set; }
    public Guid TagId { get; set; }

    // Navigation properties
    public Contact Contact { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
