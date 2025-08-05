// src/MyOpenTelemetryApi.Api/Controllers/HealthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOpenTelemetryApi.Infrastructure.Data;

namespace MyOpenTelemetryApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext dbContext, ILogger<HealthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        var health = new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "MyOpenTelemetryApi"
        };

        try
        {
            // Test database connectivity
            await _dbContext.Database.CanConnectAsync();
            return Ok(health);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(503, new
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Service = "MyOpenTelemetryApi",
                Error = "Database connection failed"
            });
        }
    }

    [HttpGet("ready")]
    public async Task<IActionResult> GetReadiness()
    {
        try
        {
            // Check if database is accessible and migrations are applied
            await _dbContext.Database.CanConnectAsync();
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                return StatusCode(503, new
                {
                    Status = "Not Ready",
                    Reason = "Database has pending migrations",
                    PendingMigrations = pendingMigrations
                });
            }

            return Ok(new
            {
                Status = "Ready",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed");
            return StatusCode(503, new
            {
                Status = "Not Ready",
                Reason = "Database check failed",
                Error = ex.Message
            });
        }
    }
}