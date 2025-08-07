using Xunit;
using FluentAssertions;
using Moq;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Common;
using Lmsr.Application.Courses;
using Lmsr.Application.Repositories;
using Lmsr.Application.Interfaces;
namespace Lmsr.Application.Tests;

public class DeleteCourseTests
{
[Fact]
public async Task handle_ShouldDeleteCourse()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockUserContext = new Mock<IUserContext>();
var userId = Guid.NewGuid().ToString();
Course capturedCourse = null;
mockCourseRepo.Setup(m => m.Delete(It.IsAny<Course>()))
.Callback<Course>(c => capturedCourse = c);
mockCourseRepo.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
.ReturnsAsync(new Course("course1", userId, false));
mockUnitOfWork.Setup(m => m.CourseRepo).Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new DeleteCourseHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
mockCourseRepo.Verify(m => m.Delete(capturedCourse), Times.Once);
mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
}

[Fact]
public async Task Handle_NoUserId_ShouldReturnNotAuthorizedError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockUserContext = new Mock<IUserContext>();
var userId = "";
mockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new DeleteCourseHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.NotAuthorized, "Authorization", "You aren't authorized."));
}

[Fact]
public async Task Handle_UnauthorizedUserId_ShouldReturnNotAuthorizedError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockUserContext = new Mock<IUserContext>();
var userId = Guid.NewGuid().ToString();
var differentUserId = Guid.NewGuid().ToString();
mockCourseRepo.Setup(m => m.GetByIdAsync(1))
.ReturnsAsync(new Course("course1", userId, false));
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns(differentUserId);
var handler = new DeleteCourseHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.NotAuthorized, "Course", "You can't delete this resource. You aren't the owner."));
}

[Fact]
public async Task Handle_NoneExistingCourse_ShouldReturnNotFoundError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockUserContext = new Mock<IUserContext>();
var userId = Guid.NewGuid().ToString();

mockCourseRepo.Setup(m => m.GetByIdAsync(1))
.ReturnsAsync((Course)null);
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new DeleteCourseHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteCourseCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.NotFound, "Course", $"A course with id {command.CourseId} hasn't been found."));
}
}