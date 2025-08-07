using FluentValidation;
namespace Lmsr.Application.Courses;
public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
public DeleteCourseCommandValidator()
{
}
}