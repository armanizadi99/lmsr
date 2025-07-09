using Lmsr.Domain.Common;
using System.Collections.Generic;
namespace Lmsr.Domain.Entities;
public class Course : BaseEntity<int>
{
public string Title{ get; private set; }
public string UserId{ get; private set; }
public IReadOnlyCollection<Word> Words = new List<Word>();

public Course(string title, string userId)
{
SetTitle(title);
_setUserId(userId);
}

public void SetTitle(string title)
{
if(title == null)
throw new ArgumentNullException(nameof(title));
if(string.IsNullOrWhiteSpace(title))
throw new ArgumentException("Title must not be white space or empty.", nameof(title));
Title=title;
}

private void _setUserId(string userId)
{
if(userId == null)
throw new ArgumentNullException(nameof(userId));
if(string.IsNullOrWhiteSpace(userId))
throw new ArgumentException("userId must not be white space or empty.", nameof(userId));
UserId=userId;
}
}