using FluentValidation;
namespace Lmsr.Application.Courses;
public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
public CreateCourseCommandValidator()
{
RuleFor(x => x.Title)
.NotEmpty()
.WithMessage("Course title must not be empty or white space");
}
}