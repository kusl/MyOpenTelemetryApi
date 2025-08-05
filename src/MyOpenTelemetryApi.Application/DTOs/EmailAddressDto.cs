// src/MyOpenTelemetryApi.Application/DTOs/EmailAddressDto.cs
namespace MyOpenTelemetryApi.Application.DTOs;

public class EmailAddressDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public class CreateEmailAddressDto
{
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
