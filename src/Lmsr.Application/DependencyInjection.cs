using Microsoft.Extensions.DependencyInjection;
using Lmsr.Application.Courses;
using Lmsr.Application.Words;
using Lmsr.Application.ViewModels;
using Lmsr.Application;
using FluentValidation;
namespace Lmsr.Application;

public static class DependencyInjection
{
public static IServiceCollection AddApplication(this IServiceCollection services)
{
services.AddScoped<IRequestHandler<CreateCourseCommand, Result<int>>, CreateCourseHandler>();
services.AddScoped<IRequestHandler<DeleteCourseCommand, Result>, DeleteCourseHandler>();
services.AddScoped<IRequestHandler<AddWordToCourseCommand, Result<Word>>, AddWordToCourseHandler>();
services.AddScoped<IRequestHandler<DeleteWordCommand, Result>, DeleteWordHandler>();
services.AddScoped<IRequestHandler<GetAllCoursesQuery, Result<List<CourseViewModel>>>, GetAllCoursesHandler>();
services.AddScoped<IRequestHandler<GetCourseWordByIdQuery, Result<WordViewModel>>, GetCourseWordByIdHandler>();
services.AddScoped<INotificationHandler<CourseDeletedEvent>, CourseDeletedEventHandler>();

services.AddValidatorsFromAssemblyContaining<CreateCourseHandler>();
return services;
}
}