using Lmsr.Domain.Common;
using System.Collections.Generic;
namespace Lmsr.Domain.Entities;
public class Course : BaseEntity<int>
{
public string Title{ get; private set; }
public string UserId{ get; private set; }
public IReadOnlyCollection<Word> Words;

public Course(string title, string userId)
{
SetTitle(title);
UserId=userId;
}

public void SetTitle(string title)
{
if(title == null)
throw new ArgumentNullException(nameof(title));
if(string.IsNullOrWhiteSpace(title))
throw new ArgumentException("Title must not be white space, or empty", nameof(title));
Title=title;
}
}