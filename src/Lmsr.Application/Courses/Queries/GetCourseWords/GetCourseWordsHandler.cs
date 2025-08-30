namespace Lmsr.Application.Courses;
public class GetCourseWordsHandler : IRequestHandler<GetCourseWordsQuery, Result<List<WordViewModel>>>
{
private readonly IUnitOfWork _unitOfWork;

public GetCourseWordsHandler(IUnitOfWork unitOfWork)
{
_unitOfWork = unitOfWork;
}
public async Task<Result<List<WordViewModel>>> Handle(GetCourseWordsQuery query, CancellationToken ca)
{
var words = await _unitOfWork.WordRepo.GetAllWordsForCourseAsync(query.CourseId);
var wordsToReturn = new List<WordViewModel>();
foreach (var word in words) {
var definitions = new List<WordDefinitionViewModel>();
foreach (var definition in word.Definitions) {
definitions.Add(new WordDefinitionViewModel(definition.Id, definition.Text, definition.Type));
}
wordsToReturn.Add(new WordViewModel(word.Id, word.Term, definitions, word.CourseId));
}
return Result<List<WordViewModel>>.Success(wordsToReturn);
}
}