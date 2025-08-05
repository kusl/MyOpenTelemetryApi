// src/MyOpenTelemetryApi.Infrastructure/Repositories/UnitOfWork.cs
using MyOpenTelemetryApi.Domain.Interfaces;
using MyOpenTelemetryApi.Infrastructure.Data;

namespace MyOpenTelemetryApi.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IContactRepository? _contacts;
    private IGroupRepository? _groups;
    private ITagRepository? _tags;

    public IContactRepository Contacts => _contacts ??= new ContactRepository(context);
    public IGroupRepository Groups => _groups ??= new GroupRepository(context);
    public ITagRepository Tags => _tags ??= new TagRepository(context);

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}