using Lmsr.Domain.Entities;
namespace Lmsr.Application.Repositories;
public interface IWordRepository : IBaseRepository<Word>
{
public Task<Word> GetWordWithCourseById(int id);
public Task<Word> GetWordWithDefinitionsById(int id);
}