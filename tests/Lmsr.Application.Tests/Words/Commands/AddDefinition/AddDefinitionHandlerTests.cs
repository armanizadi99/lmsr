namespace Lmsr.Application.Tests;
public class AddDefinitionHandlerTests : WordHandlerTestsBase
{
[Fact]
public async Task Handle_AddDefinition_ShouldAddDefinition()
{
// Arrange
var handler = new AddDefinitionHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new AddDefinitionCommand(1, DefinitionText, DefaultType);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
result.Value.Should().Be(new WordDefinitionViewModel(0, DefinitionText, DefaultType));
}
[Fact]
public async Task Handle_DefinitionWithDuplicateText_ShouldReturnDuplicateEntityError()
{
// Arrange
var handler = new AddDefinitionHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new AddDefinitionCommand(1, DefinitionText, DefaultType);
await handler.Handle(command, CancellationToken.None);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.DuplicateEntity, "Definition", "This definition already exists."));
}
[Fact]
public async Task Handle_UnauthorizedAccessToCourse_ShouldReturnUnauthorizedAccessError()
{
// Arrange
var newUserId = Guid.NewGuid().ToString();
MockUserContext.Setup(m => m.UserId)
.Returns(newUserId);
var handler = new AddDefinitionHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new AddDefinitionCommand(1, DefinitionText, DefaultType);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "Unauthorized  access to course.", "You can't modify this Word. You aren't the owner of the parent course."));
}
[Fact]
public async Task Handle_AddDefinitionToANoneExistingWord_ShouldReturnNotFoundError()
{
// Arrange
var handler = new AddDefinitionHandler(MockUnitOfWork.Object, MockUserContext.Object);
var command = new AddDefinitionCommand(NoneExistingWordId, DefinitionText, DefaultType);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Word", $"A word with id {command.WordId} doesn't exist."));
}
}