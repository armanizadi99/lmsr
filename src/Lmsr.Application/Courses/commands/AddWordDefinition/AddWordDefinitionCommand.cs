using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record AddWordDefinitionCommand(string Text, WordType WordType, int WordId, string UserId) : IRequest<int>;