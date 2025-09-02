namespace Lmsr.Application.Tests;
public class DeleteWordHandlerTests
{
public (Mock<IUnitOfWork>, Mock<IUserContext>, Mock<ICourseRepository>, Mock<IWordRepository>) MockInterfaces(string userId, Course course, Word word)
{
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
mockCourseRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(course);
mockUnitOfWork.Setup(m => m.CourseRepo).Returns(mockCourseRepo.Object);
var mockWordRepo = new Mock<IWordRepository>();
mockWordRepo.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(word);
mockUnitOfWork.Setup(m => m.WordRepo).Returns(mockWordRepo.Object);
var mockUserContext = new Mock<IUserContext>();
mockUserContext.Setup(m => m.UserId).Returns(userId);
return (mockUnitOfWork, mockUserContext, mockCourseRepo, mockWordRepo);
}
[Fact]
public async Task Handle_ExistingWord_ShouldDeleteWord()
{
// Arrange
var userId = "arman";
var (mockUnitOfWork, mockUserContext, mockCourseRepo, mockWordRepo) = MockInterfaces(userId, new Course("course1", userId, false), new Word("word1", 1));
var wordId = 1;
var handler = new DeleteWordHandler(mockUnitOfWork.Object, mockUserContext.Object);
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
string userId = "";
var (mockUnitOfWork, mockUserContext, _, __) = MockInterfaces(userId, null, null);
var handler = new DeleteWordHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteWordCommand(1);

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
var userId = "someone else";
var (mockUnitOfWork, mockUserContext, _, __) = MockInterfaces(userId, new Course("course1", "arman", false), new Word("word1", 1));
var handler = new DeleteWordHandler(mockUnitOfWork.Object, mockUserContext.Object);
var command = new DeleteWordCommand(1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be(new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToWord", "You can't delete this word. You aren't the owner of the parent course."));
}
}