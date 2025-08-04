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
List<DomainError> errors= new List<DomainError>();
if(_definitions.Exists(d => string.Equals(d.Text, text, StringComparison.OrdinalIgnoreCase)))
errors.Add(new DomainError(ErrorCodes.DuplicateEntity, "Definition", "This definition already exists."));
if(errors.Any())
return Result<WordDefinition>.Failure(errors);
var definition = new WordDefinition(text, type, Id);
_definitions.Add(definition);
return Result<WordDefinition>.Success(definition);
}

public Result RemoveDefinition(int definitionId)
{
List<DomainError> errors = new List<DomainError>();
var definition = _definitions.Where(d => d.Id == definitionId)
.FirstOrDefault();
if(definition == null)
errors.Add(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
if(errors.Any())
return Result.Failure(errors);
_definitions.Remove(definition);
return Result.Success();
}

public Result ChangeDefinitionText(string text, int definitionId)
{
List<DomainError> errors = new List<DomainError>();
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
errors.Add(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
if(errors.Any())
return  Result.Failure(errors);
def._setText(text);
return Result.Success();
}

public Result ChangeDefinitionType(WordType type, int definitionId)
{
List<DomainError> errors = new List<DomainError>();
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
errors.Add(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
if(errors.Any())
return  Result.Failure(errors);
def._setType(type);
return Result.Success();
}
}