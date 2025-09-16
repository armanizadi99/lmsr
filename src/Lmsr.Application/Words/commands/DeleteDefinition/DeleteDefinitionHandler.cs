namespace Lmsr.Application.Words;
public class DeleteDefinitionHandler : IRequestHandler<DeleteDefinitionCommand, Result>
{
private IUnitOfWork _unitOfWork;
private IUserContext _userContext;

public DeleteDefinitionHandler(IUnitOfWork unitOfWork, IUserContext userContext)
{
_unitOfWork = unitOfWork;
_userContext = userContext;
}
public async Task<Result> Handle(DeleteDefinitionCommand command, CancellationToken ca)
{
var parentWord = await _unitOfWork.WordRepo.GetWordByIdAsync(command.WordId);
if(parentWord == null)
{
return Result.Failure(new DomainError(ErrorCodes.NotFound, "Word", "Such a word doesn't exist."));
}
var parentCourse = await _unitOfWork.CourseRepo.GetCourseByIdAsync(parentWord.CourseId);
var userId = _userContext.UserId;
if(parentCourse.UserId != userId)
{
return Result.Failure(new DomainError(ErrorCodes.NotAuthorized, "Unauthorized  access to course.", "You can't modify this Word. You aren't the owner of the parent course."));
}
var result = parentWord.RemoveDefinition(command.DefinitionId);
if(!result.IsSuccess)
{
return Result.Failure(result.Error);
}
await _unitOfWork.SaveChangesAsync();
return Result.Success();
}
}