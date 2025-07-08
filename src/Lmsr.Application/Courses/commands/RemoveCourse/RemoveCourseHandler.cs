using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class RemoveCourseHandler : IRequestHandler<RemoveCourseCommand>
{
private ICourseRepository _repository;
private ILogger _logger;
public RemoveCourseHandler(ICourseRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task Handle(RemoveCourseCommand command, CancellationToken cancellationToken)
{
var course = await _repository.GetById(command.CourseId);
if(course == null)
throw new InvalidOperationException("Invalid CourseId.");
if(course.UserId != command.UserId)
throw new InvalidOperationException("User not authorized.");
await _repository.Remove(course);
await _repository.SaveChanges();
_logger.LogInformation("Removed course {@Course}.", course);
}
}