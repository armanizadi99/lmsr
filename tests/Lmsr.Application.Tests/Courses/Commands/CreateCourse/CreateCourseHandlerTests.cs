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

public class CreateCourseHandlerTests : CourseHandlerTestsBase
{
[Fact]
public async Task Handle_ValidCommand_ShouldMakeACourse()
{
// Arrange
var isPrivate = false;
var handler = BuildHandler();
var command = new CreateCourseCommand(UniqueTitle, isPrivate);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Should().Be(1);
CapturedCourse.Title.Should().Be(UniqueTitle);
CapturedCourse.UserId.Should().Be(DefaultUserId);
CapturedCourse.IsPrivate.Should().Be(isPrivate);
MockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
}

[Fact]
public async Task Handle_DuplicateTitle_ShouldReturnError()
{
// Arrange
var handler = BuildHandler();
var command = new CreateCourseCommand(DuplicateTitle, false);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.DuplicateEntity, "Course", "A course with the same name exists."));
}
}