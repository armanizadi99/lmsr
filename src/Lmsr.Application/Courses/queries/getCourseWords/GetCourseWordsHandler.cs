using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class GetCourseWordsHandler : IRequestHandler<GetCourseWordsQuery, List<Word>>
{
private ICourseRepository _repository;
private ILogger _logger;
public GetCourseWordsHandler(ICourseRepository repository, ILogger logger)
{
_repository=repository;
_logger=logger;
}
public async Task<List<Word>> Handle(GetCourseWordsQuery query, CancellationToken cancellationToken)
{
var course = await _repository.GetCourseWithWordsById(query.CourseId);;
if(course == null)
throw new InvalidOperationException("Invalid CourseId.");
_logger.LogInformation("Handled GetCourseWords query.");
return course.Words.ToList();
}
}