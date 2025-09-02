using FluentValidation;
namespace Lmsr.Application.Words;
public class DeleteWordCommandValidator : AbstractValidator<DeleteWordCommand>
{
public DeleteWordCommandValidator()
{
}
}