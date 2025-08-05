// src/MyOpenTelemetryApi.Application/DTOs/GroupDto.cs
namespace MyOpenTelemetryApi.Application.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ContactCount { get; set; }
}

public class CreateGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
