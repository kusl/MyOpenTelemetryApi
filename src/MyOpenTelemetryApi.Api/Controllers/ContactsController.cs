// src/MyOpenTelemetryApi.Api/Controllers/ContactsController.cs
using Microsoft.AspNetCore.Mvc;
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Application.Services;

namespace MyOpenTelemetryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<ContactSummaryDto>>> GetContacts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters.");
        }

        var result = await _contactService.GetPaginatedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(Guid id)
    {
        var contact = await _contactService.GetWithDetailsAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return Ok(contact);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<ContactSummaryDto>>> SearchContacts([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Search term is required.");
        }

        var results = await _contactService.SearchAsync(q);
        return Ok(results);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetContactsByGroup(Guid groupId)
    {
        var contacts = await _contactService.GetByGroupAsync(groupId);
        return Ok(contacts);
    }

    [HttpGet("tag/{tagId}")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetContactsByTag(Guid tagId)
    {
        var contacts = await _contactService.GetByTagAsync(tagId);
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto dto)
    {
        try
        {
            var contact = await _contactService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contact");
            return StatusCode(500, "An error occurred while creating the contact.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDto>> UpdateContact(Guid id, UpdateContactDto dto)
    {
        try
        {
            var contact = await _contactService.UpdateAsync(id, dto);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contact {ContactId}", id);
            return StatusCode(500, "An error occurred while updating the contact.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        var deleted = await _contactService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}