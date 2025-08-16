namespace Lmsr.Application.Courses;
public class AddWordToCourseCommand(string term, int courseId) : IRequest<Result<Word>>
{
public string Term{get; private set; } = term;
public int CourseId{get; private set; } = courseId;
}
