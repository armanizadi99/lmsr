namespace Lmsr.Application.Courses;

public record CreateCourseCommand(string Title, string UserId) : IRequest<int>;