using System.Collections.Generic;
namespace Lmsr.Application.Repositories;
public interface IBaseRepository<T>
{
public Task<T> GetById(int id);
public Task<List<T>> GetAll();
public Task Add(T entity);
public Task Remove(T entity);
public Task SaveChanges();
}