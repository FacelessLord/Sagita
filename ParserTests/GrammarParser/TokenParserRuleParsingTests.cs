using FluentAssertions;
using Parser.GrammarReader;
using Parser.GrammarReader.Models;

namespace ParserTests.GrammarParser;

public class TokenParserRuleParsingTests
{
    private TokenRuleParser parser;

    // \"1\" | \"2\" | \"3\" | \"4\" | \"5\" | \"6\" | \"7\" | \"8\" | \"9\"
    
    [SetUp]
    public void Setup()
    {
        parser = new TokenRuleParser();
    }

    [Test]
    public void ParsesSimpleRule()
    {
        const string ruleString = "NUMBER: \"1\";";

        var rule = parser.ParseRule(ruleString);

        rule.Name.Should().Be("NUMBER");
        
        rule.Context.IncludeSpaces.Should().BeFalse();
        rule.Context.UseRegex.Should().BeFalse();
        rule.Context.EscapeCharacter.Should().Be("\\");
        
        rule.Cases.Should().HaveCount(1);
        
        rule.Cases[0].Tokens.Should().HaveCount(1);
        rule.Cases[0].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[0].Name.Should().Be("1");
    }
    
    [Test]
    public void ParsesRuleWithContext()
    {
        const string ruleString = "NUMBER: spaces=true : \"1\";";

        var rule = parser.ParseRule(ruleString);

        rule.Name.Should().Be("NUMBER");
        
        rule.Context.IncludeSpaces.Should().BeTrue();
        rule.Context.UseRegex.Should().BeFalse();
        rule.Context.EscapeCharacter.Should().Be("\\");
        
        rule.Cases.Should().HaveCount(1);
        
        rule.Cases[0].Tokens.Should().HaveCount(1);
        rule.Cases[0].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[0].Name.Should().Be("1");
    }

    [Test]
    public void ParsesCasedRule()
    {
        const string ruleString = "NUMBER: \"1\" | \"2\" | \"3\";";

        var rule = parser.ParseRule(ruleString);

        rule.Name.Should().Be("NUMBER");
        
        rule.Context.IncludeSpaces.Should().BeFalse();
        rule.Context.UseRegex.Should().BeFalse();
        rule.Context.EscapeCharacter.Should().Be("\\");
        
        rule.Cases.Should().HaveCount(3);
        
        rule.Cases[0].Tokens.Should().HaveCount(1);
        rule.Cases[0].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[0].Name.Should().Be("1");
        
        rule.Cases[1].Tokens.Should().HaveCount(1);
        rule.Cases[1].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[1].Tokens[0].Name.Should().Be("2");
        
        rule.Cases[2].Tokens.Should().HaveCount(1);
        rule.Cases[2].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[2].Tokens[0].Name.Should().Be("3");
    }

    [Test]
    public void ParsesRuleWithSeveralTokens()
    {
        const string ruleString = "NUMBER: \"1\" \"2\" \"3\";"; // 123

        var rule = parser.ParseRule(ruleString);

        rule.Name.Should().Be("NUMBER");
        
        rule.Context.IncludeSpaces.Should().BeFalse();
        rule.Context.UseRegex.Should().BeFalse();
        rule.Context.EscapeCharacter.Should().Be("\\");
        
        rule.Cases.Should().HaveCount(1);
        
        rule.Cases[0].Tokens.Should().HaveCount(3);
        rule.Cases[0].Tokens[0].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[0].Name.Should().Be("1");
        rule.Cases[0].Tokens[1].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[1].Name.Should().Be("2");
        rule.Cases[0].Tokens[2].Should().BeOfType<LiteralToken>();
        rule.Cases[0].Tokens[2].Name.Should().Be("3");
    }
}