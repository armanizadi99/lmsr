namespace Lmsr.Application.Tests;
public class DeleteWordHandlerTests : WordHandlerTestsBase
{
[Fact]
public async Task Handle_ExistingWord_ShouldDeleteWord()
{
// Arrange
var wordId = 1;
var handler = new DeleteWordHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteWordCommand(wordId);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
}

[Fact]
public async Task Handle_NoneExistingWord_ShouldReturnDomainError()
{
// Arrange
var handler = new DeleteWordHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteWordCommand(2);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Word", "No such word exists in the database."));
}

[Fact]
public async Task Handle_UnauthorizedAccessToWord_ShouldReturnDomainError()
{
// Arrange
var differentUserId = Guid.NewGuid().ToString();
MockUserContext.Setup(m => m.UserId).Returns(differentUserId);
var handler = new DeleteWordHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new DeleteWordCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToWord", "You can't delete this word. You aren't the owner of the parent course."));
}
}