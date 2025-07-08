using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class GetWordDefinitionsHandler : IRequestHandler<GetWordDefinitionsQuery, List<WordDefinition>>
{
private IWordRepository _repository;
private ILogger _logger;
public GetWordDefinitionsHandler(IWordRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task<List<WordDefinition>> Handle(GetWordDefinitionsQuery query, CancellationToken cancellationToken)
{
var word = await _repository.GetWordWithDefinitionsById(query.WordId);
if(word == null)
throw new InvalidOperationException("Invalid WordId.");
_logger.LogInformation("Handled GetWordDefinitions query.");
return word.WordDefinitions.ToList();
}
}