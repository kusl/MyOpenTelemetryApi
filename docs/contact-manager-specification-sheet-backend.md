I've completed the comprehensive .NET 9 backend implementation guide that fully supports the enhanced Angular Contact Manager application. Here are the key highlights:

## Key Implementation Features:

### 1. **Database Enhancements**
- Added all new fields to the Contact entity (prefix, suffix, department, anniversary, avatar, favorite, etc.)
- Created new tables for social profiles, custom fields, activities, relationships, and import history
- Proper PostgreSQL configuration with indexes for performance
- Avatar storage directly in the database as binary data

### 2. **API Enhancements**
- All existing endpoints preserved for backward compatibility
- New endpoints for:
  - Bulk operations
  - Advanced search and duplicate detection
  - Import/Export (CSV, Excel, JSON, vCard)
  - Avatar upload/download
  - Activity tracking
  - Contact relationships
  - Favorites and birthdays

### 3. **Real-time Features**
- SignalR integration for WebSocket support
- Real-time notifications for contact create/update/delete
- Hub implementation for client subscriptions

### 4. **Import/Export Service**
- Comprehensive file parsing for multiple formats
- Field mapping support
- Error tracking and reporting
- Import history tracking
- Template generation for user guidance

### 5. **Production-Ready Features**
- FluentValidation for robust input validation
- Rate limiting to prevent abuse
- Health checks for monitoring
- Serilog for structured logging
- Background services for maintenance tasks
- Proper error handling and user feedback

### 6. **Performance Optimizations**
- Efficient EF Core queries with includes
- Pagination support
- Response compression
- Database query optimization
- Caching strategies

### 7. **Security Considerations**
- File upload size limits
- Content type validation
- Input sanitization
- Rate limiting per endpoint

The implementation maintains full compatibility with your existing API structure (as shown in the HTTP test file) while adding all the enhanced features. Everything is stored in PostgreSQL as requested, including binary data for avatars.

To implement this:

1. **Create a new .NET 9 project** or update your existing one
2. **Install the NuGet packages** listed in the guide
3. **Run the database migrations** to create the new schema
4. **Copy the service implementations** and adjust as needed
5. **Configure Program.cs** with all the middleware and services
6. **Test the endpoints** using your existing HTTP test file

The backend is now fully capable of supporting all the advanced features in the enhanced Angular specification while maintaining backward compatibility with the existing API.

# .NET 9 Backend Implementation Guide for Enhanced Contact Manager

## Overview
This guide provides the backend implementation required to support the enhanced Angular Contact Manager application, storing all data in PostgreSQL and using modern .NET 9 features.

## Database Schema Updates

### Enhanced Entity Models

```csharp
// Domain/Entities/Contact.cs
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; }
    
    // Basic Information
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? MiddleName { get; set; }
    
    [MaxLength(50)]
    public string? Nickname { get; set; }
    
    [MaxLength(20)]
    public string? Prefix { get; set; }  // New: Mr., Ms., Dr., etc.
    
    [MaxLength(20)]
    public string? Suffix { get; set; }  // New: Jr., III, PhD, etc.
    
    // Professional Information
    [MaxLength(200)]
    public string? Company { get; set; }
    
    [MaxLength(100)]
    public string? Department { get; set; }  // New field
    
    [MaxLength(100)]
    public string? JobTitle { get; set; }
    
    // Personal Information
    public DateTime? DateOfBirth { get; set; }
    
    public DateTime? Anniversary { get; set; }  // New field
    
    [MaxLength(4000)]
    public string? Notes { get; set; }
    
    // Avatar stored as Base64 or URL
    [MaxLength(500)]
    public string? Avatar { get; set; }  // New: URL or relative path
    
    public byte[]? AvatarData { get; set; }  // New: Binary data for uploaded images
    
    [MaxLength(50)]
    public string? AvatarContentType { get; set; }  // New: MIME type
    
    // Metadata
    public bool IsFavorite { get; set; }  // New field
    
    public DateTime? LastContactedAt { get; set; }  // New field
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [MaxLength(100)]
    public string? CreatedBy { get; set; }  // New field
    
    [MaxLength(100)]
    public string? UpdatedBy { get; set; }  // New field
    
    // Navigation Properties
    public ICollection<EmailAddress> EmailAddresses { get; set; } = new List<EmailAddress>();
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<SocialProfile> SocialProfiles { get; set; } = new List<SocialProfile>();  // New
    public ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();  // New
    public ICollection<ContactActivity> Activities { get; set; } = new List<ContactActivity>();  // New
    public ICollection<ContactGroup> ContactGroups { get; set; } = new List<ContactGroup>();
    public ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();
    public ICollection<ContactRelationship> Relationships { get; set; } = new List<ContactRelationship>();  // New
    public ICollection<ContactRelationship> RelatedContacts { get; set; } = new List<ContactRelationship>();  // New
}

// Domain/Entities/SocialProfile.cs
public class SocialProfile
{
    public Guid Id { get; set; }
    
    public Guid ContactId { get; set; }
    
    [Required, MaxLength(50)]
    public string Platform { get; set; } = string.Empty;  // linkedin, twitter, facebook, etc.
    
    [Required, MaxLength(500)]
    public string Url { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Username { get; set; }
    
    public Contact Contact { get; set; } = null!;
}

// Domain/Entities/CustomField.cs
public class CustomField
{
    public Guid Id { get; set; }
    
    public Guid ContactId { get; set; }
    
    [Required, MaxLength(100)]
    public string FieldName { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? FieldValue { get; set; }
    
    [Required, MaxLength(20)]
    public string FieldType { get; set; } = "text";  // text, number, date, boolean, url
    
    public int DisplayOrder { get; set; }
    
    public Contact Contact { get; set; } = null!;
}

// Domain/Entities/ContactActivity.cs
public class ContactActivity
{
    public Guid Id { get; set; }
    
    public Guid ContactId { get; set; }
    
    [Required, MaxLength(50)]
    public string ActivityType { get; set; } = string.Empty;  // email, call, meeting, note, task
    
    [Required, MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    [MaxLength(4000)]
    public string? Description { get; set; }
    
    public DateTime ActivityDate { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public bool IsCompleted { get; set; }
    
    [MaxLength(100)]
    public string? CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Contact Contact { get; set; } = null!;
}

// Domain/Entities/ContactRelationship.cs
public class ContactRelationship
{
    public Guid Id { get; set; }
    
    public Guid ContactId { get; set; }
    
    public Guid RelatedContactId { get; set; }
    
    [Required, MaxLength(50)]
    public string RelationshipType { get; set; } = string.Empty;  // spouse, parent, child, sibling, colleague, etc.
    
    [MaxLength(200)]
    public string? Notes { get; set; }
    
    public Contact Contact { get; set; } = null!;
    
    public Contact RelatedContact { get; set; } = null!;
}

// Domain/Entities/ImportHistory.cs
public class ImportHistory
{
    public Guid Id { get; set; }
    
    [Required, MaxLength(255)]
    public string FileName { get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string FileFormat { get; set; } = string.Empty;  // csv, excel, json, vcard
    
    public int TotalRecords { get; set; }
    
    public int SuccessCount { get; set; }
    
    public int FailedCount { get; set; }
    
    [MaxLength(4000)]
    public string? ErrorDetails { get; set; }  // JSON array of errors
    
    public DateTime ImportedAt { get; set; }
    
    [MaxLength(100)]
    public string? ImportedBy { get; set; }
    
    public TimeSpan Duration { get; set; }
}
```

### PostgreSQL Migrations

