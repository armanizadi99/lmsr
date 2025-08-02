using Lmsr.Domain.Aggregates.Specifications;
using Lmsr.Infrastructure.Db;
namespace Lmsr.Infrastructure.Specifications;
public class CourseUserIdAuthenticitySpecification : ICourseUserIdAuthenticitySpecification
{
private AppDbContext _context;

public CourseUserIdAuthenticitySpecification(AppDbContext context)
{
_context = context;
}
public bool IsUserIdAuthentic(string userId)
{
return !string.IsNullOrEmpty(userId); // we don't have an authentication system for now, so we asume all user  ids  that aren't empty are authentic.
}
}