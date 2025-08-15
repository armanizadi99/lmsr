using Xunit;
namespace Lmsr.Infrastructure.Tests;

[CollectionDefinition("SqliteCollection")]
public class SqliteCollection : ICollectionFixture<SqliteInMemoryFixture> { }