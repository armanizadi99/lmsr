namespace Lmsr.Application.Words;
public class  DeleteWordCommand(int wordId) : IRequest<Result>
{
public int WordId{get; private set; } = wordId;
}