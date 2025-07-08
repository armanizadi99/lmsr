using Lmsr.Domain.Entities;
using System.Collections.Generic;
namespace Lmsr.Application.Courses;
public record GetCoursesQuery : IRequest<List<Course>>;
