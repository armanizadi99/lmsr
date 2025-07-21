using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<int>>
{
private ICourseRepository _repo;
private ICourseTitleUniquenessSpecification _titleUniquenessSpec;
private ICourseUserIdAuthenticitySpecification _userIdAuthenticitySpec;

public CreateCourseHandler(ICourseRepository repo, ICourseTitleUniquenessSpecification titleUniquenessSpec, ICourseUserIdAuthenticitySpecification userIdAuthenticitySpec)
{
_repo = repo;
_titleUniquenessSpec = _titleUniquenessSpec;
_userIdAuthenticitySpec = userIdAuthenticitySpec;
}

public async Task<Result<int>> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
{
if(!_userIdAuthenticitySpec.IsUserIdAuthentic(command.UserId))
throw new InvalidDomainOperationException("Invalid UserId. ");
List<string> errors = new List<string>();
if(!_courseUniquenessSpec.IsTitleUnique(command.Title))
errors.Add("A course with the same name exists.");
if(errors.Any())
return Result<int>.Failure(errors);
var course = new Course(command.Title, command.UserId, command.IsPrivate);
await _repo.Add(course);
await _repo.SaveChanges();
return Result<int>.Success(course.Id);
}
}