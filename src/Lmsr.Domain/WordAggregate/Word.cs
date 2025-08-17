using Lmsr.Domain.Common;
using System.Collections.Generic;

namespace Lmsr.Domain.Aggregates;
public class Word : BaseEntity<int>
{
private readonly List<WordDefinition> _definitions = new List<WordDefinition>();
public string Term;
public int CourseId {get; private set; }
public IReadOnlyCollection<WordDefinition> Definitions;

private Word() { }

public Word(string term, int courseId)
{
SetTerm(term);
CourseId = courseId;
Definitions = _definitions.AsReadOnly();
}

public Result SetTerm(string term)
{
if(string.IsNullOrWhiteSpace(term))
throw new DomainValidationException("Invalid term. Term Shouldn't be null, empty or whitespace.");
Term = term;
return Result.Success();
}

public Result<WordDefinition> AddDefinition(string text, WordType type)
{
if(string.IsNullOrWhiteSpace(text))
throw new DomainValidationException("Invalid text. Text shouldn't be null, empty or whitespace.");
if(_definitions.Exists(d => string.Equals(d.Text, text, StringComparison.OrdinalIgnoreCase)))
return Result<WordDefinition>.Failure(new DomainError(ErrorCodes.DuplicateEntity, "Definition", "This definition already exists."));
var definition = new WordDefinition(text, type, Id);
_definitions.Add(definition);
return Result<WordDefinition>.Success(definition);
}

public Result RemoveDefinition(int definitionId)
{
var definition = _definitions.Where(d => d.Id == definitionId)
.FirstOrDefault();
if(definition == null)
return Result.Failure(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
_definitions.Remove(definition);
return Result.Success();
}

public Result ChangeDefinitionText(string text, int definitionId)
{
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
return  Result.Failure(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
def._setText(text);
return Result.Success();
}

public Result ChangeDefinitionType(WordType type, int definitionId)
{
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
return  Result.Failure(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
def._setType(type);
return Result.Success();
}
}