using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface ICourseRepository : IBaseRepository<Course>
{
public Task<List<Course>> GetAllCoursesAsync();
public Task<Course> GetCourseByIdAsync(int Id);
}