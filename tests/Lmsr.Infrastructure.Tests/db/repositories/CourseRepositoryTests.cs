using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Db.Repositories;
namespace Lmsr.Infrastructure.Tests;

[Collection("SqliteCollection")]
public class CourseRepositoryTests
{
private readonly SqliteInMemoryFixture _fixture;
public CourseRepositoryTests(SqliteInMemoryFixture fixture)
{
_fixture = fixture;
}

[Fact]
public async Task Add_ValidCourse_ShouldAddCourse()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
var title = "course1";
var userId = "123";
var isPrivate = false;
var course = new Course(title, userId, isPrivate);

// Act
await repo.AddAsync(course);
await context.SaveChangesAsync();

// Assert
context.Courses.Count().Should().Be(1);
}

[Fact]
public async Task Add_CourseWithDuplicateTitle_ShouldThrowDbException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
var title = "course1";
var userId = "123";
var isPrivate = false;
var course = new Course(title, userId, isPrivate);
var duplicateCourse = new Course(title, userId, isPrivate);
await repo.AddAsync(course);
await context.SaveChangesAsync();

// Act
await repo.AddAsync(duplicateCourse);
Action act = () => context.SaveChanges();

// Assert
act.Should().Throw<DbUpdateException>()
.WithInnerException<SqliteException>()
.Which.Message.Should().Contain("UNIQUE constraint failed: Courses.Title");
}

[Fact]
public async Task Add_CourseWithNullTitle_shouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
string? title = null;
var userId = "123";
var isPrivate = false;

// Act
var sql = "INSERT INTO Courses(Title, UserId, IsPrivate) values ({0}, {1}, {2})";
Action act = () => context.Database.ExecuteSqlRaw(sql, title, userId, isPrivate);

// Assert
act.Should().Throw<SqliteException>()
.WithMessage("SQLite Error 19: 'NOT NULL constraint failed: Courses.Title'.");
}

[Fact]
public async Task Add_CourseWithEmptyTitle_shouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
string? title = "";
var userId = "123";
var isPrivate = false;

// Act
var sql = "INSERT INTO Courses(Title, UserId, IsPrivate) values ({0}, {1}, {2})";
Action act = () => context.Database.ExecuteSqlRaw(sql, title, userId, isPrivate);

// Assert
act.Should().Throw<SqliteException>()
.WithMessage("SQLite Error 19: 'CHECK constraint failed: CK_Course_Title_NotEmpty'.");
}
[Fact]
public async Task Add_CourseWithNullUserId_shouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
string title = "course1";
string?  userId = null;
var isPrivate = false;

// Act
var sql = "INSERT INTO Courses(Title, UserId, IsPrivate) values ({0}, {1}, {2})";
Action act = () => context.Database.ExecuteSqlRaw(sql, title, userId, isPrivate);

// Assert
act.Should().Throw<SqliteException>()
.WithMessage("SQLite Error 19: 'NOT NULL constraint failed: Courses.UserId'.");
}

[Fact]
public async Task Add_CourseWithEmptyUserId_shouldThrowSqliteException()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
string title = "course1";
var userId = "";
var isPrivate = false;

// Act
var sql = "INSERT INTO Courses(Title, UserId, IsPrivate) values ({0}, {1}, {2})";
Action act = () => context.Database.ExecuteSqlRaw(sql, title, userId, isPrivate);

// Assert
act.Should().Throw<SqliteException>()
.WithMessage("SQLite Error 19: 'CHECK constraint failed: CK_Course_UserId_NotEmpty'.");
}

[Fact]
public async Task Delete_ExistingCourse_ShouldDeleteCourse()
{
// Arrange
_fixture.Cleanup();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);
await repo.AddAsync(new Course("course1", "123", false));
context.SaveChanges();
context.Courses.Count().Should().Be(1);
var course = context.Courses.FirstOrDefault();

// Act
repo.Delete(course);
context.SaveChanges();

// Assert
context.Courses.Count().Should().Be(0);
}

[Fact]
public async Task GetAllCourses_ShouldGetAllCourses()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);

// Act
var courses = await repo.GetAllCoursesAsync();

// Assert
courses.Count.Should().Be(3);
}

[Fact]
public async Task GetCourseById_ExistingCourse_ShouldGetCourse()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);

// Act
var course = await repo.GetCourseByIdAsync(1);

// Assert
course.Should().NotBeNull();
course.Title.Should().Be("course1");
course.UserId.Should().Be("arman");
course.IsPrivate.Should().BeFalse();
}

[Fact]
public async Task GetCourseById_NoneExistingCourse_ShouldReturnNull()
{
// Arrange
_fixture.Cleanup();
_fixture.SeedDb();
using var context = _fixture.CreateContext();
var repo = new CourseRepository(context);

// Act
var course = await repo.GetCourseByIdAsync(10);

// Assert
course.Should().BeNull();
}
}