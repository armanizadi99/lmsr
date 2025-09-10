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
public class GetAllCoursesHandlerTests : CourseHandlerTestsBase
{
[Fact]
public async Task Handle_ShouldReturnAllCourses()
{
// Arrange
var handler = new GetAllCoursesHandler(MockCourseRepo.Object);
var query = new GetAllCoursesQuery();

// Act
var result = await handler.Handle(query, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Count.Should().Be(3);
}
}