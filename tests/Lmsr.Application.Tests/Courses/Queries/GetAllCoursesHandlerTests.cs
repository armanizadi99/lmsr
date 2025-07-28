using Xunit;
using FluentAssertions;
using FluentAssertions.Collections;
using Moq;
using Lmsr.Application.Courses;
using Lmsr.Application.Repositories;
using Lmsr.Application.ViewModels;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Common;
namespace Lmsr.Application.Tests;
public class GetAllCoursesHandlerTests
{
[Fact]
public async Task Handle_ShouldReturnAllCourses()
{
// Arrange
var mockCourseRepository = new Mock<ICourseRepository>();
var courses = new List<Course>();
var course1 = new Course("english basic words", "arman", false);
course1.Id = 1;
var course2 = new Course("english intermediate words", "arman", false);
course2.Id = 2;
var course3 = new Course("english curses", "baduser", true);
course3.Id = 3;
courses.AddRange(course1, course2, course3);
mockCourseRepository.Setup(m => m.GetAllCoursesAsync()).ReturnsAsync(courses);
var handler = new GetAllCoursesHandler(mockCourseRepository.Object);
var query = new GetAllCoursesQuery();

// Act
var result = await handler.Handle(query, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Count.Should().Be(3);
(result.Value[0].Id, result.Value[0].Title, result.Value[0].UserId).Should().Be((course1.Id, course1.Title, course1.UserId));
(result.Value[1].Id, result.Value[1].Title, result.Value[1].UserId).Should().Be((course2.Id, course2.Title, course2.UserId));
(result.Value[2].Id, result.Value[2].Title, result.Value[2].UserId).Should().Be((course3.Id, course3.Title, course3.UserId));
}
}