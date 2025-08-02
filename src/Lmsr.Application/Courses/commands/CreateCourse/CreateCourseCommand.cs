namespace Lmsr.Application.Courses;
public class CreateCourseCommand (string title, string userId, bool isPrivate) : IRequest<Result<int>>
{
public string Title{get; private set; } = title;
public string UserId{get; private set; } = userId;
public bool IsPrivate {get; private set; } = isPrivate;
}
