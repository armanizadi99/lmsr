namespace Lmsr.Application.Courses;

public class AddWordToCourseHandler : IRequestHandler<AddWordToCourseCommand, Result<Word>>
{
private IUnitOfWork _unitOfWork;
private IUserContext _userContext;
private IWordTermUniquenessSpecification _wordTermUniquenessSpec;
public AddWordToCourseHandler(IUnitOfWork unitOfWork, IUserContext userContext, IWordTermUniquenessSpecification wordTermUniquenessSpec)
{
_unitOfWork = unitOfWork;
_userContext = userContext;
_wordTermUniquenessSpec = wordTermUniquenessSpec;
}
public async Task<Result<Word>> Handle(AddWordToCourseCommand command, CancellationToken ct)
{
var course = await _unitOfWork.CourseRepo.GetCourseByIdAsync(command.CourseId);
if(course == null)
{
return Result<Word>.Failure(new List<DomainError>() {new DomainError(ErrorCodes.NotFound, "Course", "this course doesn't exist.")});
}
if(_userContext.UserId != course.UserId)
{
return Result<Word>.Failure(new List<DomainError>() {new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToCourse", "You can't modify this course. You aren't the owner.")});
}
if(!_wordTermUniquenessSpec.IsWordTermUnique(command.Term))
{
return Result<Word>.Failure(new List<DomainError>() {new DomainError(ErrorCodes.DuplicateEntity, "Word", "You can't add two words with the same term in a course.")});
}
Word wordToAdd = new Word(command.Term, command.CourseId);
await _unitOfWork.WordRepo.AddAsync(wordToAdd);
course.AddWordReference(wordToAdd.Id);
await _unitOfWork.SaveChangesAsync();
return Result<Word>.Success(wordToAdd);
}
}