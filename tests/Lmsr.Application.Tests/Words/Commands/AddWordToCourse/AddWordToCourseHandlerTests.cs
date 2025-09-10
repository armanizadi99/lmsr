namespace Lmsr.Application.Tests;

public class AdWordToCourseHandlerTests : WordHandlerTestsBase
{
[Fact]
public async Task Handle_ValidCommand_ShouldAddWordToCourse()
{
// Arrange
var handler = new AddWordToCourseHandler(MockUnitOfWork.Object, MockUserContext.Object, MockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand(UniqueWord, CourseId);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Id.Should().Be(1);
result.Value.Term.Should().Be(UniqueWord);
DefaultCourse.WordsReference.Should().Contain(1);
MockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
}

[Fact]
public async Task Handle_NoneExistingCourse_ShouldReturnNotFoundError()
{
// Arrange
var handler = new AddWordToCourseHandler(MockUnitOfWork.Object, MockUserContext.Object, MockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand(UniqueWord, NoneExistingCourseId);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Course", "this course doesn't exist."));
}

[Fact]
public async Task Handle_CourseWithDifferentOwner_ShouldReturnNotAuthorizedError()
{
// Arrange
var differentUserId = Guid.NewGuid().ToString();
MockUserContext.Setup(m => m.UserId).Returns(differentUserId);
var handler = new AddWordToCourseHandler(MockUnitOfWork.Object, MockUserContext.Object, MockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand(UniqueWord, CourseId);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToCourse", "You can't modify this course. You aren't the owner."));
}

[Fact]
public async Task Handle_DuplicateWord_ShouldReturnDuplicateEntityError()
{
// Arrange
var handler = new AddWordToCourseHandler(MockUnitOfWork.Object, MockUserContext.Object, MockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand(DuplicateWord, CourseId);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.DuplicateEntity, "Word", "You can't add two words with the same term in a course."));
}
}