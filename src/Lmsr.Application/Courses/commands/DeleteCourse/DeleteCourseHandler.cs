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
if(string.IsNullOrEmpty(userId))
{
return Result.Failure(new DomainError(ErrorCodes.NotAuthorized, "Authorization", "You aren't authorized."));
}
var courseToDelete  = await _unitOfWork.CourseRepo.GetByIdAsync(command.CourseId);
if(courseToDelete == null)
{
return Result.Failure(new DomainError(ErrorCodes.NotFound, "Course", $"A course with id {command.CourseId} hasn't been found."));
}
if(courseToDelete.UserId != userId)
{
return Result.Failure(new DomainError(ErrorCodes.NotAuthorized, "Course", "You can't delete this resource. You aren't the owner."));
}
_unitOfWork.CourseRepo.Delete(courseToDelete);
await _unitOfWork.SaveChangesAsync();
return Result.Success();
}
}