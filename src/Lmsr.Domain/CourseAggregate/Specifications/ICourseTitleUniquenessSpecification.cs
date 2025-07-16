namespace Lmsr.Domain.Aggregates.Specifications;
public interface ICourseTitleUniquenessSpecification
{
bool IsTitleUnique(string title);
}