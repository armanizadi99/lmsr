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
[Fact]
public void IsTitleUnique_UniqueTitle_ShouldReturnTrue()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var spec = new CourseTitleUniquenessSpecification(context);

// Act
var result = spec.IsTitleUnique("english");

// Assert
result.Should().BeTrue();
}

[Fact]
public void IsTitleUnique_DuplicateTitle_ShouldReturnFalse()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var spec = new CourseTitleUniquenessSpecification(context);

// Act
var result = spec.IsTitleUnique("course1");

// Assert
result.Should().BeFalse();
}
}