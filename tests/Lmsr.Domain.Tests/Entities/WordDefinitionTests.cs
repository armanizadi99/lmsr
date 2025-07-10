using Lmsr.Domain.Entities;
using Xunit;
using FluentAssertions;

public class WordDefinitionTests
{
private readonly Word _word = new Word("word1", new Course("course1", Guid.NewGuid().ToString()));
private readonly string _validText = "This is the first word.";
private readonly WordType _validType = (WordType)Enum.Parse(typeof(WordType), "Noun");

[Fact]
public void Constructr_ValidTextAndTypeAndWord_SetsProperties()
{
// Act
WordDefinition def = new WordDefinition(_validText, _validType, _word);

// Assert
def.Should().NotBeNull();
def.Text.Should().Be(_validText);
def.Type.Should().Be(_validType);
def.Word.Should().Be(_word);
}

[Fact]
public void Constructer_NullText_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new WordDefinition(null, _validType, _word);

// Assert 
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("text");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void Constructer_EmptyOrWhiteSpaceText_ThrowsArgumentException(string text)
{
// Act
Action act = ()=> new WordDefinition(text, _validType, _word);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("text")
.WithMessage("text must not be white space or empty. (Parameter \'text\')");
}

[Fact]
public void Constructer_InvalidType_ArgumentException()
{
// Arrange
WordType invalidType = (WordType)(-1);

// Act
Action act = ()=> new WordDefinition(_validText, invalidType, _word);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("type")
.WithMessage("Given type is undefined. (Parameter \'type\')");
}

[Fact]
public void Constructer_NullWord_ThrowsArgumentNullException()
{
// Act
Action act = ()=> new WordDefinition(_validText, _validType, null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("word");
}

[Fact]
public void SetText_ValidText_SetsText()
{
// Arrange
WordDefinition def = new WordDefinition(_validText, _validType, _word);
string newText = "this is the new text set for word 1.";

// Act
def.SetText(newText);

// Assert
def.Text.Should().Be(newText);
}

[Fact]
public void SetText_NullText_ThrowsArgumentNullException()
{
// Arrange
WordDefinition def = new WordDefinition(_validText, _validType, _word);

// Act
Action act = ()=> def.SetText(null);

// Assert
act.Should()
.Throw<ArgumentNullException>()
.WithParameterName("text");
}

[Theory]
[InlineData("")]
[InlineData("   ")]
public void SetText_EmptyOrWhiteSpaceText_ThrowsArgumentException(string text)
{
// Arrange
WordDefinition def = new WordDefinition(_validText, _validType, _word);

// Act
Action act = ()=> def.SetText(text);

// Assert
act.Should()
.Throw<ArgumentException>()
.WithParameterName("text")
.WithMessage("text must not be white space or empty. (Parameter \'text\')");
}
}