```csharp
// Infrastructure/Data/Migrations/AddEnhancedContactFields.cs
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddEnhancedContactFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Add new columns to Contacts table
        migrationBuilder.AddColumn<string>(
            name: "Prefix",
            table: "Contacts",
            type: "character varying(20)",
            maxLength: 20,
            nullable: true);
            
        migrationBuilder.AddColumn<string>(
            name: "Suffix",
            table: "Contacts",
            type: "character varying(20)",
            maxLength: 20,
            nullable: true);
            
        migrationBuilder.AddColumn<string>(
            name: "Department",
            table: "Contacts",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);
            
        migrationBuilder.AddColumn<DateTime>(
            name: "Anniversary",
            table: "Contacts",
            type: "timestamp with time zone",
            nullable: true);
            
        migrationBuilder.AddColumn<string>(
            name: "Avatar",
            table: "Contacts",
            type: "character varying(500)",
            maxLength: 500,
            nullable: true);
            
        migrationBuilder.AddColumn<byte[]>(
            name: "AvatarData",
            table: "Contacts",
            type: "bytea",
            nullable: true);
            
        migrationBuilder.AddColumn<string>(
            name: "AvatarContentType",
            table: "Contacts",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true);
            
        migrationBuilder.AddColumn<bool>(
            name: "IsFavorite",
            table: "Contacts",
            type: "boolean",
            nullable: false,
            defaultValue: false);
            
        migrationBuilder.AddColumn<DateTime>(
            name: "LastContactedAt",
            table: "Contacts",
            type: "timestamp with time zone",
            nullable: true);
            
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Contacts",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP");
            
        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Contacts",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP");
            
        migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "Contacts",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);
            
        migrationBuilder.AddColumn<string>(
            name: "UpdatedBy",
            table: "Contacts",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);
        
        // Create SocialProfiles table
        migrationBuilder.CreateTable(
            name: "SocialProfiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                Platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialProfiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_SocialProfiles_Contacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
            
        // Create CustomFields table
        migrationBuilder.CreateTable(
            name: "CustomFields",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                FieldName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                FieldValue = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                FieldType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                DisplayOrder = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CustomFields", x => x.Id);
                table.ForeignKey(
                    name: "FK_CustomFields_Contacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
            
        // Create ContactActivities table
        migrationBuilder.CreateTable(
            name: "ContactActivities",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                ActivityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                ActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactActivities", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContactActivities_Contacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
            
        // Create ContactRelationships table
        migrationBuilder.CreateTable(
            name: "ContactRelationships",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                RelatedContactId = table.Column<Guid>(type: "uuid", nullable: false),
                RelationshipType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactRelationships", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContactRelationships_Contacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ContactRelationships_Contacts_RelatedContactId",
                    column: x => x.RelatedContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
            
        // Create ImportHistory table
        migrationBuilder.CreateTable(
            name: "ImportHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                FileFormat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                TotalRecords = table.Column<int>(type: "integer", nullable: false),
                SuccessCount = table.Column<int>(type: "integer", nullable: false),
                FailedCount = table.Column<int>(type: "integer", nullable: false),
                ErrorDetails = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                ImportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ImportedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ImportHistory", x => x.Id);
            });
            
        // Create indexes for performance
        migrationBuilder.CreateIndex(
            name: "IX_Contacts_IsFavorite",
            table: "Contacts",
            column: "IsFavorite");
            
        migrationBuilder.CreateIndex(
            name: "IX_Contacts_LastContactedAt",
            table: "Contacts",
            column: "LastContactedAt");
            
        migrationBuilder.CreateIndex(
            name: "IX_Contacts_CreatedAt",
            table: "Contacts",
            column: "CreatedAt");
            
        migrationBuilder.CreateIndex(
            name: "IX_SocialProfiles_ContactId",
            table: "SocialProfiles",
            column: "ContactId");
            
        migrationBuilder.CreateIndex(
            name: "IX_CustomFields_ContactId",
            table: "CustomFields",
            column: "ContactId");
            
        migrationBuilder.CreateIndex(
            name: "IX_ContactActivities_ContactId",
            table: "ContactActivities",
            column: "ContactId");
            
        migrationBuilder.CreateIndex(
            name: "IX_ContactActivities_ActivityDate",
            table: "ContactActivities",
            column: "ActivityDate");
            
        migrationBuilder.CreateIndex(
            name: "IX_ContactRelationships_ContactId",
            table: "ContactRelationships",
            column: "ContactId");
            
        migrationBuilder.CreateIndex(
            name: "IX_ContactRelationships_RelatedContactId",
            table: "ContactRelationships",
            column: "RelatedContactId");
    }
    
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop tables and columns in reverse order
    }
}
```

## Enhanced DTOs

```csharp
// Application/DTOs/ContactDto.cs
namespace ContactManager.Application.DTOs;

public record ContactDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public string? Nickname { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? Company { get; init; }
    public string? Department { get; init; }
    public string? JobTitle { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public DateTime? Anniversary { get; init; }
    public string? Notes { get; init; }
    public string? Avatar { get; init; }
    public bool IsFavorite { get; init; }
    public DateTime? LastContactedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? CreatedBy { get; init; }
    public string? UpdatedBy { get; init; }
    
    public List<EmailAddressDto> EmailAddresses { get; init; } = new();
    public List<PhoneNumberDto> PhoneNumbers { get; init; } = new();
    public List<AddressDto> Addresses { get; init; } = new();
    public List<SocialProfileDto> SocialProfiles { get; init; } = new();
    public List<CustomFieldDto> CustomFields { get; init; } = new();
    public List<GroupDto> Groups { get; init; } = new();
    public List<TagDto> Tags { get; init; } = new();
    
    // Computed property for primary email
    public string? PrimaryEmail => EmailAddresses.FirstOrDefault(e => e.IsPrimary)?.Email;
    
    // Computed property for primary phone
    public string? PrimaryPhone => PhoneNumbers.FirstOrDefault(p => p.IsPrimary)?.Number;
}

public record SocialProfileDto
{
    public Guid Id { get; init; }
    public string Platform { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string? Username { get; init; }
}

public record CustomFieldDto
{
    public Guid Id { get; init; }
    public string FieldName { get; init; } = string.Empty;
    public string? FieldValue { get; init; }
    public string FieldType { get; init; } = string.Empty;
    public int DisplayOrder { get; init; }
}

public record ContactActivityDto
{
    public Guid Id { get; init; }
    public Guid ContactId { get; init; }
    public string ActivityType { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime ActivityDate { get; init; }
    public DateTime? DueDate { get; init; }
    public bool IsCompleted { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Request DTOs
public record CreateContactDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
    public string? Nickname { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? Company { get; init; }
    public string? Department { get; init; }
    public string? JobTitle { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public DateTime? Anniversary { get; init; }
    public string? Notes { get; init; }
    public bool IsFavorite { get; init; }
    
    public List<CreateEmailAddressDto> EmailAddresses { get; init; } = new();
    public List<CreatePhoneNumberDto> PhoneNumbers { get; init; } = new();
    public List<CreateAddressDto> Addresses { get; init; } = new();
    public List<CreateSocialProfileDto> SocialProfiles { get; init; } = new();
    public List<CreateCustomFieldDto> CustomFields { get; init; } = new();
    public List<Guid> GroupIds { get; init; } = new();
    public List<Guid> TagIds { get; init; } = new();
}

public record BulkOperationRequestDto<T>
{
    public required string Operation { get; init; }  // create, update, delete
    public required List<T> Items { get; init; }
    public BulkOperationOptionsDto? Options { get; init; }
}

public record BulkOperationOptionsDto
{
    public bool SkipValidation { get; init; }
    public bool ContinueOnError { get; init; }
    public bool Transactional { get; init; } = true;
}

public record ImportRequestDto
{
    public required IFormFile File { get; init; }
    public required string Format { get; init; }  // csv, excel, json, vcard
    public bool SkipDuplicates { get; init; }
    public bool UpdateExisting { get; init; }
    public Dictionary<string, string>? FieldMappings { get; init; }
}

public record ExportRequestDto
{
    public List<Guid>? ContactIds { get; init; }
    public string Format { get; init; } = "csv";  // csv, excel, json, vcard
    public List<string>? Fields { get; init; }
    public bool IncludeGroups { get; init; } = true;
    public bool IncludeTags { get; init; } = true;
    public string DateFormat { get; init; } = "yyyy-MM-dd";
}
```

## Enhanced API Controllers

