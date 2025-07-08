using Lmsr.Domain.Entities;
namespace Lmsr.Application.Courses;
public record RemoveDefinitionFromWordCommand(int WordDefinitionId, string UserId) : IRequest;