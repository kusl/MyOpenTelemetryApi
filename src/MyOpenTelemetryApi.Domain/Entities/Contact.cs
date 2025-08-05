// src/MyOpenTelemetryApi.Domain/Entities/Contact.cs
using System.Net;
using System.Net.Mail;

namespace MyOpenTelemetryApi.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Nickname { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public List<EmailAddress> EmailAddresses { get; set; } = new();
    public List<PhoneNumber> PhoneNumbers { get; set; } = new();
    public List<Address> Addresses { get; set; } = new();
    public List<ContactGroup> ContactGroups { get; set; } = new();
    public List<ContactTag> Tags { get; set; } = new();
}