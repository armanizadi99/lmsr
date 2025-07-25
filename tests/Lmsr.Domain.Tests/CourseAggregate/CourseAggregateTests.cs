using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Common;
using Xunit;
using FluentAssertions;
namespace Lmsr.Domain.Tests;
public class CourseAggregateTests
{
private string _validTitle ="course1";
private string _validUserId = Guid.NewGuid().ToString();
[Fact]
public void Constructer_validData_ShouldCreateValidCourse()
{
// Arrange
bool isPrivate = false;

// Act
var course = new Course(_validTitle, _validUserId, isPrivate);

// Assert
course.Title.Should().Be(_validTitle);
course.UserId.Should().Be(_validUserId);
course.IsPrivate.Should().Be(isPrivate);
course.WordsReference.Should().NotBeNull();
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void Constructer_NullOrEmptyTitle_ShouldThrowValidationException(string? title)
{
// Arrange

// Act
Action act = () => new Course(title, _validUserId, false);

// Assert
act.Should()
.Throw<DomainValidationException>()
.WithMessage("Invalid title. Title Shouldn't be null, empty or whitespace.");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void Constructer_NullOrEmptyUserId_ShouldThrowValidationException(string? userId)
{
// Act
Action act = () => new Course(_validTitle, userId, false);

// Assert
act.Should()
.Throw<DomainValidationException>()
.WithMessage("Invalid userId. UserId Shouldn't be null, empty or whitespace.");
}

[Fact]
public void AddWordReference_DistinctReference_ShouldAddANewWordReference()
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);
int wordReference = 1;

// Act
var result = course.AddWordReference(wordReference);

// Assert
result.IsSuccess.Should().BeTrue();
course.WordsReference.Count.Should().Be(1);
course.WordsReference.Should().Contain(wordReference);
}

[Fact]
public void AddWordReference_DuplicateWordReference_ShouldThrowValidationException()
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);
var firstResult = course.AddWordReference(1);
int duplicateWordReference = 1;

// Act
Action act = () => course.AddWordReference(duplicateWordReference);

// Assert
course.WordsReference.Count.Should().Be(1);
firstResult.IsSuccess.Should().BeTrue();
act.Should().Throw<DomainValidationException>()
.WithMessage("This reference already exists.");
}

[Fact]
public void RemoveWordReference_ExistingReference_ShouldRemoveReference()
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);
int wordReference = 1;
var addResult = course.AddWordReference(wordReference);

// Act
var removeResult = course.RemoveWordReference(wordReference);

// Assert
addResult.IsSuccess.Should().BeTrue();
removeResult.IsSuccess.Should().BeTrue();
course.WordsReference.Should().NotContain(wordReference);
}

[Fact]
public void RemoveWordReference_NoneExistingReference_ShouldThrowInvalidDomainOperationException()
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);
int wordReference = 1;

// Act\
Action act = () => course.RemoveWordReference(wordReference);

// Assert
act.Should().Throw<InvalidDomainOperationException>()
.WithMessage("This reference doesn't exist.");
}

[Fact]
public void ChangeTitle_NewValidTitle_ShouldChangeTitle()
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);
string newValidTitle = "new title";

// Act
course.ChangeTitle(newValidTitle);

// Assert
course.Title.Should().Be(newValidTitle);
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void ChangeTitle_NullOrEmptyTitle_shouldThrowValidationException(string? title)
{
// Arrange
var course = new Course(_validTitle, _validUserId, false);

// Act
Action act = () => course.ChangeTitle(title);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid title. Title Shouldn't be null, empty or whitespace.");
}
}