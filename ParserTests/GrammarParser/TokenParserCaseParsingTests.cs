using FluentAssertions;
using Parser.GrammarReader;
using Parser.GrammarReader.Models;

namespace ParserTests.GrammarParser;

public class TokenParserCaseParsingTests
{
    private TokenRuleParser parser;
    
    [SetUp]
    public void Setup()
    {
        parser = new TokenRuleParser();
    }
    
    [Test]
    public void ParsesSingleLiteral()
    {
        const string caseString = "\"_\""; // _
        
        var tokenCase = parser.ParseCase(caseString);

        tokenCase.Tokens.Should().HaveCount(1);
        tokenCase.Tokens[0].Should().BeOfType<LiteralToken>();
        tokenCase.Tokens[0].As<LiteralToken>().Name.Should().Be("_");
    }
    
    [Test]
    public void ParsesLiteralList()
    {
        const string caseString = "\"_\" \"2\" \"_\""; // _2_
        
        var tokenCase = parser.ParseCase(caseString);

        tokenCase.Tokens.Should().HaveCount(3);
        tokenCase.Tokens[0].Should().BeOfType<LiteralToken>();
        tokenCase.Tokens[0].As<LiteralToken>().Name.Should().Be("_");
        tokenCase.Tokens[1].Should().BeOfType<LiteralToken>();
        tokenCase.Tokens[1].As<LiteralToken>().Name.Should().Be("2");
        tokenCase.Tokens[2].Should().BeOfType<LiteralToken>();
        tokenCase.Tokens[2].As<LiteralToken>().Name.Should().Be("_");
    }
    
    [Test]
    public void ThrowsOnUnknownToken()
    {
        const string caseString = "\"_\" NUMBER \"_\""; // _NUMBER_
        var caseParser = () => parser.ParseCase(caseString);

        caseParser.Should().Throw<ArgumentException>()
            .Where(e => e.Message.StartsWith("Token \"NUMBER\" is not defined"));
    }

}