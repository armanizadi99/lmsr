using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record GetWordDefinitionsQuery(int WordId) : IRequest<List<WordDefinition>>;