namespace Lmsr.Domain.Aggregates.Specifications;
public interface ICourseUserIdAuthenticitySpecification
{
bool IsUserIdAuthentic(string userId);
}