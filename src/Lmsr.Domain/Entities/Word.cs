using Lmsr.Domain.Common;
using System.Collections.Generic;
namespace Lmsr.Domain.Entities;
public class Word : BaseEntity<int>
{
public string Term {get; private set; }
public IReadOnlyCollection<WordDefinition> WordDefinitions = new List<WordDefinition>();
public int CourseId {get; private set; }
public Course Course {get; private set; }

public Word(string term, Course course)
{
SetTerm(term);
_linkToCourse(course);
}
private void _linkToCourse(Course course)
{
if(course == null)
throw new ArgumentNullException(nameof(course));
Course=course;
CourseId=course.Id;
}

public void SetTerm(string term)
{
if(term == null)
throw new ArgumentNullException(nameof(term));
if(string.IsNullOrWhiteSpace(term))
throw new ArgumentException("Term must not be white space or empty.", nameof(term));
Term=term;
}
}