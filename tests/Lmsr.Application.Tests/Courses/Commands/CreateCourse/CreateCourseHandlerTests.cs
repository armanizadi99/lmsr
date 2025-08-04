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
var mockUserIdAuthenticitySpec = new Mock<ICourseUserIdAuthenticitySpecification>();
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
Course capturedCourse = null;
mockUserIdAuthenticitySpec.Setup( m => m.IsUserIdAuthentic(It.IsAny<string>())).Returns(true);
mockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(It.IsAny<string>())).Returns(true);
mockCourseRepo.Setup(m => m.AddAsync(It.IsAny<Course>()))
.Callback<Course>(c => {c.Id=1; capturedCourse = c;})
.Returns(Task.CompletedTask);
mockUnitOfWork.Setup(m => m.CourseRepo).Returns(mockCourseRepo.Object);
var title = "valid title";
var userId = Guid.NewGuid().ToString();
var isPrivate = false;
CreateCourseHandler handler = new CreateCourseHandler(mockUnitOfWork.Object, mockTitleUniquenessSpec.Object, mockUserIdAuthenticitySpec.Object);
CreateCourseCommand command = new CreateCourseCommand(title, userId, isPrivate);

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
public async Task Handle_InauthenticUserId_ShouldThrowInvalidDomainOperationException()
{
// Arrange
var mockUserIdAuthenticitySpec = new Mock<ICourseUserIdAuthenticitySpecification>();
var mockTitleUniquenessSpec = new Mock<ICourseTitleUniquenessSpecification>();
var mockUnitOfWork = new Mock<IUnitOfWork>();
mockUserIdAuthenticitySpec.Setup(m => m.IsUserIdAuthentic("123")).Returns(false);
var handler = new CreateCourseHandler(mockUnitOfWork.Object, mockTitleUniquenessSpec.Object, mockUserIdAuthenticitySpec.Object);
var command = new CreateCourseCommand("course1", "123", false);

// Act
Func<Task> act = () => handler.Handle(command, CancellationToken.None);

// Assert
await act.Should().ThrowAsync<InvalidDomainOperationException>()
.WithMessage("Invalid UserId. ");
}

[Fact]
public async Task Handle_DuplicateTitle_ShouldReturnError()
{
// Arrange
var mockUserIdAuthenticitySpec = new Mock<ICourseUserIdAuthenticitySpecification>();
var mockTitleUniquenessSpec = new Mock<ICourseTitleUniquenessSpecification>();
var mockUnitOfWork = new Mock<IUnitOfWork>();
var duplicateTitle = "course1";
mockUserIdAuthenticitySpec.Setup(m => m.IsUserIdAuthentic(It.IsAny<string>())).Returns(true);
mockTitleUniquenessSpec.Setup(m => m.IsTitleUnique(duplicateTitle)).Returns(false);
var handler = new CreateCourseHandler(mockUnitOfWork.Object, mockTitleUniquenessSpec.Object, mockUserIdAuthenticitySpec.Object);
var command = new CreateCourseCommand(duplicateTitle, "123", false);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.DuplicateEntity, "Course", "A course with the same name exists."));
}
}