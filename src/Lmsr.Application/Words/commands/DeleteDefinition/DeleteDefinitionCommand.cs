namespace Lmsr.Application.Words;
public class DeleteDefinitionCommand(int wordId, int definitionId) : IRequest<Result>
{
public int WordId{ get; private set; } = wordId;
public int DefinitionId { get; private set; } = definitionId;
}