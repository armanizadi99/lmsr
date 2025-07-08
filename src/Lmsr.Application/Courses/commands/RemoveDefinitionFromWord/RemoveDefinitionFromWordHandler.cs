using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class RemoveDefinitionFromWordHandler : IRequestHandler<RemoveDefinitionFromWordCommand>
{
private IWordRepository _wRepository;
private IWordDefinitionRepository _wdRepository;
private ILogger _logger;
public RemoveDefinitionFromWordHandler(IWordRepository wRepository, IWordDefinitionRepository wdRepository, ILogger logger)
{
_wRepository = wRepository;
_wdRepository = wdRepository;
_logger = logger;
}
public async Task Handle(RemoveDefinitionFromWordCommand command, CancellationToken cancellationToken)
{
var wordDefinition = await _wdRepository.GetById(command.WordDefinitionId);
if(wordDefinition == null)
throw new InvalidOperationException("Invalid WordDefinitionId.");
var word = await _wRepository.GetWordWithCourseById(wordDefinition.WordId);
var course = word.Course;
if(course.UserId != command.UserId)
throw new InvalidOperationException("User not Authorized.");
await _wdRepository.Remove(wordDefinition);
await _wdRepository.SaveChanges();
_logger.LogInformation("removed WordDefinition {@WordDefinition} from Word {@Word}.", wordDefinition, word);
}
}