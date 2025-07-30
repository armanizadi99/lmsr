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
public async Task Add_ValidEntity_ShouldAddEntity<TEntity>(
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
}