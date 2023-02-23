using FluentAssertions;
using Parser.GrammarReader;
using Parser.GrammarReader.Models;

namespace ParserTests.GrammarParser;

public class TokenParserRulesParsingTests
{
    private TokenRuleParser parser;

    [SetUp]
    public void Setup()
    {
        parser = new TokenRuleParser();
    }

    [Test]
    public void ParsesSimpleRules()
    {
        const string ruleString = "NUMBER: \"1\"; DVA: NUMBER NUMBER";

        var rules = parser.ParseRules(ruleString);

        rules.Rules.Should().ContainKey("NUMBER");
        rules.Rules["NUMBER"].Cases.Should().HaveCount(1);
        rules.Rules["NUMBER"].Cases[0].Tokens.Should().HaveCount(1);
        rules.Rules["NUMBER"].Cases[0].Tokens[0].Should().BeOfType<LiteralToken>();
        rules.Rules["NUMBER"].Cases[0].Tokens[0].As<LiteralToken>().Name.Should().Be("1");

        rules.Rules.Should().ContainKey("DVA");
        rules.Rules["DVA"].Cases.Should().HaveCount(1);
        rules.Rules["DVA"].Cases[0].Tokens.Should().HaveCount(2);
        rules.Rules["DVA"].Cases[0].Tokens[0].Should().BeOfType<TokenRule>();
        rules.Rules["DVA"].Cases[0].Tokens[0].As<TokenRule>().Name.Should().Be("NUMBER");
        rules.Rules["DVA"].Cases[0].Tokens[1].Should().BeOfType<TokenRule>();
        rules.Rules["DVA"].Cases[0].Tokens[1].As<TokenRule>().Name.Should().Be("NUMBER");
    }

    [Test]
    public void ThrowsOnRenamingRule()
    {
        const string ruleString = "NUMBER: \"1\"; DVA: NUMBER";

        var rulesAction = () => parser.ParseRules(ruleString);

        rulesAction.Should().Throw<ArgumentException>()
            .Where(e =>
                e.Message.StartsWith("Token DVA renames token NUMBER which is permitted"));
    }
}