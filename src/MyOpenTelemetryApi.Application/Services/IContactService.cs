// src/MyOpenTelemetryApi.Application/Services/IContactService.cs
using MyOpenTelemetryApi.Application.DTOs;

namespace MyOpenTelemetryApi.Application.Services;

public interface IContactService
{
    Task<ContactDto?> GetByIdAsync(Guid id);
    Task<ContactDto?> GetWithDetailsAsync(Guid id);
    Task<PaginatedResultDto<ContactSummaryDto>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<List<ContactSummaryDto>> SearchAsync(string searchTerm);
    Task<ContactDto> CreateAsync(CreateContactDto dto);
    Task<ContactDto?> UpdateAsync(Guid id, UpdateContactDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<List<ContactSummaryDto>> GetByGroupAsync(Guid groupId);
    Task<List<ContactSummaryDto>> GetByTagAsync(Guid tagId);
}