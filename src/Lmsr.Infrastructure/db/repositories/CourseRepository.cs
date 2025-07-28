using Lmsr.Domain.Common;
using Lmsr.Application.Repositories;
using Lmsr.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
namespace Lmsr.Infrastructure.Db.Repositories;
public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
private AppDbContext _context;
private DbSet<Course> _dbSet;

public CourseRepository(AppDbContext context) : base(context)
{
_context=context;
_dbSet = context.Set<Course>();
}
public async Task<List<Course>> GetAllCoursesAsync()
{
return await GetAllAsync();
}
public async Task<Course> GetCourseByIdAsync(int id)
{
return await GetByIdAsync(id);
}
}