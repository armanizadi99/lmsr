using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class AddWordDefinitionHandler : IRequestHandler<AddWordDefinitionCommand, int>
{
private IWordDefinitionRepository _wdRepository;
private IWordRepository _wRepository;
private ILogger _logger;
public AddWordDefinitionHandler(IWordDefinitionRepository wdRepository, IWordRepository wRepository, ILogger logger)
{
_wdRepository = wdRepository;
_wRepository = wRepository;
_logger=logger;
}
public async Task<int> Handle(AddWordDefinitionCommand command, CancellationToken cancellationToken)
{
// get the word by id, along with the course eager loaded.
var word = await _wRepository.GetWordWithCourseById(command.WordId);
if(word == null)
throw new InvalidOperationException("Invalid WordId.");
if(word.Course.UserId != command.UserId)
throw new InvalidOperationException("User not Authorized.");
var definition = new WordDefinition(command.Text, command.WordType, word);
await _wdRepository.Add(definition);
await _wdRepository.SaveChanges();
_logger.LogInformation("added word definition {@WordDefinition} to word {@Word}.", definition, word);
return definition.Id;
}
}