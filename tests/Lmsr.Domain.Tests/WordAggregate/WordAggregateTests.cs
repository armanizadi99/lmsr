using Lmsr.Domain.Aggregates;
using Lmsr.Domain.Common;
using Xunit;
using FluentAssertions;
namespace Lmsr.Domain.Tests;

public class WordAggregateTests
{
private string _validTerm = "test";
private string _validText = "this means to test something.";
private WordType _validType = WordType.Verb;

[Fact]
public void Constructer_validData_ShouldSetProperties()
{
// Arrange
int courseId = 1;

// Act
var word = new Word(_validTerm, courseId);

// Assert
word.Term.Should().Be(_validTerm);
word.CourseId.Should().Be(courseId);
word.Definitions.Should().NotBeNull();
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void Constructer_NullOrEmptyTerm_ShouldThrowValidationException(string? term)
{
// Act
Action act = () => new Word(term, 1);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid term. Term Shouldn't be null, empty or whitespace.");
}

[Fact]
public void AddDefinition_ValidData_ShouldAddDefinition()
{
// Arrange
var word = new Word(_validTerm, 1);

// Act
var result = word.AddDefinition(_validText, _validType);

// Assert
result.IsSuccess.Should().BeTrue();
word.Definitions.Count.Should().Be(1);
word.Definitions.Should().Contain(result.Value);
result.Value.Text.Should().Be(_validText);
result.Value.Type.Should().Be(_validType);
result.Value.WordId.Should().Be(0);
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void AddDefinition_NullOrEmptyText_ShouldThrowValidationException(string? text)
{
// Arrange
var word = new Word(_validTerm, 1);

// Act
Action act = () => word.AddDefinition(text, WordType.Verb);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid text. Text shouldn't be null, empty or whitespace.");
}

[Fact]
public void AddDefinition_NoneExistingType_ShouldThrowValidationException()
{
// Arrange
var word = new Word(_validTerm, 1);
WordType type = (WordType)(-1);

// Act
Action act = () => word.AddDefinition(_validText, type);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid Type. Type must be one of the predefined types.");
}

[Fact]
public void AddDefinition_DuplicateDefinition_ShouldReturnError()
{
// Arrange
var word = new Word(_validTerm, 1);
var duplicateText = "this is a text.";
var firstResult = word.AddDefinition(duplicateText, WordType.Verb);

// Act
var secondResult = word.AddDefinition(duplicateText, WordType.Verb);

// Assert
firstResult.IsSuccess.Should().BeTrue();
secondResult.IsSuccess.Should().BeFalse();
secondResult.Errors.Should().Contain("This definition already exists.");
}

[Fact]
public void RemoveDefinition_ExistingDefinition_ShouldRemoveDefinition()
{
// Arrange
var word = new Word(_validTerm, 1);
var addResult = word.AddDefinition(_validText, WordType.Noun);

// Act
var removeResult = word.RemoveDefinition(addResult.Value.Id);

// Assert
addResult.IsSuccess.Should().BeTrue();
removeResult.IsSuccess.Should().BeTrue();
word.Definitions.Count.Should().Be(0);
}

[Fact]
public void RemoveDefinition_NoneExistingDefinition_ShouldReturnError()
{
// Arrange
var word = new Word(_validTerm, 1);

// Act
var result = word.RemoveDefinition(0);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain("Definition not found.");
}

[Fact]
public void ChangeDefinitionText_NewValidText_ShouldChangeDefinitionText()
{
// Arrange
var word = new Word(_validTerm, 1);
var addResult = word.AddDefinition(_validText, WordType.Noun);
string  newDefinitionText = "this is the new text43dfkljjtg";

// Act
var result = word.ChangeDefinitionText(newDefinitionText, addResult.Value.Id);

// Assert
addResult.Value.Text.Should().Be(newDefinitionText);
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public void ChangeDefinitionText_NullOrEmptyText_ShouldThrowValidationException(string? text)
{
// Arrange
var word = new Word(_validTerm, 1);
var addResult = word.AddDefinition(_validText, WordType.Noun);

// Act
Action act = () => word.ChangeDefinitionText(text, addResult.Value.Id);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid text. Text shouldn't be null, empty or white space.");
}

[Fact]
public void ChangeDefinitionText_NoneExistingDefinition_ShouldReturnError()
{
// Arrange
var word = new Word(_validTerm, 1);

// Assert
var result = word.ChangeDefinitionText("Text won't change because no such definition exists.", 0);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain("Definition not found.");
}

[Fact]
public void ChangeDefinitionType_ValidType_ShouldChangeDefinitionType()
{
// Arrange
var word = new Word(_validTerm, 1);
var def = word.AddDefinition(_validText, WordType.Verb).Value;
WordType newType = WordType.Pronoun;

// Act
var result = word.ChangeDefinitionType(newType, def.Id);

// Assert
result.IsSuccess.Should().BeTrue();
def.Type.Should().Be(newType);
}

[Fact]
public void ChangeDefinitionType_InvalidType_ShouldThrowValidationException()
{
// Arrange
var word = new Word(_validTerm, 1);
var def = word.AddDefinition(_validText, WordType.Verb).Value;
WordType invalidType = (WordType)(-1);

// Act
Action act = () => word.ChangeDefinitionType(invalidType, def.Id);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid Type. Type must be one of the predefined types.");
}

[Fact]
public void ChangeDefinitionType_NoneExistingDefinition_ShouldReturnError()
{
// Arrange
var word = new Word(_validTerm, 1);

// Act
var result = word.ChangeDefinitionType(WordType.Pronoun, 0);

// Assert
result.IsSuccess.Should().BeFalse();
result.Errors.Should().Contain("Definition not found.");
}

[Fact]
public void SetTerm_newValidTerm_ShouldChangeTerm()
{
// Arrange
var word = new Word(_validTerm, 1);
var newValidTerm = "this is the new term";

// Act
var result = word.SetTerm(newValidTerm);

// Act
result.IsSuccess.Should().BeTrue();
word.Term.Should().Be(newValidTerm);
}

[Theory]
[InlineData("")]
[InlineData("    ")]
[InlineData(null)]
public void SetTerm_NullOrEmptyTerm_ShouldThrowValidationException(string? term)
{
// Arrange
var word = new Word(_validTerm, 1);

// Act
Action act = () => word.SetTerm(term);

// Assert
act.Should().Throw<DomainValidationException>()
.WithMessage("Invalid term. Term Shouldn't be null, empty or whitespace.");
}
}