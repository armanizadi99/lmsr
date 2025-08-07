using Lmsr.Application.Courses;
using Lmsr.Application.Repositories;
using Lmsr.Application.Interfaces;
using Lmsr.Domain.Common;
using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Aggregates.Specifications;
using Xunit;
using FluentAssertions;
using Moq;
namespace Lmsr.Application.Tests;

public class CreateCourseHandlerTests
{
[Fact]
public async Task Handle_ValidCommand_ShouldMakeACourse()
{
// Arrange
var mockTitleUniquenessSpec = new Mock<ICourseTitleUniquenessSpecification>();
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockUserContext = new Mock<IUserContext>();
var mockCourseRepo = new Mock<ICourseRepository>();
var userId = Guid.NewGuid().ToString();
Course capturedCourse = null;
mockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(It.IsAny<string>())).Returns(true);
mockCourseRepo.Setup(m => m.AddAsync(It.IsAny<Course>()))
.Callback<Course>(c => {c.Id=1; capturedCourse = c;})
.Returns(Task.CompletedTask);
mockUnitOfWork.Setup(m => m.CourseRepo).Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns(userId);
var title = "valid title";
var isPrivate = false;
CreateCourseHandler handler = new CreateCourseHandler(mockUnitOfWork.Object, mockTitleUniquenessSpec.Object, mockUserContext.Object);
CreateCourseCommand command = new CreateCourseCommand(title, isPrivate);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Should().Be(1);
capturedCourse.Title.Should().Be(title);
capturedCourse.UserId.Should().Be(userId);
capturedCourse.IsPrivate.Should().Be(isPrivate);
mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
}

[Fact]
public async Task Handle_DuplicateTitle_ShouldReturnError()
{
// Arrange
var mockTitleUniquenessSpec = new Mock<ICourseTitleUniquenessSpecification>();
var mockUserContext = new Mock<IUserContext>();
var mockUnitOfWork = new Mock<IUnitOfWork>();
var userId = Guid.NewGuid().ToString();
var duplicateTitle = "course1";
mockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(duplicateTitle)).Returns(false);
mockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new CreateCourseHandler(mockUnitOfWork.Object, mockTitleUniquenessSpec.Object, mockUserContext.Object);
var command = new CreateCourseCommand(duplicateTitle, false);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.DuplicateEntity, "Course", "A course with the same name exists."));
}
}