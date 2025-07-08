using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record GetCourseWordsQuery(int CourseId) : IRequest<List<Word>>;