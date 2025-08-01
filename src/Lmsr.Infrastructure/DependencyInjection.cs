using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // For IHostEnvironment
using Lmsr.Application.Interfaces;  // Example interfaces location
using Lmsr.Application.Repositories;
using Lmsr.Infrastructure.Db; // AppDbContext location
using Lmsr.Infrastructure.Db.Repositories; // Repo implementations
namespace Lmsr.Infrastructure;
public static class DependencyInjection
{
public static IServiceCollection AddInfrastructure(
this IServiceCollection services,
IConfiguration configuration,
IHostEnvironment environment)
{
// Configure DbContext depending on environment
if (environment.IsDevelopment())
{
services.AddDbContext<AppDbContext>(options =>
options.UseSqlite(configuration.GetConnectionString("SqliteConnection")));
}
else if(environment.IsProduction())
{
services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
}

services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ICourseRepository, CourseRepository>();
services.AddScoped<IWordRepository, WordRepository>();
services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

return services;
}
}