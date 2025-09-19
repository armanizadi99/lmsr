namespace Lmsr.Application.Tests;
public class DeleteDefinitionHandlerTests : WordHandlerTestsBase
{
private readonly DeleteDefinitionHandler _handler;
private readonly DeleteDefinitionCommand _defaultCommand;

public DeleteDefinitionHandlerTests()
{
_handler = new DeleteDefinitionHandler(MockUnitOfWork.Object, MockUserContext.Object);
_defaultCommand = new DeleteDefinitionCommand(1, 0);
}
[Fact]
public async Task Handle_DeleteExistingDefinition_ShouldSucceed()
{
// Arrange
var word = new Word(UniqueWord, 1);
word.Id=1;
word.AddDefinition(DefinitionText, DefaultType);
MockWordRepo.Setup(m => m.GetWordByIdAsync(1))
.ReturnsAsync(word);

// Act
var result = await _handler.Handle(_defaultCommand, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeTrue();
word.Definitions.Count.Should().Be(0);
}
[Fact]
public async Task Handle_NoneExistingParentWord_ShouldReturnWordNotFoundError()
{
// Arrange
var newCommand = new DeleteDefinitionCommand(0, 0);

// Act
var result = await _handler.Handle(newCommand, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Word", "Such a word doesn't exist."));
}
[Fact]
public async Task Handle_UnauthorizedUserCourseAccess_ShouldReturnNotAuthorizedError()
{
// Arrange
var newUserId = Guid.NewGuid().ToString();
MockUserContext.Setup(m => m.UserId)
.Returns(newUserId);

// Act
var result = await _handler.Handle(_defaultCommand, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "Unauthorized  access to course.", "You can't modify this Word. You aren't the owner of the parent course."));
}
[Fact]
public async Task Handle_NoneExistingDefinition_ShouldReturnDefinitionNotFoundError()
{
// Arrange
// Act
var result = await _handler.Handle(_defaultCommand, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotFound, "Definition", "Definition not found."));
}
}