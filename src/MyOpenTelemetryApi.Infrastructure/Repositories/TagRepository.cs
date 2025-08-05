// src/MyOpenTelemetryApi.Infrastructure/Repositories/TagRepository.cs
using Microsoft.EntityFrameworkCore;
using MyOpenTelemetryApi.Domain.Entities;
using MyOpenTelemetryApi.Domain.Interfaces;
using MyOpenTelemetryApi.Infrastructure.Data;

namespace MyOpenTelemetryApi.Infrastructure.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context) { }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }
}