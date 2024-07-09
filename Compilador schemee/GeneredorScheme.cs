using System.Xml.Linq;

public class SchemeCodeGenerator : ICodeGenerator
{
    public string GenerateCode(AstNode node)
    {
        return node switch
        {
            NumberNode numberNode => numberNode.Value.ToString(),
            IdentifierNode identifierNode => identifierNode.Name,
            BooleanNode booleanNode => booleanNode.Value ? "#t" : "#f",
            StringNode stringNode => $"\"{stringNode.Value}\"",
            QuoteNode quoteNode => $"'{GenerateCode(quoteNode.QuotedNode)}",
            ListNode listNode => $"({string.Join(" ", listNode.Elements.ConvertAll(GenerateCode))})",
            DefineNode defineNode => $"(define {GenerateCode(defineNode.Variable)} {GenerateCode(defineNode.Value)})",
            IfNode ifNode => $"(if {GenerateCode(ifNode.Condition)} {GenerateCode(ifNode.ThenBranch)} {GenerateCode(ifNode.ElseBranch)})",
            LambdaNode lambdaNode => $"(lambda ({string.Join(" ", lambdaNode.Parameters.ConvertAll(GenerateCode))}) {GenerateCode(lambdaNode.Body)})",
            _ => throw new Exception($"Unknown AST node type: {node.GetType()}")
        };
    }
}
