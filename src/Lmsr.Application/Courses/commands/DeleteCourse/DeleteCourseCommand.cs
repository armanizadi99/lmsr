namespace Lmsr.Application.Courses;
public class DeleteCourseCommand(int courseId) : IRequest<Result>
{
public int CourseId {get; private set; } = courseId;
}