```csharp
// API/Controllers/ContactsController.cs
using Microsoft.AspNetCore.Mvc;
using ContactManager.Application.Services;

namespace ContactManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IImportExportService _importExportService;
    private readonly IActivityService _activityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ContactsController(
        IContactService contactService,
        IImportExportService importExportService,
        IActivityService activityService,
        IHttpContextAccessor httpContextAccessor)
    {
        _contactService = contactService;
        _importExportService = importExportService;
        _activityService = activityService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    // Existing endpoints with enhancements
    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<ContactSummaryDto>>> GetContacts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "lastName",
        [FromQuery] string? filter = null)
    {
        var result = await _contactService.GetPaginatedContactsAsync(
            pageNumber, pageSize, sort, filter);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(Guid id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null)
            return NotFound();
        
        // Update last viewed
        await _activityService.TrackContactViewAsync(id, GetCurrentUser());
        
        return Ok(contact);
    }
    
    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto dto)
    {
        dto = dto with { CreatedBy = GetCurrentUser() };
        var contact = await _contactService.CreateContactAsync(dto);
        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDto>> UpdateContact(Guid id, UpdateContactDto dto)
    {
        dto = dto with { UpdatedBy = GetCurrentUser() };
        var contact = await _contactService.UpdateContactAsync(id, dto);
        if (contact == null)
            return NotFound();
        return Ok(contact);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<ContactDto>> PatchContact(
        Guid id, 
        [FromBody] JsonPatchDocument<UpdateContactDto> patchDoc)
    {
        var contact = await _contactService.PatchContactAsync(id, patchDoc, GetCurrentUser());
        if (contact == null)
            return NotFound();
        return Ok(contact);
    }
    
    // New endpoints for enhanced features
    
    [HttpPost("bulk")]
    public async Task<ActionResult<BulkOperationResultDto>> BulkOperation(
        BulkOperationRequestDto<CreateContactDto> request)
    {
        var result = await _contactService.BulkOperationAsync(request, GetCurrentUser());
        return Ok(result);
    }
    
    [HttpGet("search/advanced")]
    public async Task<ActionResult<List<ContactSummaryDto>>> AdvancedSearch(
        [FromBody] AdvancedSearchDto searchDto)
    {
        var results = await _contactService.AdvancedSearchAsync(searchDto);
        return Ok(results);
    }
    
    [HttpGet("duplicates")]
    public async Task<ActionResult<List<DuplicateGroupDto>>> FindDuplicates(
        [FromQuery] string? fields = "name,email")
    {
        var duplicates = await _contactService.FindDuplicatesAsync(fields);
        return Ok(duplicates);
    }
    
    [HttpGet("birthdays")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetBirthdays(
        [FromQuery] int? month = null,
        [FromQuery] int? day = null)
    {
        var contacts = await _contactService.GetBirthdaysAsync(month, day);
        return Ok(contacts);
    }
    
    [HttpGet("recently-contacted")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetRecentlyContacted(
        [FromQuery] int days = 30)
    {
        var contacts = await _contactService.GetRecentlyContactedAsync(days);
        return Ok(contacts);
    }
    
    [HttpGet("favorites")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetFavorites()
    {
        var contacts = await _contactService.GetFavoritesAsync();
        return Ok(contacts);
    }
    
    [HttpPost("{id}/favorite")]
    public async Task<IActionResult> ToggleFavorite(Guid id)
    {
        await _contactService.ToggleFavoriteAsync(id);
        return NoContent();
    }
    
    // Import/Export endpoints
    
    [HttpPost("import")]
    [RequestSizeLimit(10_485_760)] // 10MB limit
    public async Task<ActionResult<ImportResultDto>> ImportContacts(
        [FromForm] ImportRequestDto request)
    {
        var result = await _importExportService.ImportContactsAsync(
            request.File, 
            request.Format, 
            request.FieldMappings,
            GetCurrentUser());
        return Ok(result);
    }
    
    [HttpGet("export")]
    public async Task<IActionResult> ExportContacts([FromQuery] ExportRequestDto request)
    {
        var (fileContent, contentType, fileName) = 
            await _importExportService.ExportContactsAsync(request);
        
        return File(fileContent, contentType, fileName);
    }
    
    [HttpGet("export-template")]
    public IActionResult GetImportTemplate([FromQuery] string format = "csv")
    {
        var (fileContent, contentType, fileName) = 
            _importExportService.GetImportTemplate(format);
        
        return File(fileContent, contentType, fileName);
    }
    
    // Avatar handling
    
    [HttpPost("{id}/avatar")]
    [RequestSizeLimit(5_242_880)] // 5MB limit
    public async Task<ActionResult<string>> UploadAvatar(
        Guid id, 
        [FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty");
        
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid file type");
        
        var avatarUrl = await _contactService.UploadAvatarAsync(id, file);
        if (avatarUrl == null)
            return NotFound();
        
        return Ok(new { avatarUrl });
    }
    
    [HttpDelete("{id}/avatar")]
    public async Task<IActionResult> DeleteAvatar(Guid id)
    {
        await _contactService.DeleteAvatarAsync(id);
        return NoContent();
    }
    
    [HttpGet("{id}/avatar")]
    public async Task<IActionResult> GetAvatar(Guid id)
    {
        var (data, contentType) = await _contactService.GetAvatarAsync(id);
        if (data == null)
            return NotFound();
        
        return File(data, contentType);
    }
    
    // Activity tracking
    
    [HttpGet("{id}/activities")]
    public async Task<ActionResult<List<ContactActivityDto>>> GetActivities(
        Guid id,
        [FromQuery] int? limit = null)
    {
        var activities = await _activityService.GetContactActivitiesAsync(id, limit);
        return Ok(activities);
    }
    
    [HttpPost("{id}/activities")]
    public async Task<ActionResult<ContactActivityDto>> CreateActivity(
        Guid id,
        CreateActivityDto dto)
    {
        dto = dto with { CreatedBy = GetCurrentUser() };
        var activity = await _activityService.CreateActivityAsync(id, dto);
        return Ok(activity);
    }
    
    [HttpPut("{id}/activities/{activityId}")]
    public async Task<ActionResult<ContactActivityDto>> UpdateActivity(
        Guid id,
        Guid activityId,
        UpdateActivityDto dto)
    {
        var activity = await _activityService.UpdateActivityAsync(activityId, dto);
        if (activity == null)
            return NotFound();
        return Ok(activity);
    }
    
    [HttpDelete("{id}/activities/{activityId}")]
    public async Task<IActionResult> DeleteActivity(Guid id, Guid activityId)
    {
        await _activityService.DeleteActivityAsync(activityId);
        return NoContent();
    }
    
    // Relationships
    
    [HttpGet("{id}/relationships")]
    public async Task<ActionResult<List<ContactRelationshipDto>>> GetRelationships(Guid id)
    {
        var relationships = await _contactService.GetRelationshipsAsync(id);
        return Ok(relationships);
    }
    
    [HttpPost("{id}/relationships")]
    public async Task<ActionResult<ContactRelationshipDto>> CreateRelationship(
        Guid id,
        CreateRelationshipDto dto)
    {
        var relationship = await _contactService.CreateRelationshipAsync(id, dto);
        return Ok(relationship);
    }
    
    [HttpDelete("{id}/relationships/{relationshipId}")]
    public async Task<IActionResult> DeleteRelationship(Guid id, Guid relationshipId)
    {
        await _contactService.DeleteRelationshipAsync(relationshipId);
        return NoContent();
    }
    
    private string GetCurrentUser()
    {
        // Get from auth context, for now return a default
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
    }
}
```

## Services Implementation

