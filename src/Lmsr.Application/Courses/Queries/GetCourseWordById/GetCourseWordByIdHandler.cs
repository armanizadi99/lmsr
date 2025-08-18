using Lmsr.Application.ViewModels;
using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Courses;
public class GetCourseWordByIdHandler : IRequestHandler<GetCourseWordByIdQuery, Result<WordViewModel>>
{
private IWordRepository _repository;

public GetCourseWordByIdHandler(IWordRepository repository)
{
_repository = repository;
}
public async Task<Result<WordViewModel>> Handle(GetCourseWordByIdQuery query, CancellationToken ca)
{
var word = await _repository.GetWordByIdAsync(query.WordId);
if(word == null)
return Result<WordViewModel>.Failure(new DomainError(ErrorCodes.NotFound, "Word", $"No word with id {query.WordId} exists."));
var definitions = new List<WordDefinitionViewModel>();
foreach(var definition in word.Definitions)
{
definitions.Add(new WordDefinitionViewModel(definition.Id, definition.Text, definition.Type));
}
var response = new WordViewModel(word.Id, word.Term, definitions, word.CourseId);
return Result<WordViewModel>.Success(response);
}
}