using Xunit;
using FluentAssertions;
using Lmsr.Domain.Entities;
public class CourseTests
{
private string _validTitle = "course1";
private string _validUserId = Guid.NewGuid().ToString();

[Fact]
public void Constructer_ValidTitle_SetsAttributes()
{
// Act
var course = new Course(_validTitle, _validUserId);

// Assert
course.Should().NotBeNull();
course.Title.Should().Be(_validTitle);
course.UserId.Should().Be(_validUserId);
}

[Fact]
public void Constructer_NullTitle_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new Course(null, _validUserId);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("title");
}

[Theory]
[InlineData("")]
[InlineData("    ")]
public void Constructer_EmptyAndWhiteSpaceTitle_ThrowsArgumentException(string title)
{
// Act
Action act = ()=> new Course(title, _validUserId);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("title")
.WithMessage("Title must not be white space or empty. (Parameter \'title\')");
}

[Fact]
public void Constructer_NullUserId_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new Course(_validTitle, null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("userId");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void Constructer_EmptyOrWhiteSpaceUserId_ThrowsArgumentException( string userId)
{
// Act
Action act = ()=> new Course(_validTitle, userId);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("userId")
.WithMessage("UserId must not be white space or empty. (Parameter \'userId\')");
}

[Fact]
public void SetName_ValidTitle_SetsTitle()
{
// Arrange
var course = new Course(_validTitle, _validUserId);
var newTitle = "New Title";

// Act
course.SetTitle(newTitle);

// Assert
course.Title.Should().Be(newTitle);
}

[Fact]
public void SetTitle_NullTitle_ThrowsArgumentNullException()
{
// Arrange
var course = new Course(_validTitle, _validUserId);

// Act
Action act = ()=> course.SetTitle(null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("title");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void SetTitle_EmptyOrWhiteSpaceTitle_ThrowsArgumentException(string invalidTitle)
{
// Arrange
var course = new Course(_validTitle, _validUserId);

// Act
Action act = ()=> course.SetTitle(invalidTitle);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("title")
.WithMessage("Title must not be white space or empty. (Parameter \'title\')");
}
}