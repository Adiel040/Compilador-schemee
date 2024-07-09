public abstract class AstNode { }

public class NumberNode : AstNode
{
    public int Value { get; }

    public NumberNode(int value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
}

public class IdentifierNode : AstNode
{
    public string Name { get; }

    public IdentifierNode(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}

public class BooleanNode : AstNode
{
    public bool Value { get; }

    public BooleanNode(bool value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
}

public class StringNode : AstNode
{
    public string Value { get; }

    public StringNode(string value)
    {
        Value = value;
    }

    public override string ToString() => $"\"{Value}\"";
}

public class QuoteNode : AstNode
{
    public AstNode QuotedNode { get; }

    public QuoteNode(AstNode quotedNode)
    {
        QuotedNode = quotedNode;
    }

    public override string ToString() => $"'{QuotedNode}";
}

public class ListNode : AstNode
{
    public List<AstNode> Elements { get; }

    public ListNode(List<AstNode> elements)
    {
        Elements = elements;
    }

    public override string ToString() => $"({string.Join(" ", Elements)})";
}
