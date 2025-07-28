using System.Collections.Generic;
using System.Linq.Expressions;
namespace Lmsr.Application.Repositories;
public interface IBaseRepository<T>
{
public Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
public Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
public Task AddAsync(T entity);
public void Delete(T entity);
}