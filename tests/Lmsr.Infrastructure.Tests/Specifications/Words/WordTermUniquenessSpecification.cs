using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Specifications;
namespace Lmsr.Infrastructure.Tests;

[Collection("SqliteCollection")]
public class WordTermUniquenessSpecificationTests
{
private readonly SqliteInMemoryFixture _fixture;
public WordTermUniquenessSpecificationTests(SqliteInMemoryFixture fixture)
{
_fixture = fixture;
}
[Fact]
public void IsWordTermUnique_UniqueWord_ShouldReturnTrue()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);
var uniqueWord = "unique";

// Act Assert
spec.IsWordTermUnique(uniqueWord, 1).Should().BeTrue();
}

[Fact]
public void IsWordTermUnique_DuplicateWord_ShouldReturnFalse()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);
var duplicateWord = "word1";

// Act Assert
spec.IsWordTermUnique("word1", 1).Should().BeFalse();
}

[Fact]
public void IsWordTermUnique_DuplicateWordButInADifferentCourse_ShouldReturnTrue()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);
var uniqueWordInCourse = "word1";

// Act Assert
spec.IsWordTermUnique(uniqueWordInCourse, 4).Should().BeTrue();
}
}