using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Db;
namespace Lmsr.Infrastructure.Specifications;

public class WordTermUniquenessSpecification : IWordTermUniquenessSpecification
{
private readonly AppDbContext _context;

public WordTermUniquenessSpecification(AppDbContext context)
{
_context = context;
}
public bool IsWordTermUnique(string term, int courseId)
{
return !_context.Words.Any(w => w.Term == term && w.CourseId == courseId);
}
}