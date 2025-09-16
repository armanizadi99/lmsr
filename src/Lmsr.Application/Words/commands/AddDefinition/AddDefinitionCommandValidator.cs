using FluentValidation;
namespace Lmsr.Application.Words;
public class AddDefinitionCommandValidator : AbstractValidator<AddDefinitionCommand>
{
public AddDefinitionCommandValidator()
{
RuleFor(x => x.Text)
.NotEmpty()
.WithMessage("Definition text must not be empty or white space");
}
}