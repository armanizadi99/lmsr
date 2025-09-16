namespace Lmsr.Application.Words;
public class AddDefinitionHandler : IRequestHandler<AddDefinitionCommand, Result<WordDefinitionViewModel>>
{
private IUnitOfWork _unitOfWork;
private IUserContext _userContext;

public AddDefinitionHandler(IUnitOfWork unitOfWork, IUserContext userContext)
{
_unitOfWork = unitOfWork;
_userContext = userContext;
}
public async Task<Result<WordDefinitionViewModel>> Handle(AddDefinitionCommand command, CancellationToken ca)
{
var parentWord = await _unitOfWork.WordRepo.GetWordByIdAsync(command.WordId);
if(parentWord == null)
{
return Result<WordDefinitionViewModel>.Failure(new DomainError(ErrorCodes.NotFound, "Word", $"A word with id {command.WordId} doesn't exist."));
}
var parentCourse = await _unitOfWork.CourseRepo.GetByIdAsync(parentWord.CourseId);
var userId = _userContext.UserId;
if(!(parentCourse.UserId == userId))
{
return Result<WordDefinitionViewModel>.Failure(new DomainError(ErrorCodes.NotAuthorized, "Unauthorized  access to course.", "You can't modify this Word. You aren't the owner of the parent course."));
}
var result = parentWord.AddDefinition(command.Text, command.Type);
if(!result.IsSuccess)
{
return Result<WordDefinitionViewModel>.Failure(result.Error);
}
await _unitOfWork.SaveChangesAsync();
return Result<WordDefinitionViewModel>.Success(new WordDefinitionViewModel(result.Value.Id, result.Value.Text, result.Value.Type));
}
}