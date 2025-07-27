using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface IWordRepository : IBaseRepository<Word>
{
public Task<List<Word>> GetAllWords();
public Task<Word> GetWordById(int Id);
}