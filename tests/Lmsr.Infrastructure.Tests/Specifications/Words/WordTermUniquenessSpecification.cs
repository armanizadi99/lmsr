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
[Theory]
[InlineData("unique")]
[InlineData("Unique")]
[InlineData("UNIQUE")]
public void IsWordTermUnique_UniqueWord_ShouldReturnTrue(string uniqueWord)
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);

// Act Assert
spec.IsWordTermUnique(uniqueWord, 1).Should().BeTrue();
}

[Theory]
[InlineData("word1")]
[InlineData("Word1")]
public void IsWordTermUnique_DuplicateWord_ShouldReturnFalse(string duplicateWord )
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);

// Act Assert
spec.IsWordTermUnique("word1", 1).Should().BeFalse();
}

[Theory]
[InlineData("word1")]
[InlineData("Word1")]
[InlineData("WORD1")]
public void IsWordTermUnique_DuplicateWordButInADifferentCourse_ShouldReturnTrue(string uniqueWordInCourse )
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var spec = new WordTermUniquenessSpecification(context);

// Act Assert
spec.IsWordTermUnique(uniqueWordInCourse, 4).Should().BeTrue();
}
}