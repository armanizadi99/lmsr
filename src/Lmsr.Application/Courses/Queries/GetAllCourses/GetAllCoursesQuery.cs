using Lmsr.Application.ViewModels;
namespace Lmsr.Application.Courses;
public record GetAllCoursesQuery(int courseId) : IRequest<Result<List<CourseViewModel>>>;