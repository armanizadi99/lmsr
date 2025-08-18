using Lmsr.Application.ViewModels;
namespace Lmsr.Application.Courses;
public record GetCourseWordByIdQuery(int WordId) : IRequest<Result<WordViewModel>>;