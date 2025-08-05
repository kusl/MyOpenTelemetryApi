// src/MyOpenTelemetryApi.Infrastructure/Data/Configurations/EmailAddressConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOpenTelemetryApi.Domain.Entities;

namespace MyOpenTelemetryApi.Infrastructure.Data.Configurations;

public class EmailAddressConfiguration : IEntityTypeConfiguration<EmailAddress>
{
    public void Configure(EntityTypeBuilder<EmailAddress> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(254); // Standard email max length

        builder.Property(e => e.Type)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
