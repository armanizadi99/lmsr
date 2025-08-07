namespace Lmsr.Application.Courses;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
{
private IUnitOfWork _unitOfWork;
private IUserContext _userContext;

public DeleteCourseHandler(IUnitOfWork unitOfWork, IUserContext userContext)
{
_unitOfWork = unitOfWork;
_userContext = userContext;
}
public async Task<Result> Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
{
var userId = _userContext.UserId;
List<DomainError> errors = new List<DomainError>();
if(string.IsNullOrEmpty(userId))
{
errors.Add(new DomainError(ErrorCodes.NotAuthorized, "Authorization", "You aren't authorized."));
return Result.Failure(errors);
}
var courseToDelete  = await _unitOfWork.CourseRepo.GetByIdAsync(command.CourseId);
if(courseToDelete == null)
{
errors.Add(new DomainError(ErrorCodes.NotFound, "Course", $"A course with id {command.CourseId} hasn't been found."));
return Result.Failure(errors);
}
if(courseToDelete.UserId != userId)
{
errors.Add(new DomainError(ErrorCodes.NotAuthorized, "Course", "You can't delete this resource. You aren't the owner."));
return Result.Failure(errors);
}
_unitOfWork.CourseRepo.Delete(courseToDelete);
await _unitOfWork.SaveChangesAsync();
return Result.Success();
}
}