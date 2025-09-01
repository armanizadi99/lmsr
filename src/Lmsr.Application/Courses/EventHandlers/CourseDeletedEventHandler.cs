namespace Lmsr.Application.Courses;
public class CourseDeletedEventHandler : INotificationHandler<CourseDeletedEvent>
{
private readonly IWordRepository _repository;
public CourseDeletedEventHandler(IWordRepository repository)
{
_repository = repository;
}
public Task Handle(CourseDeletedEvent notification, CancellationToken ca)
{
Console.WriteLine("handled ther course deleted event.");
return Task.CompletedTask;
}
}