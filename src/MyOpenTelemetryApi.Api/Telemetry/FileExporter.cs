// src/MyOpenTelemetryApi.Api/Telemetry/FileExporter.cs
using System.Text.Json;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace MyOpenTelemetryApi.Api.Telemetry;

public class FileLogExporter : BaseExporter<LogRecord>
{
    private readonly string _filePath;
    private readonly object _lockObject = new();
    private readonly JsonSerializerOptions _jsonOptions;

    public FileLogExporter(string filePath)
    {
        _filePath = filePath;
        _jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        // Ensure directory exists
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public override ExportResult Export(in Batch<LogRecord> batch)
    {
        try
        {
            lock (_lockObject)
            {
                using var writer = new StreamWriter(_filePath, append: true);
                
                foreach (var logRecord in batch)
                {
                    var logEntry = new
                    {
                        Timestamp = logRecord.Timestamp,
                        TraceId = logRecord.TraceId.ToString(),
                        SpanId = logRecord.SpanId.ToString(),
                        TraceFlags = logRecord.TraceFlags.ToString(),
                        CategoryName = logRecord.CategoryName,
                        LogLevel = logRecord.LogLevel.ToString(),
                        FormattedMessage = logRecord.FormattedMessage,
                        Body = logRecord.Body,
                        ScopeValues = ExtractScopeValues(logRecord),
                        Exception = logRecord.Exception?.ToString(),
                        Attributes = ExtractAttributes(logRecord)
                    };
                    
                    var json = JsonSerializer.Serialize(logEntry, _jsonOptions);
                    writer.WriteLine(json);
                }
            }
            
            return ExportResult.Success;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting logs to file: {ex.Message}");
            return ExportResult.Failure;
        }
    }

    private List<object> ExtractScopeValues(LogRecord logRecord)
    {
        var scopes = new List<object>();
        
        logRecord.ForEachScope((scope, state) =>
        {
            if (!scope.Equals(default(LogRecordScope)))
            {
                scopes.Add(scope.ToString() ?? "null");
            }
        }, scopes);
        
        return scopes;
    }

    private Dictionary<string, object?> ExtractAttributes(LogRecord logRecord)
    {
        var attributes = new Dictionary<string, object?>();
        
        if (logRecord.Attributes != null)
        {
            foreach (var attribute in logRecord.Attributes)
            {
                attributes[attribute.Key] = attribute.Value;
            }
        }
        
        return attributes;
    }
}
