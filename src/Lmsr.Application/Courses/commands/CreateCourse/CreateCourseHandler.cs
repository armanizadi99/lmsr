using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<int>>
{
private IUnitOfWork _unitOfWork;
private ICourseTitleUniquenessSpecification _titleUniquenessSpec;
private IUserContext _userContext;

public CreateCourseHandler(IUnitOfWork unitOfWork, ICourseTitleUniquenessSpecification titleUniquenessSpec, IUserContext userContext)
{
_unitOfWork = unitOfWork;
_titleUniquenessSpec = titleUniquenessSpec;
_userContext=userContext;
}

public async Task<Result<int>> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
{
var userId = _userContext.UserId;
if(string.IsNullOrEmpty(userId))
{
return Result<int>.Failure(new DomainError(ErrorCodes.NotAuthorized, "Authorization", "You aren't authorized."));
}
if(!_titleUniquenessSpec.IsTitleUnique(command.Title))
{
return Result<int>.Failure(new DomainError(ErrorCodes.DuplicateEntity, "Course", "A course with the same name exists."));
}

var course = new Course(command.Title, userId, command.IsPrivate);
await _unitOfWork.CourseRepo.AddAsync(course);
await _unitOfWork.SaveChangesAsync();
return Result<int>.Success(course.Id);
}
}