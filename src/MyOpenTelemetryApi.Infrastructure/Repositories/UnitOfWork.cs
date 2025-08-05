// src/MyOpenTelemetryApi.Infrastructure/Repositories/UnitOfWork.cs
using MyOpenTelemetryApi.Domain.Interfaces;
using MyOpenTelemetryApi.Infrastructure.Data;

namespace MyOpenTelemetryApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IContactRepository? _contacts;
    private IGroupRepository? _groups;
    private ITagRepository? _tags;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IContactRepository Contacts => _contacts ??= new ContactRepository(_context);
    public IGroupRepository Groups => _groups ??= new GroupRepository(_context);
    public ITagRepository Tags => _tags ??= new TagRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}