using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record RemoveWordFromCourseCommand (int CourseId, int WordId, string UserId) : IRequest;