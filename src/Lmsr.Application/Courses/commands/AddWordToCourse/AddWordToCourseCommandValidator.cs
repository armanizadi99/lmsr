using FluentValidation;
namespace Lmsr.Application.Courses;
public class AddWordToCourseCommandValidator : AbstractValidator<AddWordToCourseCommand>
{
public AddWordToCourseCommandValidator()
{
RuleFor(x => x.Term)
.NotEmpty()
.WithMessage("Word term must not be empty or white space");
RuleFor(x => x.Term)
.NotNull()
.WithMessage("Word term must not be null");
}
}