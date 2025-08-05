// src/MyOpenTelemetryApi.Domain/Interfaces/IContactRepository.cs
using MyOpenTelemetryApi.Domain.Entities;

namespace MyOpenTelemetryApi.Domain.Interfaces;

public interface IContactRepository : IRepository<Contact>
{
    Task<IEnumerable<Contact>> GetContactsByGroupAsync(Guid groupId);
    Task<IEnumerable<Contact>> GetContactsByTagAsync(Guid tagId);
    Task<Contact?> GetContactWithDetailsAsync(Guid id);
    Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
}