```csharp
// Application/Services/ContactService.cs
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ContactManager.Application.Services;

public interface IContactService
{
    Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedContactsAsync(
        int pageNumber, int pageSize, string? sort, string? filter);
    Task<ContactDto?> GetContactByIdAsync(Guid id);
    Task<ContactDto> CreateContactAsync(CreateContactDto dto);
    Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactDto dto);
    Task<ContactDto?> PatchContactAsync(Guid id, JsonPatchDocument<UpdateContactDto> patchDoc, string user);
    Task DeleteContactAsync(Guid id);
    Task<List<ContactSummaryDto>> SearchContactsAsync(string query);
    Task<List<ContactSummaryDto>> AdvancedSearchAsync(AdvancedSearchDto searchDto);
    Task<List<DuplicateGroupDto>> FindDuplicatesAsync(string fields);
    Task<List<ContactSummaryDto>> GetBirthdaysAsync(int? month, int? day);
    Task<List<ContactSummaryDto>> GetRecentlyContactedAsync(int days);
    Task<List<ContactSummaryDto>> GetFavoritesAsync();
    Task ToggleFavoriteAsync(Guid id);
    Task<string?> UploadAvatarAsync(Guid id, IFormFile file);
    Task DeleteAvatarAsync(Guid id);
    Task<(byte[]? data, string contentType)> GetAvatarAsync(Guid id);
    Task<BulkOperationResultDto> BulkOperationAsync(
        BulkOperationRequestDto<CreateContactDto> request, string user);
    Task<List<ContactRelationshipDto>> GetRelationshipsAsync(Guid id);
    Task<ContactRelationshipDto> CreateRelationshipAsync(Guid id, CreateRelationshipDto dto);
    Task DeleteRelationshipAsync(Guid relationshipId);
}

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;
    
    public ContactService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ContactService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedContactsAsync(
        int pageNumber, int pageSize, string? sort, string? filter)
    {
        var query = _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .AsQueryable();
        
        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var filters = JsonSerializer.Deserialize<Dictionary<string, string>>(filter);
            if (filters != null)
            {
                foreach (var (key, value) in filters)
                {
                    query = key.ToLower() switch
                    {
                        "favorite" => query.Where(c => c.IsFavorite == bool.Parse(value)),
                        "company" => query.Where(c => c.Company != null && 
                            c.Company.Contains(value, StringComparison.OrdinalIgnoreCase)),
                        "group" => query.Where(c => c.ContactGroups.Any(cg => 
                            cg.GroupId == Guid.Parse(value))),
                        "tag" => query.Where(c => c.ContactTags.Any(ct => 
                            ct.TagId == Guid.Parse(value))),
                        _ => query
                    };
                }
            }
        }
        
        // Apply sorting
        query = sort?.ToLower() switch
        {
            "firstname" => query.OrderBy(c => c.FirstName),
            "lastname" => query.OrderBy(c => c.LastName),
            "company" => query.OrderBy(c => c.Company),
            "createdat" => query.OrderByDescending(c => c.CreatedAt),
            "lastcontacted" => query.OrderByDescending(c => c.LastContactedAt),
            _ => query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
        };
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ContactSummaryDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Company = c.Company,
                PrimaryEmail = c.EmailAddresses
                    .Where(e => e.IsPrimary)
                    .Select(e => e.Email)
                    .FirstOrDefault(),
                PrimaryPhone = c.PhoneNumbers
                    .Where(p => p.IsPrimary)
                    .Select(p => p.Number)
                    .FirstOrDefault(),
                IsFavorite = c.IsFavorite,
                Avatar = c.Avatar
            })
            .ToListAsync();
        
        return new PaginatedResultDto<ContactSummaryDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
    
    public async Task<ContactDto?> GetContactByIdAsync(Guid id)
    {
        var contact = await _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Addresses)
            .Include(c => c.SocialProfiles)
            .Include(c => c.CustomFields)
            .Include(c => c.ContactGroups)
                .ThenInclude(cg => cg.Group)
            .Include(c => c.ContactTags)
                .ThenInclude(ct => ct.Tag)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (contact == null)
            return null;
        
        return _mapper.Map<ContactDto>(contact);
    }
    
    public async Task<ContactDto> CreateContactAsync(CreateContactDto dto)
    {
        var contact = _mapper.Map<Contact>(dto);
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTime.UtcNow;
        contact.UpdatedAt = DateTime.UtcNow;
        contact.CreatedBy = dto.CreatedBy;
        
        // Handle many-to-many relationships
        foreach (var groupId in dto.GroupIds)
        {
            contact.ContactGroups.Add(new ContactGroup 
            { 
                ContactId = contact.Id, 
                GroupId = groupId 
            });
        }
        
        foreach (var tagId in dto.TagIds)
        {
            contact.ContactTags.Add(new ContactTag 
            { 
                ContactId = contact.Id, 
                TagId = tagId 
            });
        }
        
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        
        return await GetContactByIdAsync(contact.Id) ?? throw new InvalidOperationException();
    }
    
    public async Task<string?> UploadAvatarAsync(Guid id, IFormFile file)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return null;
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        
        // Store the image data in the database
        contact.AvatarData = memoryStream.ToArray();
        contact.AvatarContentType = file.ContentType;
        contact.Avatar = $"/api/contacts/{id}/avatar"; // URL to retrieve the avatar
        contact.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return contact.Avatar;
    }
    
    public async Task<(byte[]? data, string contentType)> GetAvatarAsync(Guid id)
    {
        var contact = await _context.Contacts
            .Select(c => new { c.Id, c.AvatarData, c.AvatarContentType })
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (contact?.AvatarData == null)
            return (null, "");
        
        return (contact.AvatarData, contact.AvatarContentType ?? "image/jpeg");
    }
    
    public async Task<List<DuplicateGroupDto>> FindDuplicatesAsync(string fields)
    {
        var fieldList = fields.Split(',').Select(f => f.Trim().ToLower()).ToList();
        var duplicates = new List<DuplicateGroupDto>();
        
        if (fieldList.Contains("name"))
        {
            var nameGroups = await _context.Contacts
                .GroupBy(c => new { c.FirstName, c.LastName })
                .Where(g => g.Count() > 1)
                .Select(g => new DuplicateGroupDto
                {
                    Field = "Name",
                    Value = $"{g.Key.FirstName} {g.Key.LastName}",
                    Count = g.Count(),
                    ContactIds = g.Select(c => c.Id).ToList()
                })
                .ToListAsync();
            
            duplicates.AddRange(nameGroups);
        }
        
        if (fieldList.Contains("email"))
        {
            var emailGroups = await _context.EmailAddresses
                .GroupBy(e => e.Email.ToLower())
                .Where(g => g.Count() > 1)
                .Select(g => new DuplicateGroupDto
                {
                    Field = "Email",
                    Value = g.Key,
                    Count = g.Count(),
                    ContactIds = g.Select(e => e.ContactId).Distinct().ToList()
                })
                .ToListAsync();
            
            duplicates.AddRange(emailGroups);
        }
        
        return duplicates;
    }
    
    // Additional service methods implementation...
}
```

## Import/Export Service

