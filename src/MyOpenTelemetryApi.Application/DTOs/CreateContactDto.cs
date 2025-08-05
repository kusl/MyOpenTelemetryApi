// src/MyOpenTelemetryApi.Application/DTOs/CreateContactDto.cs
namespace MyOpenTelemetryApi.Application.DTOs;

public class CreateContactDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Nickname { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Notes { get; set; }

    public List<CreateEmailAddressDto> EmailAddresses { get; set; } = new();
    public List<CreatePhoneNumberDto> PhoneNumbers { get; set; } = new();
    public List<CreateAddressDto> Addresses { get; set; } = new();
    public List<Guid> GroupIds { get; set; } = new();
    public List<Guid> TagIds { get; set; } = new();
}