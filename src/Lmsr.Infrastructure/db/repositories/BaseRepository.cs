using Lmsr.Domain.Common;
using Lmsr.Domain.Aggregates;
using Lmsr.Application.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace Lmsr.Infrastructure.Db.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity<int>
{
private AppDbContext _context;
private DbSet<T> _dbSet;
public BaseRepository(AppDbContext context)
{
_context = context;
_dbSet = context.Set<T>();
}
public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
public void Delete(T entity) => _dbSet.Remove(entity);

public async Task<List<T>> GetAllAsync(    params Expression<Func<T, object>>[] includes)
{
IQueryable<T> query = _dbSet;
foreach (var include in includes)
{
query = query.Include(include);
}
return await query.ToListAsync();
}
public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
{
IQueryable<T> query = _dbSet;
foreach (var include in includes)
{
query = query.Include(include);
}
return await query.FirstOrDefaultAsync(e => e.Id == id);
}
}