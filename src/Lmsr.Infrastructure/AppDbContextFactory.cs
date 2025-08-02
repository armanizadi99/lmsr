using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Lmsr.Infrastructure.Db;

namespace Lmsr.Infrastructure;
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
public AppDbContext CreateDbContext(string[] args)
{
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

// Directly supply your SQLite connection string here
optionsBuilder.UseSqlite("Data Source=App.db");

return new AppDbContext(optionsBuilder.Options);
}
}