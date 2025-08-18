using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Specifications;
namespace Lmsr.Infrastructure.Tests;

[Collection("SqliteCollection")]
public class CourseTitleUniquenessSpecificationTests
{
private readonly SqliteInMemoryFixture _fixture;
public CourseTitleUniquenessSpecificationTests(SqliteInMemoryFixture fixture)
{
_fixture = fixture;
}
[Theory]
[InlineData("english")]
[InlineData("English")]
[InlineData("ENGLISH")]
public void IsTitleUnique_UniqueTitle_ShouldReturnTrue(string uniqueTitle)
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var spec = new CourseTitleUniquenessSpecification(context);

// Act
var result = spec.IsTitleUnique(uniqueTitle);

// Assert
result.Should().BeTrue();
}

[Theory]
[InlineData("course1")]
[InlineData("Course1")]
[InlineData("COURSE1")]
public void IsTitleUnique_DuplicateTitle_ShouldReturnFalse(string duplicateTitle)
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var spec = new CourseTitleUniquenessSpecification(context);

// Act
var result = spec.IsTitleUnique(duplicateTitle);

// Assert
result.Should().BeFalse();
}
}