```csharp
// Application/Services/ImportExportService.cs
using CsvHelper;
using ClosedXML.Excel;
using System.Text.Json;

namespace ContactManager.Application.Services;

public interface IImportExportService
{
    Task<ImportResultDto> ImportContactsAsync(
        IFormFile file, string format, Dictionary<string, string>? fieldMappings, string user);
    Task<(byte[] content, string contentType, string fileName)> ExportContactsAsync(
        ExportRequestDto request);
    (byte[] content, string contentType, string fileName) GetImportTemplate(string format);
}

public class ImportExportService : IImportExportService
{
    private readonly ApplicationDbContext _context;
    private readonly IContactService _contactService;
    private readonly ILogger<ImportExportService> _logger;
    
    public async Task<ImportResultDto> ImportContactsAsync(
        IFormFile file, string format, Dictionary<string, string>? fieldMappings, string user)
    {
        var startTime = DateTime.UtcNow;
        var errors = new List<ImportError>();
        var successCount = 0;
        var totalRecords = 0;
        
        try
        {
            var contacts = format.ToLower() switch
            {
                "csv" => await ParseCsvFile(file, fieldMappings),
                "excel" => await ParseExcelFile(file, fieldMappings),
                "json" => await ParseJsonFile(file),
                "vcard" => await ParseVCardFile(file),
                _ => throw new NotSupportedException($"Format {format} is not supported")
            };
            
            totalRecords = contacts.Count;
            
            foreach (var (contact, index) in contacts.Select((c, i) => (c, i)))
            {
                try
                {
                    contact.CreatedBy = user;
                    await _contactService.CreateContactAsync(contact);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add(new ImportError
                    {
                        Row = index + 2, // +2 for header and 0-based index
                        Field = "General",
                        Value = $"{contact.FirstName} {contact.LastName}",
                        Error = ex.Message
                    });
                }
            }
            
            // Save import history
            var history = new ImportHistory
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                FileFormat = format,
                TotalRecords = totalRecords,
                SuccessCount = successCount,
                FailedCount = errors.Count,
                ErrorDetails = errors.Any() ? JsonSerializer.Serialize(errors) : null,
                ImportedAt = DateTime.UtcNow,
                ImportedBy = user,
                Duration = DateTime.UtcNow - startTime
            };
            
            _context.ImportHistory.Add(history);
            await _context.SaveChangesAsync();
            
            return new ImportResultDto
            {
                Success = successCount,
                Failed = errors.Count,
                Errors = errors
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to import contacts from {FileName}", file.FileName);
            throw;
        }
    }
    
    private async Task<List<CreateContactDto>> ParseCsvFile(
        IFormFile file, Dictionary<string, string>? fieldMappings)
    {
        var contacts = new List<CreateContactDto>();
        
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        // Auto-detect delimiter
        csv.Configuration.Delimiter = DetectDelimiter(await reader.ReadLineAsync() ?? ",");
        reader.BaseStream.Position = 0;
        reader.DiscardBufferedData();
        
        var records = csv.GetRecords<dynamic>().ToList();
        
        foreach (var record in records)
        {
            var dict = record as IDictionary<string, object>;
            if (dict == null) continue;
            
            var contact = new CreateContactDto
            {
                FirstName = GetMappedValue(dict, fieldMappings, "FirstName", "first_name", "fname"),
                LastName = GetMappedValue(dict, fieldMappings, "LastName", "last_name", "lname"),
                MiddleName = GetMappedValue(dict, fieldMappings, "MiddleName", "middle_name"),
                Company = GetMappedValue(dict, fieldMappings, "Company", "company", "organization"),
                JobTitle = GetMappedValue(dict, fieldMappings, "JobTitle", "job_title", "title"),
                Notes = GetMappedValue(dict, fieldMappings, "Notes", "notes", "comments"),
                EmailAddresses = new List<CreateEmailAddressDto>(),
                PhoneNumbers = new List<CreatePhoneNumberDto>(),
                Addresses = new List<CreateAddressDto>()
            };
            
            // Parse email
            var email = GetMappedValue(dict, fieldMappings, "Email", "email", "email_address");
            if (!string.IsNullOrWhiteSpace(email))
            {
                contact.EmailAddresses.Add(new CreateEmailAddressDto
                {
                    Email = email,
                    Type = "Personal",
                    IsPrimary = true
                });
            }
            
            // Parse phone
            var phone = GetMappedValue(dict, fieldMappings, "Phone", "phone", "phone_number");
            if (!string.IsNullOrWhiteSpace(phone))
            {
                contact.PhoneNumbers.Add(new CreatePhoneNumberDto
                {
                    Number = phone,
                    Type = "Mobile",
                    IsPrimary = true
                });
            }
            
            contacts.Add(contact);
        }
        
        return contacts;
    }
    
    private async Task<List<CreateContactDto>> ParseExcelFile(
        IFormFile file, Dictionary<string, string>? fieldMappings)
    {
        var contacts = new List<CreateContactDto>();
        
        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RowsUsed().Skip(1); // Skip header
        
        foreach (var row in rows)
        {
            var contact = new CreateContactDto
            {
                FirstName = row.Cell(1).GetValue<string>(),
                LastName = row.Cell(2).GetValue<string>(),
                MiddleName = row.Cell(3).GetValue<string>(),
                Company = row.Cell(4).GetValue<string>(),
                JobTitle = row.Cell(5).GetValue<string>(),
                EmailAddresses = new List<CreateEmailAddressDto>(),
                PhoneNumbers = new List<CreatePhoneNumberDto>(),
                Addresses = new List<CreateAddressDto>()
            };
            
            var email = row.Cell(6).GetValue<string>();
            if (!string.IsNullOrWhiteSpace(email))
            {
                contact.EmailAddresses.Add(new CreateEmailAddressDto
                {
                    Email = email,
                    Type = "Personal",
                    IsPrimary = true
                });
            }
            
            var phone = row.Cell(7).GetValue<string>();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                contact.PhoneNumbers.Add(new CreatePhoneNumberDto
                {
                    Number = phone,
                    Type = "Mobile",
                    IsPrimary = true
                });
            }
            
            contacts.Add(contact);
        }
        
        return contacts;
    }
    
    public async Task<(byte[] content, string contentType, string fileName)> ExportContactsAsync(
        ExportRequestDto request)
    {
        var query = _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Addresses)
            .AsQueryable();
        
        if (request.ContactIds?.Any() == true)
        {
            query = query.Where(c => request.ContactIds.Contains(c.Id));
        }
        
        var contacts = await query.ToListAsync();
        
        return request.Format.ToLower() switch
        {
            "csv" => ExportToCsv(contacts, request),
            "excel" => ExportToExcel(contacts, request),
            "json" => ExportToJson(contacts, request),
            "vcard" => ExportToVCard(contacts, request),
            _ => throw new NotSupportedException($"Format {request.Format} is not supported")
        };
    }
    
    private (byte[] content, string contentType, string fileName) ExportToCsv(
        List<Contact> contacts, ExportRequestDto request)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        var records = contacts.Select(c => new
        {
            c.FirstName,
            c.LastName,
            c.MiddleName,
            c.Company,
            c.JobTitle,
            Email = c.EmailAddresses.FirstOrDefault(e => e.IsPrimary)?.Email,
            Phone = c.PhoneNumbers.FirstOrDefault(p => p.IsPrimary)?.Number,
            Address = c.Addresses.FirstOrDefault(a => a.IsPrimary)?.ToString(),
            Groups = request.IncludeGroups ? 
                string.Join(", ", c.ContactGroups.Select(cg => cg.Group.Name)) : null,
            Tags = request.IncludeTags ? 
                string.Join(", ", c.ContactTags.Select(ct => ct.Tag.Name)) : null
        });
        
        csv.WriteRecords(records);
        writer.Flush();
        
        return (
            memoryStream.ToArray(),
            "text/csv",
            $"contacts_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
        );
    }
    
    private string GetMappedValue(
        IDictionary<string, object> dict, 
        Dictionary<string, string>? fieldMappings,
        params string[] possibleKeys)
    {
        // Check field mappings first
        if (fieldMappings != null)
        {
            foreach (var key in possibleKeys)
            {
                if (fieldMappings.TryGetValue(key, out var mappedKey) &&
                    dict.TryGetValue(mappedKey, out var value))
                {
                    return value?.ToString() ?? string.Empty;
                }
            }
        }
        
        // Check default keys
        foreach (var key in possibleKeys)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value?.ToString() ?? string.Empty;
            }
            
            // Case-insensitive search
            var matchingKey = dict.Keys.FirstOrDefault(k => 
                string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
            
            if (matchingKey != null && dict.TryGetValue(matchingKey, out value))
            {
                return value?.ToString() ?? string.Empty;
            }
        }
        
        return string.Empty;
    }
    
    private string DetectDelimiter(string firstLine)
    {
        var delimiters = new[] { ",", ";", "\t", "|" };
        var counts = delimiters.Select(d => new 
        { 
            Delimiter = d, 
            Count = firstLine.Count(c => c.ToString() == d) 
        });
        
        return counts.OrderByDescending(c => c.Count).First().Delimiter;
    }
}
```

## WebSocket Support for Real-time Updates

```csharp
// API/Hubs/ContactHub.cs
using Microsoft.AspNetCore.SignalR;

namespace ContactManager.API.Hubs;

public interface IContactClient
{
    Task ContactCreated(ContactDto contact);
    Task ContactUpdated(ContactDto contact);
    Task ContactDeleted(Guid contactId);
    Task ContactsImported(ImportResultDto result);
}

public class ContactHub : Hub<IContactClient>
{
    private readonly ILogger<ContactHub> _logger;
    
    public ContactHub(ILogger<ContactHub> logger)
    {
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} joined group {GroupName}", 
            Context.ConnectionId, groupName);
    }
    
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} left group {GroupName}", 
            Context.ConnectionId, groupName);
    }
}

// Integration in ContactService
public class ContactService : IContactService
{
    private readonly IHubContext<ContactHub, IContactClient> _hubContext;
    
    public async Task<ContactDto> CreateContactAsync(CreateContactDto dto)
    {
        // ... existing create logic ...
        
        var createdContact = await GetContactByIdAsync(contact.Id);
        
        // Notify all connected clients
        await _hubContext.Clients.All.ContactCreated(createdContact);
        
        return createdContact;
    }
    
    public async Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactDto dto)
    {
        // ... existing update logic ...
        
        var updatedContact = await GetContactByIdAsync(id);
        
        // Notify all connected clients
        await _hubContext.Clients.All.ContactUpdated(updatedContact);
        
        return updatedContact;
    }
    
    public async Task DeleteContactAsync(Guid id)
    {
        // ... existing delete logic ...
        
        // Notify all connected clients
        await _hubContext.Clients.All.ContactDeleted(id);
    }
}
```

## Application DbContext

