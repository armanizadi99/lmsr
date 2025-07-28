using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<int>>
{
private IUnitOfWork _unitOfWork;
private ICourseTitleUniquenessSpecification _titleUniquenessSpec;
private ICourseUserIdAuthenticitySpecification _userIdAuthenticitySpec;

public CreateCourseHandler(IUnitOfWork unitOfWork, ICourseTitleUniquenessSpecification titleUniquenessSpec, ICourseUserIdAuthenticitySpecification userIdAuthenticitySpec)
{
_unitOfWork = unitOfWork;
_titleUniquenessSpec = titleUniquenessSpec;
_userIdAuthenticitySpec = userIdAuthenticitySpec;
}

public async Task<Result<int>> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
{
if(!_userIdAuthenticitySpec.IsUserIdAuthentic(command.UserId))
throw new InvalidDomainOperationException("Invalid UserId. ");

List<string> errors = new List<string>();
if(!_titleUniquenessSpec.IsTitleUnique(command.Title))
errors.Add("A course with the same name exists.");
if(errors.Any())
return Result<int>.Failure(errors);
var course = new Course(command.Title, command.UserId, command.IsPrivate);
await _unitOfWork.CourseRepo.AddAsync(course);
_unitOfWork.Commit();
return Result<int>.Success(course.Id);
}
}