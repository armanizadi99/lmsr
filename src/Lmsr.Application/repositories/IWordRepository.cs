using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface IWordRepository : IBaseRepository<Word>
{
public Task<List<Word>> GetAllWordsAsync();
public Task<Word> GetWordByIdAsync(int Id);
public Task<List<Word>> GetAllWordsForCourseAsync(int courseId);
}