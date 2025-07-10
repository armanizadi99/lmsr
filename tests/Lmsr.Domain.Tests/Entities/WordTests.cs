using Lmsr.Domain.Entities;
using Xunit;
using FluentAssertions;
public class WordTests
{
private Course _course = new Course("course1", Guid.NewGuid().ToString());
private string _validTerm = "Hello";

[Fact]
public void Constructer_ValidTermAndCourse_SetsParametersToProperties()
{
// Act
Word word = new Word(_validTerm, _course);

// Assert
word.Should().NotBeNull();
word.Term.Should().Be(_validTerm);
word.Course.Should().NotBeNull().And.Be(_course);
}

[Fact]
public void Constructer_NullTerm_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new Word(null, _course);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("term");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void Constructer_EmptyOrWhiteSpaceTerm_ThrowsArgumentException(string term)
{
// Act
Action act = ()=> new Word(term, _course);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("term")
.WithMessage("term must not be white space or empty. (Parameter \'term\')");
}

[Fact]
public void Constructer_NullCourse_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new Word(_validTerm, null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("course");
}

[Fact]
public void SetTerm_ValidTerm_SetsTerm()
{
// Arrange
Word word = new Word(_validTerm, _course);
string newTerm = "new term";

// Act
word.SetTerm(newTerm);

// Assert
word.Term.Should().Be(newTerm);
}

[Fact]
public void SetTerm_NullTerm_ThrowsArgumentNullException()
{
// Arrange
Word word = new Word(_validTerm, _course);

// Act
Action act = ()=> word.SetTerm(null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("term");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void SetTerm_EmptyOrWhiteSpaceTerm_ThrowsArgumentException(string term)
{
// Arrange
Word word = new Word(_validTerm, _course);

// Act
Action act = ()=> word.SetTerm(term);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("term")
.WithMessage("term must not be white space or empty. (Parameter \'term\')");
}
}