using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Parser.GrammarReader.Models;

namespace Parser.GrammarReader;

public class TokenRuleParser
{
    public Dictionary<string, LiteralToken> Literals;
    public Dictionary<string, TokenRule> Rules;

    private Regex ContextSettingsRegex = new Regex("(\\w+)=(\\w+|(?<!\\\\)\".*?(?<!\\\\)\")");
    private Regex TokensInCase = new Regex("((?<!\\\\)\".*?(?<!\\\\)\"|\\w+)+");
    private Regex RuleBase = new Regex("(\\w+)\\s*:\\s*((.*?):)?\\s*(.*?)\\s*;");

    public TokenRuleParser()
    {
        Literals = new Dictionary<string, LiteralToken>();
        Rules = new Dictionary<string, TokenRule>();
    }

    public TokenRules ParseRules(string rules)
    {
        throw new NotImplementedException();
    }

    internal TokenRule ParseRule(string rule)
    {
        var match = RuleBase.Match(rule);
        if (!match.Success)
            throw new ArgumentException($"Incorrect rule definition: \"{rule}\"");
        var key = match.Groups[1].Value;
        
        var ctxString = match.Groups[3].Value;
        var ctxConfig = ParseContext(ctxString);

        var rest = match.Groups[4].Value
            .Split('|')
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => ParseCase(s.Trim()))
            .ToArray();

        return new TokenRule(key, ctxConfig, rest);
    }

    internal TokenCase ParseCase(string tokenCase)
    {
        var tokens = TokensInCase.Matches(tokenCase)
            .Select(m => m.Groups[1].Value)
            .Select(ToToken)
            .ToArray();

        return new TokenCase(tokens);
    }

    private IToken ToToken(string token)
    {
        if (token.StartsWith('"') && token.EndsWith('"'))
        {
            var tokenValue = RemoveSurroundingQuotes(token);
            if (Literals.TryGetValue(tokenValue, out var result))
                return result;
            var literal = new LiteralToken(tokenValue);
            Literals[tokenValue] = literal;
            return literal;
        }

        if (Rules.TryGetValue(token, out var rule))
            return rule;
        throw new ArgumentException($"Token \"{token}\" is not defined");
    }

    internal ContextConfig ParseContext(string context)
    {
        var dict = new Dictionary<string, string>();

        var matches = ContextSettingsRegex.Matches(context);
        foreach (Match match in matches)
        {
            dict[match.Groups[1].Value] = RemoveSurroundingQuotes(match.Groups[2].Value);
        }

        return ContextConfig.FromDict(dict);
    }

    private static string RemoveSurroundingQuotes(string value)
    {
        if (value.StartsWith('"') && value.EndsWith('"'))
            return value.Substring(1, value.Length - 2);

        return value;
    }

    private LiteralToken GetOrCreateLiteral(string literal)
    {
        if (Literals.TryGetValue(literal, out var literalToken))
            return literalToken;

        literalToken = new LiteralToken(literal);
        Literals[literal] = literalToken;
        return literalToken;
    }
}