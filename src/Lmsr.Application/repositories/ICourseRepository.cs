using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface ICourseRepository : IBaseRepository<Course>
{
public Task<List<Course>> GetAllCourses(int Id);
public Task<Course> GetCourseById(int Id);
}