namespace Lmsr.Application.Courses;
public record AddWordCommand(string Term, int CourseId, string UserId) : IRequest<int>;