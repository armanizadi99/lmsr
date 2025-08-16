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

public class AdWordToCourseHandlerTests
{
[Fact]
public async Task Handle_ValidCommand_ShouldAddWordToCourse()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockWordRepo = new Mock<IWordRepository>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockWordTermUniquenessSpec = new Mock<IWordTermUniquenessSpecification>();
var mockUserContext = new Mock<IUserContext>();
Word capturedWord = null;
var userId = Guid.NewGuid().ToString();
var course = new Course("course1", userId, false);
mockWordRepo.Setup(m => m.AddAsync(It.IsAny<Word>()))
.Callback<Word>(w => {
w.Id=1;
capturedWord = w;
}
)
.Returns(Task.CompletedTask);
mockCourseRepo.Setup(m => m.GetCourseByIdAsync(1))
.ReturnsAsync(course);
mockUnitOfWork.Setup(m => m.WordRepo)
.Returns(mockWordRepo.Object);
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
mockWordTermUniquenessSpec.Setup(m => m.IsWordTermUnique(It.IsAny<string>()))
.Returns(true);
mockUserContext.Setup(m => m.UserId).Returns(userId);
var handler = new AddWordToCourseHandler(mockUnitOfWork.Object, mockUserContext.Object, mockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand("word1", 1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

// Assert
mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
result.IsSuccess.Should().BeTrue();
result.Value.Id.Should().Be(1);
result.Value.Term.Should().Be("word1");
course.WordsReference.Should().Contain(1);
}

[Fact]
public async Task Handle_NoneExistingCourse_ShouldReturnNotFoundError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockWordTermUniquenessSpec = new Mock<IWordTermUniquenessSpecification>();
var mockUserContext = new Mock<IUserContext>();
var course = new Course("course1", "123", false);
mockCourseRepo.Setup(m => m.GetCourseByIdAsync(It.IsAny<int>()))
.ReturnsAsync((Course)null);
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
var handler = new AddWordToCourseHandler(mockUnitOfWork.Object, mockUserContext.Object, mockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand("word1", 1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.NotFound, "Course", "this course doesn't exist."));
}

[Fact]
public async Task Handle_CourseWithDifferentOwner_ShouldReturnNotAuthorizedError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockWordTermUniquenessSpec = new Mock<IWordTermUniquenessSpecification>();
var mockUserContext = new Mock<IUserContext>();
var userId = Guid.NewGuid().ToString();
var course = new Course("course1", userId, false);
mockCourseRepo.Setup(m => m.GetCourseByIdAsync(It.IsAny<int>()))
.ReturnsAsync(course);
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns("arman");
var handler = new AddWordToCourseHandler(mockUnitOfWork.Object, mockUserContext.Object, mockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand("word1", 1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.NotAuthorized, "UnauthorizedAccessToCourse", "You can't modify this course. You aren't the owner."));
}

[Fact]
public async Task Handle_DuplicateWord_ShouldReturnDuplicateEntityError()
{
// Arrange
var mockUnitOfWork = new Mock<IUnitOfWork>();
var mockCourseRepo = new Mock<ICourseRepository>();
var mockWordTermUniquenessSpec = new Mock<IWordTermUniquenessSpecification>();
var mockUserContext = new Mock<IUserContext>();
var userId = Guid.NewGuid().ToString();
var course = new Course("course1", userId, false);
mockCourseRepo.Setup(m => m.GetCourseByIdAsync(It.IsAny<int>()))
.ReturnsAsync(course);
mockUnitOfWork.Setup(m => m.CourseRepo)
.Returns(mockCourseRepo.Object);
mockUserContext.Setup(m => m.UserId).Returns(userId);
mockWordTermUniquenessSpec.Setup(m => m.IsWordTermUnique("word1")).Returns(false);
var handler = new AddWordToCourseHandler(mockUnitOfWork.Object, mockUserContext.Object, mockWordTermUniquenessSpec.Object);
var command = new AddWordToCourseCommand("word1", 1);

// Act
var result = await handler.Handle(command, CancellationToken.None);

/// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain(new DomainError(ErrorCodes.DuplicateEntity, "Word", "You can't add two words with the same term in a course."));
}
}