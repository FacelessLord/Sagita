namespace Parser.GrammarReader.Models;

public class TokenRules
{
    public Dictionary<string, TokenRule> Rules;
    public Dictionary<string, LiteralToken> Literals;

    public TokenRules(Dictionary<string, TokenRule> rules, Dictionary<string, LiteralToken> literals)
    {
        Rules = rules;
        Literals = literals;
    }
}

public interface IToken
{
    public string Name { get; }
}

public class TokenRule : IToken
{
    public string Name { get; }
    public ContextConfig Context;
    public TokenCase[] Cases;

    public TokenRule(string name, ContextConfig context, params TokenCase[] cases)
    {
        Name = name;
        Context = context;
        Cases = cases;
    }
}

public struct LiteralToken : IToken
{
    public string Name { get; }

    public LiteralToken(string literal)
    {
        Name = literal;
    }
}

public class TokenCase
{
    public IToken[] Tokens;

    public TokenCase(params IToken[] tokens)
    {
        Tokens = tokens;
    }
}