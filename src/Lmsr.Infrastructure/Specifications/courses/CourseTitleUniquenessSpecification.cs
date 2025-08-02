using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Db;
namespace Lmsr.Infrastructure.Specifications;
public class CourseTitleUniquenessSpecification : ICourseTitleUniquenessSpecification
{
private AppDbContext _context;

public CourseTitleUniquenessSpecification (AppDbContext context)
{
_context = context;
}
public bool IsTitleUnique(string title)
{
return !_context.Courses.Any(c => c.Title == title);
}
}