using Lmsr.Domain.Entities;
namespace Lmsr.Application.Repositories;
public interface IWordDefinitionRepository : IBaseRepository<WordDefinition>
{
public Task<WordDefinition> GetWordDefinitionWithWordById(int Id);
}