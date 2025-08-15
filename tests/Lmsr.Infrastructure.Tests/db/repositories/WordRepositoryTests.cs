using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Db.Repositories;
namespace Lmsr.Infrastructure.Tests;

[Collection("SqliteCollection")]
public class WordRepositoryTests
{
private readonly SqliteInMemoryFixture _fixture;
public WordRepositoryTests(SqliteInMemoryFixture fixture)
{
_fixture = fixture;
}

[Fact]
public async Task Add_Word_ShouldAddWord()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var term = "word1";
var courseId = 1;
var word = new Word(term, courseId);

// Act
await repo.AddAsync(word);
var result = context.SaveChanges();

// Assert
context.Words.Count().Should().Be(1);
result.Should().Be(1);
}

[Fact]
public async Task Add_DuplicateWordWithDifferentCourseIds_ShouldAddDuplicateWord()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var word1 = new Word("word1", 1);
var word2 = new Word("word1", 2);
await repo.AddAsync(word1);
var result1 = context.SaveChanges();

// Act
await repo.AddAsync(word2);
var result2 = context.SaveChanges();

// Assert
result1.Should().Be(1);
result2.Should().Be(1);
context.Words.Count().Should().Be(2);
}

[Fact]
public async Task Add_DuplicateWordWithSameCourseIds_ShouldThrowDbUpdateException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var word1 = new Word("word1", 1);
var word2 = new Word("word1", 1);
await repo.AddAsync(word1);
context.SaveChanges();

// Act
await repo.AddAsync(word2);
Action act = () => context.SaveChanges();

// Assert
act.Should().Throw<DbUpdateException>()
.WithInnerException<SqliteException>()
.Which.Message.Should().Be("SQLite Error 19: 'UNIQUE constraint failed: Words.CourseId, Words.Term'.");
}

[Fact]
public async Task Add_WordWithNullTerm_ShouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var sql = "INSERT INTO Words(Term, CourseId) Values({0}, {1})";
string? term = null;
int courseId = 1;

// Act
Action act = () => context.Database.ExecuteSqlRaw(sql, term, courseId);

// Assert
act.Should().Throw<SqliteException>()	
.WithMessage("SQLite Error 19: 'NOT NULL constraint failed: Words.Term'.");
}

[Fact]
public async Task Add_WordWithEmptyTerm_ShouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var sql = "INSERT INTO Words(Term, CourseId) Values({0}, {1})";
string term = "";
int courseId = 1;

// Act
Action act = () => context.Database.ExecuteSqlRaw(sql, term, courseId);

// Assert
act.Should().Throw<SqliteException>()	
.WithMessage("SQLite Error 19: 'CHECK constraint failed: CK_Word_Term_NotEmpty'.");
}

[Fact]
public async Task Delete_ExistingWord_ShouldDeleteWord()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var numberOfWords = context.Words.Count();

// Act
var wordToDelete = await repo.GetWordByIdAsync(1);
repo.Delete(wordToDelete);
var effectedRows = context.SaveChanges();

// Assert
effectedRows.Should().Be(3);
context.Words.Count().Should().Be(numberOfWords - 1);
}

[Fact]
public async Task Delete_NoneExistingWord_ShouldThrowArgumentNullException()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);

// Act
var wordToDelete = await repo.GetWordByIdAsync(5);
Action act = () => repo.Delete(wordToDelete);

// Assert
act.Should().Throw<ArgumentNullException>()
.WithMessage("Value cannot be null. (Parameter 'entity')");
}

[Fact]
public async Task Add_WordWithValidDefinitions_ShouldAddWordWithDefinitions()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var word = new Word("word1", 1);
word.AddDefinition("this is one of the definitions for word1.", WordType.Noun);
word.AddDefinition("this is another definition for word1.", WordType.Verb);

// Act
await repo.AddAsync(word);
var effectedRows = context.SaveChanges();

// Assert
effectedRows.Should().Be(3);
context.Words.Count().Should().Be(1);
context.Set<WordDefinition>().Count().Should().Be(2);
}

[Fact]
public async Task Delete_WordWithDefinitions_ShouldRemoveWordWithItsDefinitions()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);
var wordToDelete = await repo.GetWordByIdAsync(3);

// Act
repo.Delete(wordToDelete);
var effectedRows = context.SaveChanges();

// Assert
effectedRows.Should().Be(3);
context.Words.Count().Should().Be(2);
wordToDelete.Definitions.Count.Should().Be(2);
}

[Fact]
public async Task GetById_ExistingWordWithDefinitions_ShouldReturnWordWithSubentitiesNotLoaded()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);

// Act
var word = await repo.GetByIdAsync(1);

// Assert
word.Should().NotBeNull();
word.Definitions.Should().BeNull();
}

[Fact]
public async Task GetWordById_ExistingWordWithDefinitions_ShouldReturnWordWithItsDefinitionsLoaded()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);

// Act
var word = await repo.GetWordByIdAsync(1);

// Assert
word.Should().NotBeNull();
word.Definitions.Should().NotBeNull();
word.Definitions.Count.Should().Be(2);
}

[Fact]
public async Task GetAll_WordsWithDefinitions_ShouldReturnAllExistingWordsWithDefinitionsNotLoaded()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);

// Act
var words = await repo.GetAllAsync();

// Assert
words.Should().NotBeNull();
words.Count.Should().Be(3);
foreach (var word in words)
{
word.Definitions.Should().BeNull();
}
}

[Fact]
public async Task GetAllWords_WordsWithDefinitions_ShouldReturnAllExistingWordsWithDefinitionsLoaded()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedWords();
using var context = _fixture.CreateContext();
var repo = new WordRepository(context);

// Act
var words = await repo.GetAllWordsAsync();

// Assert
words.Should().NotBeNull();
words.Count.Should().Be(3);
foreach (var word in words)
{
word.Definitions.Should().NotBeNull();
word.Definitions.Count.Should().Be(2);
}
}
}