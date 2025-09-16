namespace Lmsr.Application.Words;
public class  AddDefinitionCommand(int wordId, string text, WordType type) : IRequest<Result<WordDefinitionViewModel>> 
{
public int WordId { get; private set; } = wordId;
public string Text { get; private set ;} = text;
public WordType Type { get; private set; } = type;
}