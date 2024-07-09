class Program
{
    static void Main()
    {
        string input = "(define x 42) (define y \"hello\") #t #f";
        Lexer lexer = new Lexer(input);

        List<Token> tokens = new();
        Token token;
        do
        {
            token = lexer.GetNextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EndOfInput);

        Parser parser = new Parser(tokens);
        AstNode ast = parser.Parse();

        SchemeCodeGenerator generator = new SchemeCodeGenerator();
        string code = generator.GenerateCode(ast);

        Console.WriteLine(code);
    }
}
