namespace Lmsr.Application.Tests;

public class DeleteCourseTests : CourseHandlerTestsBase
{
[Fact]
public async Task handle_ShouldDeleteCourse()
{
// Arrange
var handler = new DeleteCourseHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
MockCourseRepo.Verify(m => m.Delete(CapturedCourse), Times.Once);
MockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
}

[Fact]
public async Task Handle_NoUserId_ShouldReturnNotAuthorizedError()
{
// Arrange
var userId = "";
MockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new DeleteCourseHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "Authorization", "You aren't authorized."));
}

[Fact]
public async Task Handle_UnauthorizedUserId_ShouldReturnNotAuthorizedError()
{
// Arrange
var differentUserId = Guid.NewGuid().ToString();
MockUserContext.Setup(m => m.UserId).Returns(differentUserId);
var handler = new DeleteCourseHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "Course", "You can't delete this resource. You aren't the owner."));
}

[Fact]
public async Task Handle_NoneExistingCourse_ShouldReturnNotFoundError()
{
// Arrange
var handler = new DeleteCourseHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteCourseCommand(3);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Course", $"A course with id {command.CourseId} hasn't been found."));
}
}