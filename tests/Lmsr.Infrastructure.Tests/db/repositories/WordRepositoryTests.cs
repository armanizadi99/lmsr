using Lmsr.Infrastructure.Db.Repositories;
using Lmsr.Infrastructure.Db;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Common;
using Xunit;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Lmsr.Infrastructure.Tests;
public class WordRepositoryTests
{
[Fact]
public async Task GetWordByIdAsync_ShouldReturnAllWordsWithLoadedEntities()
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new WordRepository(context);
var word = new Word("test", 1);
word.AddDefinition("this is a word", WordType.Verb);
await repo.AddAsync(word);
await context.SaveChangesAsync();

// Act
using var  actContext = new AppDbContext(options); // a new context so those added aggregates aren't tracked.
repo = new WordRepository(actContext);
var result = await repo.GetWordByIdAsync(1);

// Assert
result.Id.Should().Be(1);
result.Definitions.Should().NotBeEmpty();
result.Definitions.Count.Should().Be(1);
}
}