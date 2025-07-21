using Lmsr.Domain.Common;
namespace Lmsr.Domain.Aggregates;
public class WordDefinition : BaseEntity<int>
{
public string Text { get; private set; }
public WordType Type { get; private set; }
public int WordId {get; private set; }

private WordDefinition() {}

public WordDefinition(string text, WordType type, int wordId)
{
_setText(text);
_setType(type);
WordId = wordId;
}
internal Result _setText(string text)
{
if(string.IsNullOrWhiteSpace(text))
throw new ValidationException("Invalid text. Text shouldn't be null, empty or white space.");
Text = text;
return Result.Success();
}
internal void _setType(WordType type)
{
if(!Enum.IsDefined(typeof(WordDefinition), type))
throw new ValidationException("Invalid Type. Type must be one of the predefined types.");
Type = type;
}
}