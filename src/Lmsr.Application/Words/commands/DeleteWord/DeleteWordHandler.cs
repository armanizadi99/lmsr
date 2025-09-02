namespace Lmsr.Application.Words;
public class DeleteWordHandler : IRequestHandler<DeleteWordCommand, Result>
{
private IUnitOfWork _unitOfWork;
private IUserContext _userContext;

public DeleteWordHandler(IUnitOfWork unitOfWork, IUserContext userContext)
{
_unitOfWork = unitOfWork;
_userContext = userContext;
}
public async Task<Result> Handle(DeleteWordCommand command, CancellationToken ca)
{
var wordToDelete = await _unitOfWork.WordRepo.GetByIdAsync(command.WordId);
if(wordToDelete == null)
{
return Result.Failure(new DomainError(ErrorCodes.NotFound, "Word", "No such word exists in the database."));
}
var parentCourse = await _unitOfWork.CourseRepo.GetByIdAsync(wordToDelete.CourseId);
var userId = _userContext.UserId;
if(parentCourse.UserId != userId)
{
return Result.Failure(new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToWord", "You can't delete this word. You aren't the owner of the parent course."));
}
_unitOfWork.WordRepo.Delete(wordToDelete);
await _unitOfWork.SaveChangesAsync();
return Result.Success();
}
}