// tests/MyOpenTelemetryApi.Application.Tests/ApplicationTests.cs
using MyOpenTelemetryApi.Application.DTOs;
using MyOpenTelemetryApi.Domain.Entities;

namespace MyOpenTelemetryApi.Application.Tests;

public class ApplicationTests
{
    [Fact]
    public void ContactDto_Initialization_SetsDefaultValues()
    {
        // Arrange & Act
        ContactDto dto = new();

        // Assert
        Assert.NotNull(dto.FirstName);
        Assert.NotNull(dto.LastName);
        Assert.NotNull(dto.EmailAddresses);
        Assert.NotNull(dto.PhoneNumbers);
        Assert.NotNull(dto.Addresses);
        Assert.NotNull(dto.Groups);
        Assert.NotNull(dto.Tags);
        Assert.Empty(dto.EmailAddresses);
        Assert.Empty(dto.PhoneNumbers);
    }

    [Fact]
    public void CreateContactDto_Initialization_SetsDefaultValues()
    {
        // Arrange & Act
        CreateContactDto dto = new();

        // Assert
        Assert.NotNull(dto.FirstName);
        Assert.NotNull(dto.LastName);
        Assert.NotNull(dto.EmailAddresses);
        Assert.NotNull(dto.PhoneNumbers);
        Assert.NotNull(dto.Addresses);
        Assert.NotNull(dto.GroupIds);
        Assert.NotNull(dto.TagIds);
    }

    [Theory]
    [InlineData("Personal", EmailType.Personal)]
    [InlineData("Work", EmailType.Work)]
    [InlineData("Other", EmailType.Other)]
    public void EmailType_ParsesCorrectly(string input, EmailType expected)
    {
        // Act
        EmailType result = Enum.Parse<EmailType>(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Mobile", PhoneType.Mobile)]
    [InlineData("Home", PhoneType.Home)]
    [InlineData("Work", PhoneType.Work)]
    [InlineData("Fax", PhoneType.Fax)]
    [InlineData("Other", PhoneType.Other)]
    public void PhoneType_ParsesCorrectly(string input, PhoneType expected)
    {
        // Act
        PhoneType result = Enum.Parse<PhoneType>(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Home", AddressType.Home)]
    [InlineData("Work", AddressType.Work)]
    [InlineData("Other", AddressType.Other)]
    public void AddressType_ParsesCorrectly(string input, AddressType expected)
    {
        // Act
        AddressType result = Enum.Parse<AddressType>(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TagDto_CanBeCreatedWithName()
    {
        // Arrange & Act
        TagDto tag = new()
        {
            Id = Guid.NewGuid(),
            Name = "Important",
            ColorHex = "#FF0000"
        };

        // Assert
        Assert.NotEqual(Guid.Empty, tag.Id);
        Assert.Equal("Important", tag.Name);
        Assert.Equal("#FF0000", tag.ColorHex);
    }

    [Fact]
    public void GroupDto_ContactCountDefaultsToZero()
    {
        // Arrange & Act
        GroupDto group = new();

        // Assert
        Assert.Equal(0, group.ContactCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Some notes")]
    public void UpdateContactDto_AcceptsVariousNoteValues(string? notes)
    {
        // Arrange & Act
        UpdateContactDto dto = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Notes = notes
        };

        // Assert
        Assert.Equal(notes, dto.Notes);
    }
}
