using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface IWordDefinitionRepository : IBaseRepository<WordDefinition>
{
public Task<WordDefinition> GetWordDefinitionWithWordById(int Id);
}