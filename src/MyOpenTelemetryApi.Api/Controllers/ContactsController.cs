// src/MyOpenTelemetryApi.Api/Controllers/ContactsController.cs
using Microsoft.AspNetCore.Mvc;
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Application.Services;

namespace MyOpenTelemetryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactService contactService, ILogger<ContactsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<ContactSummaryDto>>> GetContacts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters.");
        }

        PaginatedResultDto<ContactSummaryDto> result = await contactService.GetPaginatedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(Guid id)
    {
        ContactDto? contact = await contactService.GetWithDetailsAsync(id);
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

        List<ContactSummaryDto> results = await contactService.SearchAsync(q);
        return Ok(results);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetContactsByGroup(Guid groupId)
    {
        List<ContactSummaryDto> contacts = await contactService.GetByGroupAsync(groupId);
        return Ok(contacts);
    }

    [HttpGet("tag/{tagId}")]
    public async Task<ActionResult<List<ContactSummaryDto>>> GetContactsByTag(Guid tagId)
    {
        List<ContactSummaryDto> contacts = await contactService.GetByTagAsync(tagId);
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto dto)
    {
        try
        {
            ContactDto contact = await contactService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating contact");
            return StatusCode(500, "An error occurred while creating the contact.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDto>> UpdateContact(Guid id, UpdateContactDto dto)
    {
        try
        {
            ContactDto? contact = await contactService.UpdateAsync(id, dto);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating contact {ContactId}", id);
            return StatusCode(500, "An error occurred while updating the contact.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        bool deleted = await contactService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}