```csharp
// Infrastructure/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ContactManager.Domain.Entities;

namespace ContactManager.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<EmailAddress> EmailAddresses => Set<EmailAddress>();
    public DbSet<PhoneNumber> PhoneNumbers => Set<PhoneNumber>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<SocialProfile> SocialProfiles => Set<SocialProfile>();
    public DbSet<CustomField> CustomFields => Set<CustomField>();
    public DbSet<ContactActivity> ContactActivities => Set<ContactActivity>();
    public DbSet<ContactRelationship> ContactRelationships => Set<ContactRelationship>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ContactGroup> ContactGroups => Set<ContactGroup>();
    public DbSet<ContactTag> ContactTags => Set<ContactTag>();
    public DbSet<ImportHistory> ImportHistory => Set<ImportHistory>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Contact configuration
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.IsFavorite);
            entity.HasIndex(e => e.LastContactedAt);
            entity.HasIndex(e => new { e.FirstName, e.LastName });
            
            // Set default values
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsFavorite).HasDefaultValue(false);
        });
        
        // Email configuration
        modelBuilder.Entity<EmailAddress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.EmailAddresses)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Phone configuration
        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Number).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Number);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.PhoneNumbers)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Address configuration
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StreetLine1).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.Addresses)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Social Profile configuration
        modelBuilder.Entity<SocialProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Platform).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.SocialProfiles)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Custom Field configuration
        modelBuilder.Entity<CustomField>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FieldName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FieldType).IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.CustomFields)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Activity configuration
        modelBuilder.Entity<ContactActivity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActivityType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.ActivityDate);
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.Activities)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Relationship configuration
        modelBuilder.Entity<ContactRelationship>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RelationshipType).IsRequired().HasMaxLength(50);
            
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.Relationships)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.RelatedContact)
                .WithMany(c => c.RelatedContacts)
                .HasForeignKey(e => e.RelatedContactId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // Group configuration
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });
        
        // Tag configuration
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ColorHex).IsRequired().HasMaxLength(7);
            entity.HasIndex(e => e.Name).IsUnique();
        });
        
        // Many-to-many: Contact-Group
        modelBuilder.Entity<ContactGroup>(entity =>
        {
            entity.HasKey(e => new { e.ContactId, e.GroupId });
            
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.ContactGroups)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Group)
                .WithMany(g => g.ContactGroups)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Many-to-many: Contact-Tag
        modelBuilder.Entity<ContactTag>(entity =>
        {
            entity.HasKey(e => new { e.ContactId, e.TagId });
            
            entity.HasOne(e => e.Contact)
                .WithMany(c => c.ContactTags)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Tag)
                .WithMany(t => t.ContactTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Import History configuration
        modelBuilder.Entity<ImportHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FileFormat).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ImportedAt);
        });
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Contact && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));
        
        foreach (var entry in entries)
        {
            var contact = (Contact)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                contact.CreatedAt = DateTime.UtcNow;
            }
            
            contact.UpdatedAt = DateTime.UtcNow;
        }
    }
}
```

## AutoMapper Profiles

```csharp
// Application/Mappings/ContactMappingProfile.cs
using AutoMapper;
using ContactManager.Domain.Entities;
using ContactManager.Application.DTOs;

namespace ContactManager.Application.Mappings;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        // Contact mappings
        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => 
                src.ContactGroups.Select(cg => cg.Group)))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => 
                src.ContactTags.Select(ct => ct.Tag)));
                
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContactGroups, opt => opt.Ignore())
            .ForMember(dest => dest.ContactTags, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContactGroups, opt => opt.Ignore())
            .ForMember(dest => dest.ContactTags, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        // Email mappings
        CreateMap<EmailAddress, EmailAddressDto>();
        CreateMap<CreateEmailAddressDto, EmailAddress>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Phone mappings
        CreateMap<PhoneNumber, PhoneNumberDto>();
        CreateMap<CreatePhoneNumberDto, PhoneNumber>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Address mappings
        CreateMap<Address, AddressDto>();
        CreateMap<CreateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Social Profile mappings
        CreateMap<SocialProfile, SocialProfileDto>();
        CreateMap<CreateSocialProfileDto, SocialProfile>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Custom Field mappings
        CreateMap<CustomField, CustomFieldDto>();
        CreateMap<CreateCustomFieldDto, CustomField>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Activity mappings
        CreateMap<ContactActivity, ContactActivityDto>();
        CreateMap<CreateActivityDto, ContactActivity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Group mappings
        CreateMap<Group, GroupDto>();
        CreateMap<CreateGroupDto, Group>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Tag mappings
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
```

## Program.cs Configuration

```csharp
// Program.cs
using ContactManager.API.Hubs;
using ContactManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // For JSON Patch support

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Contact Manager API", 
        Version = "v1",
        Description = "Enhanced Contact Management System API"
    });
});

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(3);
            npgsqlOptions.CommandTimeout(30);
        }));

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IImportExportService, ImportExportService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

// Add SignalR for real-time updates
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaximumReceiveMessageSize = 102400; // 100KB
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Required for SignalR
        });
});

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database")
    .AddCheck("ready", () => HealthCheckResult.Healthy("Application is ready"));

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Configure request size limits
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10MB
});

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseResponseCompression();

// Health check endpoints
app.MapHealthChecks("/api/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            service = "Contact Manager API",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.MapHealthChecks("/api/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Name == "ready",
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ContactHub>("/hubs/contacts");

app.Run();
```
















































































I see we are using `async` but we don't forward cancellation tokens. Is this not required anymore? Also what is the most basic way to add authentication? This is a simple app to be used by a single person. 

You're absolutely right! Forwarding cancellation tokens is still a best practice in .NET 9. Let me update the implementation with proper cancellation token usage and add a simple authentication solution.Perfect! I've created a comprehensive guide addressing both of your concerns:

## 1. **Cancellation Tokens** - You're absolutely right!

Forwarding cancellation tokens is still a best practice in .NET 9. I've updated all the code to properly use them:

- **Every async method** now accepts `CancellationToken cancellationToken = default`
- **Controllers automatically receive tokens** from ASP.NET Core
- **All async operations** (database queries, file I/O) forward the token
- **Default parameter values** maintain backward compatibility

Key benefits:
- Proper request cancellation when clients disconnect
- Better resource management
- Prevents unnecessary work when requests are cancelled
- Essential for production applications

## 2. **Simple Authentication Options**

For a single-user application, I've provided three options:

### **Option A: API Key (Recommended for your use case)**
```csharp
// Just add a header to all requests
X-API-Key: your-secret-key
```
- ✅ **Simplest to implement**
- ✅ No login UI needed
- ✅ Configure once and forget
- ✅ Works with HTTP test files easily

### **Option B: Basic Authentication**
```csharp
// Standard HTTP Basic Auth
Authorization: Basic base64(username:password)
```
- ✅ Built into browsers
- ✅ Simple configuration
- ⚠️ Credentials sent with every request

### **Option C: JWT Token**
```csharp
// Login once, use token
POST /api/auth/login → Get token
Authorization: Bearer {token}
```
- ✅ Most secure and flexible
- ✅ Tokens can expire
- ⚠️ Requires login UI

## **My Recommendation**

For your single-person use case, **go with API Key authentication**:

1. **Set it in appsettings.json**:
```json
{
  "Auth": {
    "ApiKey": "generate-a-long-random-string-here"
  }
}
```

2. **It's automatically required on all endpoints**
3. **Store the key in your Angular app's environment file**
4. **Use HTTPS in production**

The implementation I provided:
- Uses proper async/await patterns with cancellation
- Includes authentication without over-engineering
- Maintains clean, testable code
- Follows .NET 9 best practices

Would you like me to elaborate on any specific part or help you implement a particular authentication method?
























# Enhanced .NET 9 Implementation with Cancellation Tokens and Simple Authentication

## 1. Proper Cancellation Token Usage

### Updated Service Interface with Cancellation Tokens

```csharp
// Application/Services/IContactService.cs
namespace ContactManager.Application.Services;

public interface IContactService
{
    Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedContactsAsync(
        int pageNumber, int pageSize, string? sort, string? filter, CancellationToken cancellationToken = default);
    Task<ContactDto?> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContactDto> CreateContactAsync(CreateContactDto dto, CancellationToken cancellationToken = default);
    Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactDto dto, CancellationToken cancellationToken = default);
    Task<ContactDto?> PatchContactAsync(Guid id, JsonPatchDocument<UpdateContactDto> patchDoc, string user, CancellationToken cancellationToken = default);
    Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ContactSummaryDto>> SearchContactsAsync(string query, CancellationToken cancellationToken = default);
    Task<List<ContactSummaryDto>> AdvancedSearchAsync(AdvancedSearchDto searchDto, CancellationToken cancellationToken = default);
    Task<List<DuplicateGroupDto>> FindDuplicatesAsync(string fields, CancellationToken cancellationToken = default);
    Task<List<ContactSummaryDto>> GetBirthdaysAsync(int? month, int? day, CancellationToken cancellationToken = default);
    Task<List<ContactSummaryDto>> GetRecentlyContactedAsync(int days, CancellationToken cancellationToken = default);
    Task<List<ContactSummaryDto>> GetFavoritesAsync(CancellationToken cancellationToken = default);
    Task ToggleFavoriteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<string?> UploadAvatarAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAvatarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(byte[]? data, string contentType)> GetAvatarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BulkOperationResultDto> BulkOperationAsync(
        BulkOperationRequestDto<CreateContactDto> request, string user, CancellationToken cancellationToken = default);
}
```

### Updated Service Implementation with Cancellation Tokens

