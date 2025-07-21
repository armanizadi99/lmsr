namespace Lmsr.Application.Courses;
public class CreateCourseCommand (string title, string userId, bool isPrivate) : IRequest<Result<int>>
{
public string Title = title;
public string UserId = userId;
public bool IsPrivate = isPrivate;
}
