// src/MyOpenTelemetryApi.Infrastructure/Repositories/GroupRepository.cs
using Microsoft.EntityFrameworkCore;
using MyOpenTelemetryApi.Domain.Entities;
using MyOpenTelemetryApi.Domain.Interfaces;
using MyOpenTelemetryApi.Infrastructure.Data;

namespace MyOpenTelemetryApi.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(AppDbContext context) : base(context) { }

    public async Task<Group?> GetGroupWithContactsAsync(Guid id)
    {
        return await _context.Groups
            .Include(g => g.ContactGroups)
                .ThenInclude(cg => cg.Contact)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
}