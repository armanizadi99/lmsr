using Lmsr.Domain.Common;
using Lmsr.Application.Repositories;
using Lmsr.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
namespace Lmsr.Infrastructure.Db.Repositories;
public class WordRepository : BaseRepository<Word>, IWordRepository
{
private AppDbContext _context;
private DbSet<Word> _dbSet;

public WordRepository(AppDbContext context) : base(context)
{
_context=context;
_dbSet = context.Set<Word>();
}
public async Task<List<Word>> GetAllWordsAsync()
{
return await GetAllAsync(w => w.Definitions);
}
public async Task<Word> GetWordByIdAsync(int id)
{
return await GetByIdAsync(id, w => w.Definitions);
}
public async Task<List<Word>> GetAllWordsForCourseAsync(int courseId)
{
return await _dbSet
.Include(w => w.Definitions)
.Where(w => w.CourseId == courseId)
.ToListAsync();
}
}