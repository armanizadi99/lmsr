using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class RemoveWordFromCoursehandler : IRequestHandler<RemoveWordFromCourseCommand>
{
private IWordRepository _repository;
private ILogger _logger;
public async Task RemoveWordFromCourseHandler(IWordRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task Handle(RemoveWordFromCourseCommand command, CancellationToken cancellationToken)
{
var word = await _repository.GetById(command.WordId);
if(word == null)
throw new InvalidOperationException("Invalid WordId.");
if(word.Course.UserId != command.UserId)
throw new InvalidOperationException("User not Authorized.");
await _repository.Remove(word);
await _repository.SaveChanges();
_logger.LogInformation("Removed Word {@Word} from Course {@Course}.", word, word.Course);
}
}