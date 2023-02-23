using FluentAssertions;
using Parser.GrammarReader;

namespace ParserTests.GrammarParser;

public class TokenParserContextParsingTests
{
    private TokenRuleParser parser;
    
    [SetUp]
    public void Setup()
    {
        parser = new TokenRuleParser();
    }

    [Test]
    public void ParsesCorrectContext()
    {
        const string contextString = "spaces=true regex=false";
        
        var config = parser.ParseContext(contextString);

        config.IncludeSpaces.Should().Be(true);
        config.UseRegex.Should().Be(false);
    }
    
    [Test]
    public void ParsesEmptyContext()
    {
        const string contextString = "";
        
        var config = parser.ParseContext(contextString);

        config.IncludeSpaces.Should().Be(false);
        config.UseRegex.Should().Be(false);
    }
    
    [Test]
    public void ParsesWithUnexpectedKeys()
    {
        const string contextString = "cool=true";
        
        var config = parser.ParseContext(contextString);

        config.IncludeSpaces.Should().Be(false);
        config.UseRegex.Should().Be(false);
    }
    
    [Test]
    public void AllowsStringValuesToBePassed()
    {
        const string contextString = "escape=\"/\"";
        
        var config = parser.ParseContext(contextString);

        config.IncludeSpaces.Should().Be(false);
        config.UseRegex.Should().Be(false);
        config.EscapeCharacter.Should().Be("/");
    }
}