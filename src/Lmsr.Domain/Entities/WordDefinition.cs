using Lmsr.Domain.Common;
using System.Collections.Generic;
namespace Lmsr.Domain.Entities;
public class WordDefinition : BaseEntity<int>
{
public string Text {get; private set; }
public WordType type {get; private set; }
public int WordId {get; private set; }
public Word Word {get; private set; }

public WordDefinition(string text, Word word)
{
SetText(text);
_linkToWord(word);
}

private void _linkToWord(Word word)
{
if(word == null)
throw new ArgumentNullException(nameof(word));
Word=word;
WordId=word.Id;
}

public void SetText(string text)
{
if(text == null)
throw new ArgumentNullException(nameof(text));
if(string.IsNullOrWhiteSpace(text))
throw new ArgumentException("Text must not be white space or empty.", nameof(text));
Text=text;
}
}