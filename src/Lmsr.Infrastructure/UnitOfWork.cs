using Lmsr.Application.Repositories;
using Lmsr.Application.Interfaces;
using Lmsr.Infrastructure.Db;
using Lmsr.Infrastructure.Db.Repositories;
namespace Lmsr.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
private AppDbContext _context;
public ICourseRepository CourseRepo {get; private set; }
public IWordRepository WordRepo { get; private set; }

public UnitOfWork(AppDbContext context)
{
_context = context;
CourseRepo = new CourseRepository(context);
WordRepo = new WordRepository(context);
}

public async Task SaveChangesAsync()
{
await _context.SaveChangesAsync();
}
}