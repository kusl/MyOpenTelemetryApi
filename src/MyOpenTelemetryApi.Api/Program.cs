// src/MyOpenTelemetryApi.Api/Program.cs - Updated version with configuration-based setup
using Microsoft.EntityFrameworkCore;
using MyOpenTelemetryApi.Application.Services;
using MyOpenTelemetryApi.Domain.Interfaces;
using MyOpenTelemetryApi.Infrastructure.Data;
using MyOpenTelemetryApi.Infrastructure.Repositories;
using MyOpenTelemetryApi.Api.Telemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Define service name and version for OpenTelemetry
var serviceName = builder.Configuration.GetValue<string>("OpenTelemetry:ServiceName") ?? "MyOpenTelemetryApi";
var serviceVersion = builder.Configuration.GetValue<string>("OpenTelemetry:ServiceVersion") ??
                    Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

// Configure OpenTelemetry Resource
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
    .AddTelemetrySdk()
    .AddAttributes(new Dictionary<string, object>
    {
        ["environment"] = builder.Environment.EnvironmentName,
        ["deployment.environment"] = builder.Environment.EnvironmentName,
        ["host.name"] = Environment.MachineName
    });

// Configure OpenTelemetry Logging
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;

    // Console exporter
    if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:Console:Enabled"))
    {
        options.AddConsoleExporter();
    }

    // File exporter
    if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:File:Enabled"))
    {
        var logPath = builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:File:LogPath")
                     ?? "logs/otel-logs.json";
        options.AddFileExporter(logPath);
    }

    // OTLP exporter
    if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:OTLP:Enabled"))
    {
        options.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Endpoint")
                                          ?? "http://localhost:4317");
            var protocol = builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Protocol") ?? "Grpc";
            otlpOptions.Protocol = protocol == "Grpc" ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
        });
    }
});

// Configure OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = (httpContext) => !httpContext.Request.Path.StartsWithSegments("/health");
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
                options.SetDbStatementForStoredProcedure = true;
            })
            .AddSource("MyOpenTelemetryApi.*"); // Add custom activity sources

        // Configure sampling
        var alwaysOn = builder.Configuration.GetValue<bool>("OpenTelemetry:Sampling:AlwaysOn");
        if (alwaysOn)
        {
            tracing.SetSampler(new AlwaysOnSampler());
        }
        else
        {
            var ratio = builder.Configuration.GetValue<double>("OpenTelemetry:Sampling:Ratio");
            tracing.SetSampler(new TraceIdRatioBasedSampler(ratio));
        }

        // Configure exporters
        if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:Console:Enabled"))
        {
            tracing.AddConsoleExporter();
        }

        if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:OTLP:Enabled"))
        {
            tracing.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Endpoint")
                                          ?? "http://localhost:4317");
                var protocol = builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Protocol") ?? "Grpc";
                options.Protocol = protocol == "Grpc" ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
            });
        }
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddMeter("MyOpenTelemetryApi.*"); // Add custom meters

        // Configure exporters
        if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:Console:Enabled"))
        {
            metrics.AddConsoleExporter();
        }

        if (builder.Configuration.GetValue<bool>("OpenTelemetry:Exporter:OTLP:Enabled"))
        {
            metrics.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Endpoint")
                                          ?? "http://localhost:4317");
                var protocol = builder.Configuration.GetValue<string>("OpenTelemetry:Exporter:OTLP:Protocol") ?? "Grpc";
                options.Protocol = protocol == "Grpc" ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
            });
        }
    });

// Add services to the container.
builder.Services.AddControllers();

// Configure PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Register application services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITagService, TagService>();

// Add HTTP context accessor for tracing context
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Add custom middleware for request tracing
app.Use(async (context, next) =>
{
    using var activity = Activity.Current;
    if (activity != null)
    {
        activity.SetTag("http.request.body.size", context.Request.ContentLength ?? 0);
        activity.SetTag("user.agent", context.Request.Headers.UserAgent.ToString());
        activity.SetTag("client.ip", context.Connection.RemoteIpAddress?.ToString());
    }

    await next();

    if (activity != null)
    {
        activity.SetTag("http.response.body.size", context.Response.ContentLength ?? 0);
    }
});

app.UseAuthorization();

app.MapControllers();

// Apply migrations on startup (optional - remove in production)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        using var activity = Activity.Current?.Source.StartActivity("DatabaseMigration");
        try
        {
            logger.LogInformation("Applying database migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying database migrations");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }
}

app.Logger.LogInformation("Starting {ServiceName} version {ServiceVersion}", serviceName, serviceVersion);

app.Run();