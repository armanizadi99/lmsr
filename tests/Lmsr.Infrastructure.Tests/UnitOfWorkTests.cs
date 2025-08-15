using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure;
namespace Lmsr.Infrastructure.Tests;

[Collection("SqliteCollection")]
public class UnitOfWorkTests
{
private readonly SqliteInMemoryFixture _fixture;
public UnitOfWorkTests(SqliteInMemoryFixture fixture)
{
_fixture = fixture;
}
[Fact]
public async Task SaveChanges_ShouldSaveChanges()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var unitOfWork = new UnitOfWork(context);
await unitOfWork.CourseRepo.AddAsync(new Course("course1", "arman", false));
await unitOfWork.WordRepo.AddAsync(new Word("word1", 1));

// Act
await unitOfWork.SaveChangesAsync();

// Assert
context.Courses.Count().Should().Be(1);
context.Words.Count().Should().Be(1);
}
}