```csharp
// Application/Services/ContactService.cs
public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;
    private readonly IHubContext<ContactHub, IContactClient> _hubContext;
    
    public ContactService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ContactService> logger,
        IHubContext<ContactHub, IContactClient> hubContext)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _hubContext = hubContext;
    }
    
    public async Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedContactsAsync(
        int pageNumber, int pageSize, string? sort, string? filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .AsNoTracking() // Better performance for read-only queries
            .AsQueryable();
        
        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var filters = JsonSerializer.Deserialize<Dictionary<string, string>>(filter);
            if (filters != null)
            {
                query = ApplyFilters(query, filters);
            }
        }
        
        // Apply sorting
        query = ApplySorting(query, sort);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ContactSummaryDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Company = c.Company,
                PrimaryEmail = c.EmailAddresses
                    .Where(e => e.IsPrimary)
                    .Select(e => e.Email)
                    .FirstOrDefault(),
                PrimaryPhone = c.PhoneNumbers
                    .Where(p => p.IsPrimary)
                    .Select(p => p.Number)
                    .FirstOrDefault(),
                IsFavorite = c.IsFavorite,
                Avatar = c.Avatar
            })
            .ToListAsync(cancellationToken);
        
        return new PaginatedResultDto<ContactSummaryDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
    
    public async Task<ContactDto?> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Addresses)
            .Include(c => c.SocialProfiles)
            .Include(c => c.CustomFields)
            .Include(c => c.ContactGroups)
                .ThenInclude(cg => cg.Group)
            .Include(c => c.ContactTags)
                .ThenInclude(ct => ct.Tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (contact == null)
            return null;
        
        return _mapper.Map<ContactDto>(contact);
    }
    
    public async Task<ContactDto> CreateContactAsync(CreateContactDto dto, CancellationToken cancellationToken = default)
    {
        var contact = _mapper.Map<Contact>(dto);
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTime.UtcNow;
        contact.UpdatedAt = DateTime.UtcNow;
        contact.CreatedBy = dto.CreatedBy;
        
        // Handle many-to-many relationships
        foreach (var groupId in dto.GroupIds)
        {
            contact.ContactGroups.Add(new ContactGroup 
            { 
                ContactId = contact.Id, 
                GroupId = groupId 
            });
        }
        
        foreach (var tagId in dto.TagIds)
        {
            contact.ContactTags.Add(new ContactTag 
            { 
                ContactId = contact.Id, 
                TagId = tagId 
            });
        }
        
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync(cancellationToken);
        
        var createdContact = await GetContactByIdAsync(contact.Id, cancellationToken) 
            ?? throw new InvalidOperationException("Failed to retrieve created contact");
        
        // Notify all connected clients
        await _hubContext.Clients.All.ContactCreated(createdContact);
        
        return createdContact;
    }
    
    public async Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactDto dto, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .Include(c => c.EmailAddresses)
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Addresses)
            .Include(c => c.SocialProfiles)
            .Include(c => c.CustomFields)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (contact == null)
            return null;
        
        _mapper.Map(dto, contact);
        contact.UpdatedAt = DateTime.UtcNow;
        contact.UpdatedBy = dto.UpdatedBy;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        var updatedContact = await GetContactByIdAsync(id, cancellationToken);
        
        if (updatedContact != null)
            await _hubContext.Clients.All.ContactUpdated(updatedContact);
        
        return updatedContact;
    }
    
    public async Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts.FindAsync(new object[] { id }, cancellationToken);
        
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync(cancellationToken);
            
            await _hubContext.Clients.All.ContactDeleted(id);
        }
    }
    
    public async Task<string?> UploadAvatarAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts.FindAsync(new object[] { id }, cancellationToken);
        if (contact == null)
            return null;
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        
        // Store the image data in the database
        contact.AvatarData = memoryStream.ToArray();
        contact.AvatarContentType = file.ContentType;
        contact.Avatar = $"/api/contacts/{id}/avatar";
        contact.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return contact.Avatar;
    }
    
    // Helper methods
    private IQueryable<Contact> ApplyFilters(IQueryable<Contact> query, Dictionary<string, string> filters)
    {
        foreach (var (key, value) in filters)
        {
            query = key.ToLower() switch
            {
                "favorite" => query.Where(c => c.IsFavorite == bool.Parse(value)),
                "company" => query.Where(c => c.Company != null && 
                    c.Company.Contains(value, StringComparison.OrdinalIgnoreCase)),
                "group" => query.Where(c => c.ContactGroups.Any(cg => 
                    cg.GroupId == Guid.Parse(value))),
                "tag" => query.Where(c => c.ContactTags.Any(ct => 
                    ct.TagId == Guid.Parse(value))),
                _ => query
            };
        }
        return query;
    }
    
    private IQueryable<Contact> ApplySorting(IQueryable<Contact> query, string? sort)
    {
        return sort?.ToLower() switch
        {
            "firstname" => query.OrderBy(c => c.FirstName),
            "lastname" => query.OrderBy(c => c.LastName),
            "company" => query.OrderBy(c => c.Company),
            "createdat" => query.OrderByDescending(c => c.CreatedAt),
            "lastcontacted" => query.OrderByDescending(c => c.LastContactedAt),
            _ => query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
        };
    }
}
```

### Updated Controller with Cancellation Tokens

```csharp
// API/Controllers/ContactsController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires authentication
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IImportExportService _importExportService;
    private readonly IActivityService _activityService;
    
    public ContactsController(
        IContactService contactService,
        IImportExportService importExportService,
        IActivityService activityService)
    {
        _contactService = contactService;
        _importExportService = importExportService;
        _activityService = activityService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<ContactSummaryDto>>> GetContacts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "lastName",
        [FromQuery] string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _contactService.GetPaginatedContactsAsync(
            pageNumber, pageSize, sort, filter, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _contactService.GetContactByIdAsync(id, cancellationToken);
        if (contact == null)
            return NotFound();
        
        // Update last viewed
        await _activityService.TrackContactViewAsync(id, GetCurrentUser(), cancellationToken);
        
        return Ok(contact);
    }
    
    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(
        CreateContactDto dto, 
        CancellationToken cancellationToken = default)
    {
        dto = dto with { CreatedBy = GetCurrentUser() };
        var contact = await _contactService.CreateContactAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDto>> UpdateContact(
        Guid id, 
        UpdateContactDto dto,
        CancellationToken cancellationToken = default)
    {
        dto = dto with { UpdatedBy = GetCurrentUser() };
        var contact = await _contactService.UpdateContactAsync(id, dto, cancellationToken);
        if (contact == null)
            return NotFound();
        return Ok(contact);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(Guid id, CancellationToken cancellationToken = default)
    {
        await _contactService.DeleteContactAsync(id, cancellationToken);
        return NoContent();
    }
    
    [HttpPost("{id}/avatar")]
    [RequestSizeLimit(5_242_880)] // 5MB limit
    public async Task<ActionResult<string>> UploadAvatar(
        Guid id, 
        [FromForm] IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return BadRequest("File is empty");
        
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid file type");
        
        var avatarUrl = await _contactService.UploadAvatarAsync(id, file, cancellationToken);
        if (avatarUrl == null)
            return NotFound();
        
        return Ok(new { avatarUrl });
    }
    
    private string GetCurrentUser()
    {
        return User.Identity?.Name ?? "system";
    }
}
```

## 2. Simple Authentication for Single User

### Option A: API Key Authentication (Simplest)

```csharp
// Auth/ApiKeyAuthenticationHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ContactManager.Auth;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string ApiKeyHeaderName = "X-API-Key";
    
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if API key is in header
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeader))
        {
            // Check if API key is in query string (for convenience)
            if (!Request.Query.TryGetValue("apiKey", out var apiKeyQuery))
            {
                return AuthenticateResult.Fail("API Key not found");
            }
            apiKeyHeader = apiKeyQuery;
        }
        
        var providedApiKey = apiKeyHeader.ToString();
        
        // Validate API key
        if (string.IsNullOrWhiteSpace(providedApiKey) || providedApiKey != Options.ApiKey)
        {
            return AuthenticateResult.Fail("Invalid API Key");
        }
        
        // Create claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "User"),
            new Claim(ClaimTypes.NameIdentifier, "single-user")
        };
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = string.Empty;
}

// Extension method for easier registration
public static class ApiKeyAuthenticationExtensions
{
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        string apiKey)
    {
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            "ApiKey",
            options => options.ApiKey = apiKey);
    }
}
```

### Option B: Basic Authentication (Username/Password)

```csharp
// Auth/BasicAuthenticationHandler.cs
using System.Net.Http.Headers;
using System.Text;

namespace ContactManager.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<BasicAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check for Authorization header
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }
        
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            
            if (authHeader.Scheme != "Basic")
            {
                return AuthenticateResult.Fail("Invalid Authentication Scheme");
            }
            
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            
            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
            
            var username = credentials[0];
            var password = credentials[1];
            
            // Validate credentials
            if (username != Options.Username || password != Options.Password)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username)
            };
            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            
            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
    
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = $"Basic realm=\"Contact Manager\"";
        return base.HandleChallengeAsync(properties);
    }
}

public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

### Option C: Simple JWT Token (Stateless)

```csharp
// Auth/SimpleJwtService.cs
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManager.Auth;

