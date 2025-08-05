// src/MyOpenTelemetryApi.Application/Services/GroupService.cs
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Domain.Entities;
using MyOpenTelemetryApi.Domain.Interfaces;

namespace MyOpenTelemetryApi.Application.Services;

public class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;

    public GroupService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GroupDto?> GetByIdAsync(Guid id)
    {
        var group = await _unitOfWork.Groups.GetGroupWithContactsAsync(id);
        return group == null ? null : MapToDto(group);
    }

    public async Task<List<GroupDto>> GetAllAsync()
    {
        var groups = await _unitOfWork.Groups.GetAllAsync();
        var groupDtos = new List<GroupDto>();

        foreach (var group in groups)
        {
            var groupWithContacts = await _unitOfWork.Groups.GetGroupWithContactsAsync(group.Id);
            groupDtos.Add(MapToDto(groupWithContacts!));
        }

        return groupDtos;
    }

    public async Task<GroupDto> CreateAsync(CreateGroupDto dto)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Groups.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(group);
    }

    public async Task<GroupDto?> UpdateAsync(Guid id, UpdateGroupDto dto)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id);
        if (group == null) return null;

        group.Name = dto.Name;
        group.Description = dto.Description;

        await _unitOfWork.Groups.UpdateAsync(group);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(group);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id);
        if (group == null) return false;

        await _unitOfWork.Groups.DeleteAsync(group);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static GroupDto MapToDto(Group group)
    {
        return new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            ContactCount = group.ContactGroups?.Count ?? 0
        };
    }
}