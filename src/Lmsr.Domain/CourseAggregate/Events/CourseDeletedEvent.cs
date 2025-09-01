using MediatR;
namespace Lmsr.Domain.Aggregates.Events;
public class CourseDeletedEvent(int courseId) : INotification
{
public int CourseId {get; private set; } = courseId;
}