public interface ISimpleJwtService
{
    string GenerateToken(string username);
    ClaimsPrincipal? ValidateToken(string token);
}

public class SimpleJwtService : ISimpleJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    
    public SimpleJwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        _issuer = configuration["Jwt:Issuer"] ?? "ContactManager";
        _audience = configuration["Jwt:Audience"] ?? "ContactManagerUsers";
    }
    
    public string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("created", DateTime.UtcNow.ToString("o"))
            }),
            Expires = DateTime.UtcNow.AddDays(30), // Long-lived for single user
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}

// Simple login controller
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ISimpleJwtService _jwtService;
    
    public AuthController(IConfiguration configuration, ISimpleJwtService jwtService)
    {
        _configuration = configuration;
        _jwtService = jwtService;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var configuredUsername = _configuration["Auth:Username"] ?? "admin";
        var configuredPassword = _configuration["Auth:Password"] ?? "password";
        
        if (dto.Username != configuredUsername || dto.Password != configuredPassword)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        
        var token = _jwtService.GenerateToken(dto.Username);
        
        return Ok(new
        {
            token,
            expiresIn = 2592000, // 30 days in seconds
            username = dto.Username
        });
    }
}

public record LoginDto(string Username, string Password);
```

## 3. Program.cs Configuration with Authentication

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Choose ONE authentication method:

// Option A: API Key Authentication (Simplest)
builder.Services.AddAuthentication("ApiKey")
    .AddApiKey(builder.Configuration["Auth:ApiKey"] ?? "your-secret-api-key-here");

// Option B: Basic Authentication
builder.Services.AddAuthentication("Basic")
    .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("Basic", options =>
    {
        options.Username = builder.Configuration["Auth:Username"] ?? "admin";
        options.Password = builder.Configuration["Auth:Password"] ?? "password";
    });

// Option C: JWT Authentication
builder.Services.AddSingleton<ISimpleJwtService, SimpleJwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(
            builder.Configuration["Jwt:SecretKey"] ?? "your-256-bit-secret-key-here-minimum-32-characters");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ContactManager",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ContactManagerUsers",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        // Support token from query string for SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Configure CORS to include auth headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader() // Allows Authorization header
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition"); // For file downloads
        });
});

// Rest of services...
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

// Optional: Allow anonymous access to health endpoints
app.MapHealthChecks("/api/health").AllowAnonymous();
app.MapHealthChecks("/api/health/ready").AllowAnonymous();

// Optional: Allow anonymous access to auth endpoint if using JWT
if (builder.Configuration["Auth:Method"] == "JWT")
{
    app.MapControllers()
        .RequireAuthorization() // Require auth by default
        .WithMetadata(new AllowAnonymousAttribute()); // Except where specified
}
else
{
    app.MapControllers().RequireAuthorization();
}

app.MapHub<ContactHub>("/hubs/contacts").RequireAuthorization();

app.Run();
```

## 4. Updated appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ContactManager;Username=postgres;Password=yourpassword"
  },
  "Auth": {
    "Method": "ApiKey", // Options: "ApiKey", "Basic", "JWT"
    "ApiKey": "your-secret-api-key-change-this-in-production",
    "Username": "admin",
    "Password": "your-secure-password-here"
  },
  "Jwt": {
    "SecretKey": "your-256-bit-secret-key-here-minimum-32-characters-long",
    "Issuer": "ContactManager",
    "Audience": "ContactManagerUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 5. Angular Integration Examples

### API Key Authentication in Angular

```typescript
// Angular service with API Key
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiKey = 'your-secret-api-key-change-this-in-production';
  private baseUrl = 'https://localhost:7034/api';
  
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'X-API-Key': this.apiKey,
      'Content-Type': 'application/json'
    });
  }
  
  getContacts() {
    return this.http.get(`${this.baseUrl}/contacts`, {
      headers: this.getHeaders()
    });
  }
}
```

### Basic Authentication in Angular

```typescript
// Angular service with Basic Auth
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private username = 'admin';
  private password = 'password';
  private baseUrl = 'https://localhost:7034/api';
  
  private getHeaders(): HttpHeaders {
    const credentials = btoa(`${this.username}:${this.password}`);
    return new HttpHeaders({
      'Authorization': `Basic ${credentials}`,
      'Content-Type': 'application/json'
    });
  }
  
  getContacts() {
    return this.http.get(`${this.baseUrl}/contacts`, {
      headers: this.getHeaders()
    });
  }
}
```

### JWT Authentication in Angular

```typescript
// Angular service with JWT
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token = signal<string | null>(null);
  private baseUrl = 'https://localhost:7034/api';
  
  constructor(private http: HttpClient) {
    // Load token from localStorage on init
    const savedToken = localStorage.getItem('auth_token');
    if (savedToken) {
      this.token.set(savedToken);
    }
  }
  
  async login(username: string, password: string): Promise<boolean> {
    try {
      const response = await firstValueFrom(
        this.http.post<any>(`${this.baseUrl}/auth/login`, { username, password })
      );
      
      this.token.set(response.token);
      localStorage.setItem('auth_token', response.token);
      return true;
    } catch {
      return false;
    }
  }
  
  logout() {
    this.token.set(null);
    localStorage.removeItem('auth_token');
  }
  
  getAuthHeaders(): HttpHeaders {
    const token = this.token();
    return new HttpHeaders({
      'Authorization': token ? `Bearer ${token}` : '',
      'Content-Type': 'application/json'
    });
  }
}

// HTTP Interceptor for JWT
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.token();
    
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    
    return next.handle(req);
  }
}
```

## 6. Updated HTTP Test File with Authentication

```http
# MyOpenTelemetryApi.http
@host = https://localhost:7034
@contentType = application/json

# For API Key auth
@apiKey = your-secret-api-key-change-this-in-production

# For Basic auth
@username = admin
@password = password

# For JWT auth - first login to get token
@token = {{login.response.body.token}}

################################################
# AUTH ENDPOINTS (if using JWT)
################################################

### Login (JWT only)
# @name login
POST {{host}}/api/auth/login
Content-Type: {{contentType}}

{
  "username": "{{username}}",
  "password": "{{password}}"
}

###

################################################
# HEALTH ENDPOINTS (No auth required)
################################################

### Health Check
GET {{host}}/api/health
Accept: {{contentType}}

###

################################################
# CONTACT ENDPOINTS (Auth required)
################################################

### Get Contacts with API Key
GET {{host}}/api/contacts
X-API-Key: {{apiKey}}
Accept: {{contentType}}

###

### Get Contacts with Basic Auth
GET {{host}}/api/contacts
Authorization: Basic {{$base64 username:password}}
Accept: {{contentType}}

###

### Get Contacts with JWT
GET {{host}}/api/contacts
Authorization: Bearer {{token}}
Accept: {{contentType}}

###

### Create Contact with API Key
POST {{host}}/api/contacts
X-API-Key: {{apiKey}}
Content-Type: {{contentType}}

{
  "firstName": "John",
  "lastName": "Doe",
  "emailAddresses": [
    {
      "email": "john@example.com",
      "type": "Personal",
      "isPrimary": true
    }
  ]
}

###
```

## Summary

### Cancellation Token Best Practices:
1. **Always forward cancellation tokens** through the entire call chain
2. **Use default parameter values** to maintain backward compatibility
3. **Pass tokens to all async operations** (database queries, HTTP calls, file I/O)
4. **Controllers automatically receive tokens** from ASP.NET Core

### Authentication Recommendations:

For a **single-user application**, I recommend:

1. **API Key (Simplest)**: 
   - ✅ No login UI needed
   - ✅ Easy to implement
   - ✅ Works with all clients
   - ⚠️ Key must be kept secret
   - 📱 Store key in environment variables

2. **Basic Auth (Simple but Standard)**:
   - ✅ Built into browsers
   - ✅ No token management
   - ⚠️ Credentials sent with every request
   - 📱 Use HTTPS always

3. **JWT (Most Flexible)**:
   - ✅ Stateless
   - ✅ Can expire and refresh
   - ✅ Industry standard
   - ⚠️ Requires login UI
   - 📱 Good for web apps

For your use case (single person), **API Key** is probably the best choice - simple, secure enough with HTTPS, and no login UI needed.