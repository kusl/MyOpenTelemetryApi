// src/MyOpenTelemetryApi.Application/Services/TagService.cs
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Domain.Entities;
using MyOpenTelemetryApi.Domain.Interfaces;

namespace MyOpenTelemetryApi.Application.Services;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;

    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TagDto?> GetByIdAsync(Guid id)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        return tag == null ? null : MapToDto(tag);
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _unitOfWork.Tags.GetAllAsync();
        return tags.Select(MapToDto).ToList();
    }

    public async Task<TagDto> CreateAsync(CreateTagDto dto)
    {
        // Check if tag with same name already exists
        var existingTag = await _unitOfWork.Tags.GetByNameAsync(dto.Name);
        if (existingTag != null)
        {
            throw new InvalidOperationException($"Tag with name '{dto.Name}' already exists.");
        }

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ColorHex = dto.ColorHex
        };

        await _unitOfWork.Tags.AddAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(tag);
    }

    public async Task<TagDto?> UpdateAsync(Guid id, CreateTagDto dto)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null) return null;

        // Check if another tag with the same name exists
        var existingTag = await _unitOfWork.Tags.GetByNameAsync(dto.Name);
        if (existingTag != null && existingTag.Id != id)
        {
            throw new InvalidOperationException($"Tag with name '{dto.Name}' already exists.");
        }

        tag.Name = dto.Name;
        tag.ColorHex = dto.ColorHex;

        await _unitOfWork.Tags.UpdateAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(tag);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null) return false;

        await _unitOfWork.Tags.DeleteAsync(tag);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private TagDto MapToDto(Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            ColorHex = tag.ColorHex
        };
    }
}