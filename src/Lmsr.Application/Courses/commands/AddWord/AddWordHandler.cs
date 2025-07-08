using Lmsr.Domain.Entities;
using Lmsr.Application.Repositories;
namespace Lmsr.Application.Courses;
public class AddWordHandler : IRequestHandler<AddWordCommand, int>
{
private IWordRepository _wRepository;
private ICourseRepository _cRepository;
private ILogger _logger;
public AddWordHandler(IWordRepository wRepository, ICourseRepository cRepository, ILogger logger)
{
_wRepository=wRepository;
_cRepository=cRepository;
_logger=logger;
}
public async Task<int> Handle(AddWordCommand command, CancellationToken cancellationToken)
{
var course = await _cRepository.GetById(command.CourseId);
if(course == null)
throw new InvalidOperationException("Invalid CourseId");
if(course.UserId != command.UserId)
throw new InvalidOperationException("User not authorized.");
var word = new Word(command.Term, course);
await _wRepository.Add(word);
await _wRepository.SaveChanges();
_logger.LogInformation("added word {@Word} to course {@Course}.", word, course);
return word.Id;
}
}