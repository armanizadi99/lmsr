using Microsoft.Extensions.DependencyInjection;
using Lmsr.Application.Courses;
using Lmsr.Application.ViewModels;
namespace Lmsr.Application;

public static class DependencyInjection
{
public static IServiceCollection AddApplication(this IServiceCollection services)
{
services.AddScoped<IRequestHandler<CreateCourseCommand, Result<int>>, CreateCourseHandler>();
services.AddScoped<IRequestHandler<GetAllCoursesQuery, Result<List<CourseViewModel>>>, GetAllCoursesHandler>();
return services;
}
}