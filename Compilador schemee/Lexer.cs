public enum TokenType
{
    Number,
    Identifier,
    LeftParen,   // (
    RightParen,  // )
    Quote,       // '
    Dot,         // .
    Boolean,
    String,
    Keyword,
    EndOfInput
}

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public object Value { get; }

    public Token(TokenType type, string lexeme, object value = null)
    {
        Type = type;
        Lexeme = lexeme;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type}: {Lexeme}";
    }
}

public class Lexer
{
    private readonly string _input;
    private int _position;

    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "define", TokenType.Keyword },
        { "lambda", TokenType.Keyword },
        { "if", TokenType.Keyword },
        { "else", TokenType.Keyword },
        { "cond", TokenType.Keyword },
        { "let", TokenType.Keyword },
        { "begin", TokenType.Keyword },
        { "quote", TokenType.Keyword },
        { "set!", TokenType.Keyword },
        { "and", TokenType.Keyword },
        { "or", TokenType.Keyword },
        { "not", TokenType.Keyword },
        { "#t", TokenType.Boolean },
        { "#f", TokenType.Boolean }
    };

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    public Token GetNextToken()
    {
        SkipWhitespace();

        if (_position >= _input.Length)
            return new Token(TokenType.EndOfInput, "");

        char current = _input[_position];

        if (char.IsDigit(current))
            return ReadNumber();

        if (char.IsLetter(current))
            return ReadIdentifierOrKeyword();

        if (current == '(')
            return ReadSingleCharToken(TokenType.LeftParen);

        if (current == ')')
            return ReadSingleCharToken(TokenType.RightParen);

        if (current == '\'')
            return ReadSingleCharToken(TokenType.Quote);

        if (current == '.')
            return ReadSingleCharToken(TokenType.Dot);

        if (current == '"')
            return ReadString();

        // Añadir más reglas para otros tipos de tokens según sea necesario

        throw new Exception($"Unexpected character: {current}");
    }

    private Token ReadNumber()
    {
        int start = _position;
        while (_position < _input.Length && char.IsDigit(_input[_position]))
            _position++;

        string lexeme = _input[start.._position];
        return new Token(TokenType.Number, lexeme, int.Parse(lexeme));
    }

    private Token ReadIdentifierOrKeyword()
    {
        int start = _position;
        while (_position < _input.Length && char.IsLetterOrDigit(_input[_position]))
            _position++;

        string lexeme = _input[start.._position];
        if (Keywords.ContainsKey(lexeme))
        {
            return lexeme switch
            {
                "#t" => new Token(TokenType.Boolean, lexeme, true),
                "#f" => new Token(TokenType.Boolean, lexeme, false),
                _ => new Token(Keywords[lexeme], lexeme)
            };
        }

        return new Token(TokenType.Identifier, lexeme);
    }

    private Token ReadString()
    {
        _position++; // Skip the opening quote
        int start = _position;

        while (_position < _input.Length && _input[_position] != '"')
            _position++;

        if (_position >= _input.Length)
            throw new Exception("Unterminated string literal");

        string lexeme = _input[start.._position];
        _position++; // Skip the closing quote

        return new Token(TokenType.String, lexeme);
    }

    private Token ReadSingleCharToken(TokenType type)
    {
        char current = _input[_position];
        _position++;
        return new Token(type, current.ToString());
    }

    private void SkipWhitespace()
    {
        while (!(_position >= _input.Length || !char.IsWhiteSpace(_input[_position])))
            _position++;
    }
}
