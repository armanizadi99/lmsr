using Lmsr.Domain.Common;
using System.Collections.Generic;
namespace Lmsr.Domain.Aggregates;
public class Course : BaseEntity<int>
{
private List<int> _wordsReference = new List<int>();
public string Title { get; private set; }
public string UserId { get; private set; }
public bool IsPrivate { get; private set; }
public IReadOnlyCollection<int> WordsReference;

private Course(){ }

public Course(string title, string userId, bool isPrivate)
{
if(string.IsNullOrWhiteSpace(title))
throw new DomainValidationException("Invalid title. Title Shouldn't be null, empty or whitespace.");
if(string.IsNullOrWhiteSpace(userId))
throw new DomainValidationException("Invalid userId. UserId Shouldn't be null, empty or whitespace.");
if(isPrivate == null)
throw new DomainValidationException("IsPrivate shouldn't be null.");
Title = title;
UserId = userId;
IsPrivate = isPrivate;
WordsReference = _wordsReference.AsReadOnly();
}

public Result AddWordReference(int referenceId)
{
if(referenceId == null)
<<<<<<< Updated upstream
throw new DomainValidationException("Reference id must not be null.");
=======
throw new ValidationException("Reference id must not be null.");
List<string> errors = new List<string>();

// this should throw exception as well, since we really don't expect duplicate references, this will have to be handled by application layer.
>>>>>>> Stashed changes
if(_wordsReference.Contains(referenceId))
throw new DomainValidationException("This reference already exists.");
List<string> errors = new List<string>();
if(errors.Any())
return Result.Failure(errors);
_wordsReference.Add(referenceId);
return Result.Success();
}

public Result RemoveWordReference(int referenceId)
{
if(referenceId == null)
throw new DomainValidationException("Reference id must not be null.");
if(!_wordsReference.Contains(referenceId))
throw new DomainValidationException("This reference doesn't exist.");
List<string> errors = new List<string>();
if(errors.Any())
return Result.Failure(errors);
_wordsReference.Remove(referenceId);
return Result.Success();
}

public void MakeCoursePrivate()
{
IsPrivate = true;
}

public void MakeCoursePublic()
{
IsPrivate = false;
}

public void ChangeTitle(string title)
{
if(title == null || string.IsNullOrWhiteSpace(title))
throw new DomainValidationException("Invalid title. Title Shouldn't be null, empty or whitespace.");
Title = title;
}
}