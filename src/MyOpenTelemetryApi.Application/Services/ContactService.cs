// src/MyOpenTelemetryApi.Application/Services/ContactService.cs - Updated with telemetry
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Domain.Entities;
using MyOpenTelemetryApi.Domain.Interfaces;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;

namespace MyOpenTelemetryApi.Application.Services;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ContactService> _logger;
    private readonly ActivitySource _activitySource;
    private readonly Meter _meter;
    private readonly Counter<int> _contactsCreatedCounter;
    private readonly Counter<int> _contactsDeletedCounter;
    private readonly Counter<int> _searchCounter;
    private readonly Histogram<double> _searchDuration;

    public ContactService(IUnitOfWork unitOfWork, ILogger<ContactService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _activitySource = new ActivitySource("MyOpenTelemetryApi.ContactService");
        _meter = new Meter("MyOpenTelemetryApi.ContactService", "1.0.0");

        // Initialize metrics
        _contactsCreatedCounter = _meter.CreateCounter<int>("contacts.created", description: "Number of contacts created");
        _contactsDeletedCounter = _meter.CreateCounter<int>("contacts.deleted", description: "Number of contacts deleted");
        _searchCounter = _meter.CreateCounter<int>("contacts.searches", description: "Number of contact searches performed");
        _searchDuration = _meter.CreateHistogram<double>("contacts.search.duration", unit: "ms", description: "Duration of contact searches");
    }

    public async Task<ContactDto?> GetByIdAsync(Guid id)
    {
        using var activity = _activitySource.StartActivity("GetContactById", ActivityKind.Internal);
        activity?.SetTag("contact.id", id);

        _logger.LogInformation("Getting contact by ID: {ContactId}", id);

        var contact = await _unitOfWork.Contacts.GetByIdAsync(id);

        if (contact == null)
        {
            _logger.LogWarning("Contact not found: {ContactId}", id);
            activity?.SetStatus(ActivityStatusCode.Error, "Contact not found");
        }

        return contact == null ? null : MapToDto(contact);
    }

    public async Task<ContactDto?> GetWithDetailsAsync(Guid id)
    {
        using var activity = _activitySource.StartActivity("GetContactWithDetails", ActivityKind.Internal);
        activity?.SetTag("contact.id", id);

        _logger.LogInformation("Getting contact with details: {ContactId}", id);

        var contact = await _unitOfWork.Contacts.GetContactWithDetailsAsync(id);

        if (contact == null)
        {
            _logger.LogWarning("Contact not found: {ContactId}", id);
            activity?.SetStatus(ActivityStatusCode.Error, "Contact not found");
        }
        else
        {
            activity?.SetTag("email.count", contact.EmailAddresses.Count);
            activity?.SetTag("phone.count", contact.PhoneNumbers.Count);
            activity?.SetTag("address.count", contact.Addresses.Count);
        }

        return contact == null ? null : MapToDetailedDto(contact);
    }

    public async Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        using var activity = _activitySource.StartActivity("GetContactsPaginated", ActivityKind.Internal);
        activity?.SetTag("page.number", pageNumber);
        activity?.SetTag("page.size", pageSize);

        _logger.LogInformation("Getting paginated contacts: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);

        var allContacts = await _unitOfWork.Contacts.GetAllAsync();
        var totalCount = allContacts.Count();

        activity?.SetTag("total.count", totalCount);

        var pagedContacts = allContacts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToSummaryDto)
            .ToList();

        return new PaginatedResultDto<ContactSummaryDto>
        {
            Items = pagedContacts,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<List<ContactSummaryDto>> SearchAsync(string searchTerm)
    {
        using var activity = _activitySource.StartActivity("SearchContacts", ActivityKind.Internal);
        activity?.SetTag("search.term", searchTerm);

        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Searching contacts with term: {SearchTerm}", searchTerm);

        var contacts = await _unitOfWork.Contacts.SearchContactsAsync(searchTerm);
        var results = contacts.Select(MapToSummaryDto).ToList();

        stopwatch.Stop();

        activity?.SetTag("result.count", results.Count);
        _searchCounter.Add(1, new KeyValuePair<string, object?>("result.count", results.Count));
        _searchDuration.Record(stopwatch.ElapsedMilliseconds);

        _logger.LogInformation("Search completed: Found {Count} contacts in {Duration}ms", results.Count, stopwatch.ElapsedMilliseconds);

        return results;
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto)
    {
        using var activity = _activitySource.StartActivity("CreateContact", ActivityKind.Internal);

        _logger.LogInformation("Creating new contact: {FirstName} {LastName}", dto.FirstName, dto.LastName);

        try
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                Nickname = dto.Nickname,
                Company = dto.Company,
                JobTitle = dto.JobTitle,
                DateOfBirth = dto.DateOfBirth,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            activity?.SetTag("contact.id", contact.Id);
            activity?.SetTag("contact.company", contact.Company);

            // Add email addresses
            foreach (var emailDto in dto.EmailAddresses)
            {
                contact.EmailAddresses.Add(new EmailAddress
                {
                    Id = Guid.NewGuid(),
                    Email = emailDto.Email,
                    Type = Enum.Parse<EmailType>(emailDto.Type),
                    IsPrimary = emailDto.IsPrimary
                });
            }

            // Add phone numbers
            foreach (var phoneDto in dto.PhoneNumbers)
            {
                contact.PhoneNumbers.Add(new PhoneNumber
                {
                    Id = Guid.NewGuid(),
                    Number = phoneDto.Number,
                    Type = Enum.Parse<PhoneType>(phoneDto.Type),
                    IsPrimary = phoneDto.IsPrimary
                });
            }

            // Add addresses
            foreach (var addressDto in dto.Addresses)
            {
                contact.Addresses.Add(new Address
                {
                    Id = Guid.NewGuid(),
                    StreetLine1 = addressDto.StreetLine1,
                    StreetLine2 = addressDto.StreetLine2,
                    City = addressDto.City,
                    StateProvince = addressDto.StateProvince,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country,
                    Type = Enum.Parse<AddressType>(addressDto.Type),
                    IsPrimary = addressDto.IsPrimary
                });
            }

            // Add to groups
            foreach (var groupId in dto.GroupIds)
            {
                contact.ContactGroups.Add(new ContactGroup
                {
                    GroupId = groupId,
                    AddedAt = DateTime.UtcNow
                });
            }

            // Add tags
            foreach (var tagId in dto.TagIds)
            {
                contact.Tags.Add(new ContactTag
                {
                    TagId = tagId
                });
            }

            await _unitOfWork.Contacts.AddAsync(contact);
            await _unitOfWork.SaveChangesAsync();

            _contactsCreatedCounter.Add(1,
                new KeyValuePair<string, object?>("has.company", !string.IsNullOrEmpty(contact.Company)));

            _logger.LogInformation("Contact created successfully: {ContactId}", contact.Id);

            return MapToDto(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contact");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }

    public async Task<ContactDto?> UpdateAsync(Guid id, UpdateContactDto dto)
    {
        using var activity = _activitySource.StartActivity("UpdateContact", ActivityKind.Internal);
        activity?.SetTag("contact.id", id);

        _logger.LogInformation("Updating contact: {ContactId}", id);

        var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
        if (contact == null)
        {
            _logger.LogWarning("Contact not found for update: {ContactId}", id);
            activity?.SetStatus(ActivityStatusCode.Error, "Contact not found");
            return null;
        }

        contact.FirstName = dto.FirstName;
        contact.LastName = dto.LastName;
        contact.MiddleName = dto.MiddleName;
        contact.Nickname = dto.Nickname;
        contact.Company = dto.Company;
        contact.JobTitle = dto.JobTitle;
        contact.DateOfBirth = dto.DateOfBirth;
        contact.Notes = dto.Notes;
        contact.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Contacts.UpdateAsync(contact);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Contact updated successfully: {ContactId}", id);

        return MapToDto(contact);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var activity = _activitySource.StartActivity("DeleteContact", ActivityKind.Internal);
        activity?.SetTag("contact.id", id);

        _logger.LogInformation("Deleting contact: {ContactId}", id);

        var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
        if (contact == null)
        {
            _logger.LogWarning("Contact not found for deletion: {ContactId}", id);
            activity?.SetStatus(ActivityStatusCode.Error, "Contact not found");
            return false;
        }

        await _unitOfWork.Contacts.DeleteAsync(contact);
        await _unitOfWork.SaveChangesAsync();

        _contactsDeletedCounter.Add(1);
        _logger.LogInformation("Contact deleted successfully: {ContactId}", id);

        return true;
    }

    public async Task<List<ContactSummaryDto>> GetByGroupAsync(Guid groupId)
    {
        using var activity = _activitySource.StartActivity("GetContactsByGroup", ActivityKind.Internal);
        activity?.SetTag("group.id", groupId);

        _logger.LogInformation("Getting contacts by group: {GroupId}", groupId);

        var contacts = await _unitOfWork.Contacts.GetContactsByGroupAsync(groupId);
        var results = contacts.Select(MapToSummaryDto).ToList();

        activity?.SetTag("result.count", results.Count);
        _logger.LogInformation("Found {Count} contacts in group {GroupId}", results.Count, groupId);

        return results;
    }

    public async Task<List<ContactSummaryDto>> GetByTagAsync(Guid tagId)
    {
        using var activity = _activitySource.StartActivity("GetContactsByTag", ActivityKind.Internal);
        activity?.SetTag("tag.id", tagId);

        _logger.LogInformation("Getting contacts by tag: {TagId}", tagId);

        var contacts = await _unitOfWork.Contacts.GetContactsByTagAsync(tagId);
        var results = contacts.Select(MapToSummaryDto).ToList();

        activity?.SetTag("result.count", results.Count);
        _logger.LogInformation("Found {Count} contacts with tag {TagId}", results.Count, tagId);

        return results;
    }

    private static ContactDto MapToDto(Contact contact)
    {
        return new ContactDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            MiddleName = contact.MiddleName,
            Nickname = contact.Nickname,
            Company = contact.Company,
            JobTitle = contact.JobTitle,
            DateOfBirth = contact.DateOfBirth,
            Notes = contact.Notes,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    private ContactDto MapToDetailedDto(Contact contact)
    {
        var dto = MapToDto(contact);

        dto.EmailAddresses = contact.EmailAddresses.Select(e => new EmailAddressDto
        {
            Id = e.Id,
            Email = e.Email,
            Type = e.Type.ToString(),
            IsPrimary = e.IsPrimary
        }).ToList();

        dto.PhoneNumbers = contact.PhoneNumbers.Select(p => new PhoneNumberDto
        {
            Id = p.Id,
            Number = p.Number,
            Type = p.Type.ToString(),
            IsPrimary = p.IsPrimary
        }).ToList();

        dto.Addresses = contact.Addresses.Select(a => new AddressDto
        {
            Id = a.Id,
            StreetLine1 = a.StreetLine1,
            StreetLine2 = a.StreetLine2,
            City = a.City,
            StateProvince = a.StateProvince,
            PostalCode = a.PostalCode,
            Country = a.Country,
            Type = a.Type.ToString(),
            IsPrimary = a.IsPrimary
        }).ToList();

        dto.Groups = contact.ContactGroups.Select(cg => new GroupDto
        {
            Id = cg.Group.Id,
            Name = cg.Group.Name,
            Description = cg.Group.Description,
            CreatedAt = cg.Group.CreatedAt
        }).ToList();

        dto.Tags = contact.Tags.Select(ct => new TagDto
        {
            Id = ct.Tag.Id,
            Name = ct.Tag.Name,
            ColorHex = ct.Tag.ColorHex
        }).ToList();

        return dto;
    }

    private ContactSummaryDto MapToSummaryDto(Contact contact)
    {
        return new ContactSummaryDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Company = contact.Company,
            PrimaryEmail = contact.EmailAddresses.FirstOrDefault(e => e.IsPrimary)?.Email
                          ?? contact.EmailAddresses.FirstOrDefault()?.Email,
            PrimaryPhone = contact.PhoneNumbers.FirstOrDefault(p => p.IsPrimary)?.Number
                          ?? contact.PhoneNumbers.FirstOrDefault()?.Number
        };
    }
}