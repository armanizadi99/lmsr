namespace Lmsr.Application.Interfaces;
public interface IUnitOfWork
{
ICourseRepository CourseRepo {get; }
IWordRepository WordRepo {get; }
void Commit();
}