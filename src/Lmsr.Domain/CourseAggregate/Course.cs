using Lmsr.Domain.Common;
namespace Lmsr.Domain.Aggregates;
public class Course : IBaseEntity<int>
{
private List<int> _wordsReference;
public string Title { get; private set; }
public string UserId { get; private set; }
public bool IsPrivate { get; private set; }
public IReadOnlyCollection<int> WordsReference = _wordsReference.AsReadOnly();

private Course(){ }

public Course(string title, string userId, bool isPrivate)
{
if(title == null || string.IsNullOrWhiteSpace(title))
throw new ValidationException("Invalid title. Title Shouldn't be null, empty or whitespace.");
if(userId == null || string.IsNullOrWhiteSpace(userId))
throw new ValidationException("Invalid userId. UserId Shouldn't be null, empty or whitespace.");
if(isPrivate == null)
throw new ValidationException("IsPrivate shouldn't be null.");
Title = title;
UserId = userId;
IsPrivate = isPrivate;
}

public void AddWordReference(int referenceId)
{
if(referenceId == null)
throw new ValidationException("Reference id must not be null.");
if(_wordsReference.Contains(referenceId))
throw new DomainRouleViolationException("This reference already exists.");
_wordsReference.Add(referenceId);
}

public void RemoveWordReference(int referenceId)
{
if(referenceId == null)
throw new ValidationException("Reference id must not be null.");
if(!_wordsReference.Contains(referenceId))
throw new InvalidDomainOperationException("This reference doesn't exist.");
_wordsReference.Remove(referenceId);
}

public void MakeCoursePrivate()
{
IsPrivate true;
}

public void MakeCoursePublic()
{
IsPrivate = false;
}

public void ChangeTitle(string title)
{
if(title == null || string.IsNullOrWhiteSpace(title))
throw new ValidationException("Invalid title. Title Shouldn't be null, empty or whitespace.");
Title = title;
}
}