namespace Lmsr.Domain.Aggregates.Specifications;
public interface IDefinitionTextUniquenessSpecification
{
bool IsDefinitionTextUnique(string text);
}