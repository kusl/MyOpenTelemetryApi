// src/MyOpenTelemetryApi.Api/Controllers/TagsController.cs
using Microsoft.AspNetCore.Mvc;
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Application.Services;

namespace MyOpenTelemetryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagService tagService, ILogger<TagsController> logger)
    {
        _tagService = tagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> GetTags()
    {
        var tags = await _tagService.GetAllAsync();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(Guid id)
    {
        var tag = await _tagService.GetByIdAsync(id);
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
            var tag = await _tagService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tag");
            return StatusCode(500, "An error occurred while creating the tag.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagDto>> UpdateTag(Guid id, CreateTagDto dto)
    {
        try
        {
            var tag = await _tagService.UpdateAsync(id, dto);
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
            _logger.LogError(ex, "Error updating tag {TagId}", id);
            return StatusCode(500, "An error occurred while updating the tag.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        var deleted = await _tagService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
