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