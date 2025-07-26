using Lmsr.Application.ViewModels;
namespace Lmsr.Application.Courses;
public record GetAllCoursesQuery() : IRequest<Result<List<CourseViewModel>>>;