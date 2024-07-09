using System;
using System.Collections.Generic;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _position;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;
    }

    public AstNode Parse()
    {
        return ParseExpression();
    }

    private AstNode ParseExpression()
    {
        if (_position >= _tokens.Count)
            throw new Exception("Unexpected end of input");

        Token token = _tokens[_position];
        _position++;

        return token.Type switch
        {
            TokenType.Number => new NumberNode((int)token.Value),
            TokenType.Identifier => new IdentifierNode(token.Lexeme),
            TokenType.Boolean => new BooleanNode((bool)token.Value),
            TokenType.String => new StringNode(token.Lexeme),
            TokenType.Quote => new QuoteNode(ParseExpression()),
            TokenType.LeftParen => ParseList(),
            _ => throw new Exception($"Unexpected token: {token.Type}")
        };
    }

    private AstNode ParseList()
    {
        List<AstNode> elements = new();

        while (_position < _tokens.Count && _tokens[_position].Type != TokenType.RightParen)
        {
            elements.Add(ParseExpression());
        }

        if (_position >= _tokens.Count)
            throw new Exception("Unclosed parenthesis");

        _position++; // Skip the right parenthesis

        return new ListNode(elements);
    }
}
