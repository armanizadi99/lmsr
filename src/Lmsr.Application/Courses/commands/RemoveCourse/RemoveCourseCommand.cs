using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record RemoveCourseCommand(int CourseId, string UserId) : IRequest;