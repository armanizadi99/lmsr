namespace Lmsr.Application.Courses;
public class CreateCourseCommand (string title, bool isPrivate) : IRequest<Result<int>>
{
public string Title{get; private set; } = title;
public bool IsPrivate {get; private set; } = isPrivate;
}
