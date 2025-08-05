// src/MyOpenTelemetryApi.Api/Controllers/TagsController.cs
using Microsoft.AspNetCore.Mvc;
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Application.Services;

namespace MyOpenTelemetryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController(ITagService tagService, ILogger<TagsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> GetTags()
    {
        List<TagDto> tags = await tagService.GetAllAsync();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(Guid id)
    {
        TagDto? tag = await tagService.GetByIdAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto dto)
    {
        try
        {
            TagDto tag = await tagService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating tag");
            return StatusCode(500, "An error occurred while creating the tag.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagDto>> UpdateTag(Guid id, CreateTagDto dto)
    {
        try
        {
            TagDto? tag = await tagService.UpdateAsync(id, dto);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating tag {TagId}", id);
            return StatusCode(500, "An error occurred while updating the tag.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        bool deleted = await tagService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
