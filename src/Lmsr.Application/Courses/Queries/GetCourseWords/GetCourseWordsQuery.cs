namespace Lmsr.Application.Courses;
public record GetCourseWordsQuery(int CourseId) : IRequest<Result<List<WordViewModel>>>;