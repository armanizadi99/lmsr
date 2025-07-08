using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, List<Course>>
{
private ICourseRepository _repository;
private ILogger _logger;
public GetCoursesHandler(ICourseRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task<List<Course>> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
{
_logger.LogInformation("Handled GetCourses query.");
return await _repository.GetAll();
}
}