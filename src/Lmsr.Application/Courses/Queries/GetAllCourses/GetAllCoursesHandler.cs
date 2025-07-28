using Lmsr.Application.ViewModels;
using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Courses;
public class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, Result<List<CourseViewModel>>>
{
private ICourseRepository _repository;

public GetAllCoursesHandler(ICourseRepository repository)
{
_repository = repository;
}
public async Task<Result<List<CourseViewModel>>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken)
{
var courses = await _repository.GetAllCoursesAsync();
return Result<List<CourseViewModel>>.Success(courses.Select(c => new CourseViewModel(c.Id, c.Title, c.UserId)).ToList());
}
}