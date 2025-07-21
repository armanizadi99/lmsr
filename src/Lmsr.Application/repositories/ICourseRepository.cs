using Lmsr.Domain.Aggregates;
namespace Lmsr.Application.Repositories;
public interface ICourseRepository : IBaseRepository<Course>
{
public Task<Course> GetCourseWithWordsById(int Id);
}