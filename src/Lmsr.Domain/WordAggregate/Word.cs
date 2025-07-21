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
if(term == null || string.IsNullOrWhiteSpace(term))
throw new ValidationException("Invalid term. term Shouldn't be null, empty or whitespace.");
Term = term;
return Result.Success();
}

public Result<WordDefinition> AddDefinition(string text, WordType type)
{
if(string.IsNullOrWhiteSpace(text))
throw new ValidationException("Invalid text. Text shouldn't be null, empty or whitespace.");
List<string> errors= new List<string>();
if(_definitions.Exists(d => string.Equals(d.Text, text, StringComparison.OrdinalIgnoreCase)))
errors.Add("This definition already exists.");
if(errors.Any())
return Result<WordDefinition>.Failure(errors);
var definition = new WordDefinition(text, type, Id);
_definitions.Add(definition);
return Result<WordDefinition>.Success(definition);
}

public Result RemoveDefinition(int definitionId)
{
List<string> errors = new List<string>();
var definition = _definitions.Where(d => d.Id == definitionId)
.FirstOrDefault();
if(definition == null)
errors.Add("Definition not found.");
if(errors.Any())
return Result.Failure(errors);
_definitions.Remove(definition);
return Result.Success();
}

public Result ChangeDefinitionText(string text, int definitionId)
{
List<string> errors = new List<string>();
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
errors.Add("Definition not found.");
if(errors.Any())
return  Result.Failure(errors);
def._setText(text);
return Result.Success();
}

public Result ChangeDefinitionType(WordType type, int definitionId)
{
List<string> errors = new List<string>();
var def = _definitions.Where(d => d.Id == definitionId).FirstOrDefault();
if(def == null)
errors.Add("Definition not found.");
if(errors.Any())
return  Result.Failure(errors);
def._setType(type);
return Result.Success();
}
}