using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, int>
{
private ICourseRepository _repository;
private ILogger _logger;
public CreateCourseHandler(ICourseRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task<int> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
{
var course = new Course(command.Title, command.UserId);
await _repository.Add(course);
await _repository.SaveChanges();
_logger.LogInformation("added a new course with title {title} and UserId {UserId}.", command.Title, command.UserId);
return course.Id;
}
}