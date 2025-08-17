namespace Lmsr.Domain.Aggregates.Specifications;
public interface IWordTermUniquenessSpecification
{
bool IsWordTermUnique(string text, int courseId);
}