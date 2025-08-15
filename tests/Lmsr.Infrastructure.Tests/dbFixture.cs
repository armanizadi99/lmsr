using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Lmsr.Infrastructure.Db;

namespace Lmsr.Infrastructure.Tests;

public class SqliteInMemoryFixture : IDisposable
{
private readonly SqliteConnection _connection;

public SqliteInMemoryFixture()
{
_connection = new SqliteConnection("DataSource=:memory:");
_connection.Open(); // Keeps the in-memory DB alive during tests
}

public AppDbContext CreateContext()
{
var options = new DbContextOptionsBuilder<AppDbContext>()
.UseSqlite(_connection)
.Options;

var context = new AppDbContext(options);
context.Database.EnsureCreated();
return context;
}

public void Cleanup()
{
using var context = CreateContext();
context.Courses.RemoveRange(context.Courses);
context.Words.RemoveRange(context.Words);
context.SaveChanges();
}
public void SeedDb()
{
string sql = "INSERT INTO Courses(Id, Title, UserId, IsPrivate) VALUES (1, \"course1\", \"arman\", false), (2, \"course2\", \"arman\", true), (3, \"course3\", \"hamidreza\", false);";
using var context = CreateContext();
context.Database.ExecuteSqlRaw(sql);
}
public void SeedWords()
{
string sqlWords = "INSERT INTO Words(Id, Term, CourseId) VALUES (1, \"word1\", 1), (2, \"word2\", 1), (3, \"word3\", 2);";
string sqlWord1Definitions = "INSERT INTO WordDefinition(Id, Text, Type, WordId) VALUES(1, \"def1\", 1, 1), (2, \"def2\", 2, 1);";
string sqlWord2Definitions = "INSERT INTO WordDefinition(Id, Text, Type, WordId) VALUES(3, \"def1\", 1, 2), (4, \"def2\", 2, 2);";
string sqlWord3Definitions = "INSERT INTO WordDefinition(Id, Text, Type, WordId) VALUES(5, \"def1\", 1, 3), (6, \"def2\", 2, 3);";
using var context = CreateContext();
context.Database.ExecuteSqlRaw(sqlWords);
context.Database.ExecuteSqlRaw(sqlWord1Definitions);
context.Database.ExecuteSqlRaw(sqlWord2Definitions);
context.Database.ExecuteSqlRaw(sqlWord3Definitions);
}
public void Dispose()
{
_connection.Dispose();
}
}