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
public class BaseRepositoryTests
{
public static  IEnumerable<object[]> validEntities()
{
yield return new object[] {
new Course("course1", Guid.NewGuid().ToString(), false),
(Func<AppDbContext, DbSet<Course>>)(ctx => ctx.Courses)
};
yield return new object[] {
new Word("word1", 1),
(Func<AppDbContext, DbSet<Word>>)(ctx => ctx.Words)
};
}
[Theory]
[MemberData(nameof(validEntities))]
public async Task Add_ShouldAddEntity<TEntity>(
TEntity entity,
Func<AppDbContext, DbSet<TEntity>> dbSetAccessor)
where TEntity : BaseEntity<int>
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new BaseRepository<TEntity>(context);

// Act
await repo.AddAsync(entity);
await context.SaveChangesAsync();

// Assert
dbSetAccessor(context).Count().Should().Be(1);
}

[Theory]
[MemberData(nameof(validEntities))]
public async Task Delete_ShouldDelete<TEntity>(
TEntity entity,
Func<AppDbContext, DbSet<TEntity>> dbSetAccessor)
where TEntity : BaseEntity<int>
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new BaseRepository<TEntity>(context);
await repo.AddAsync(entity);
await context.SaveChangesAsync();
dbSetAccessor(context).Count().Should().Be(1);

// Act
repo.Delete(entity);
await context.SaveChangesAsync();

// Assert
dbSetAccessor(context).Count().Should().Be(0);
}

[Fact]
public async Task GetAllAsync_Courses_ShouldReturnAllCourses()
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new BaseRepository<Course>(context);
await repo.AddAsync(new Course("course1", "arman", false));
await repo.AddAsync(new Course("course2", "hamidreza", false));
await context.SaveChangesAsync();

// Act
var courses = await repo.GetAllAsync();

// Assert
courses.Count.Should().Be(2);
}

[Fact]
public async Task GetByIdAsync_CourseExistingId_ShouldReturnCourse()
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new BaseRepository<Course>(context);
await repo.AddAsync(new Course("course1", "arman", false));
await repo.AddAsync(new Course("course2", "hamidreza", false));
await context.SaveChangesAsync();

// Fact
var course = await repo.GetByIdAsync(1);

// Assert
course.Should().NotBeNull();
course.Id.Should().Be(1);
}

[Fact]
public async Task GetByIdAsync_WordsNotLoadedDefinitions_DefinitionsShouldBeNull()
{
// Arrange
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique DB per test!
.Options;
using var context = new AppDbContext(options);
var repo = new BaseRepository<Word>(context);
var word = new Word("test", 1);
word.AddDefinition("test is a word.", WordType.Verb);
await repo.AddAsync(word);
await context.SaveChangesAsync();

// Act
using var  actContext = new AppDbContext(options); // a new context so those added aggregates aren't tracked.
repo = new BaseRepository<Word>(actContext);
var result = await repo.GetByIdAsync(1);

// Assert
result.Definitions.Should().BeNull();
}
}