
/*
    xml -> element
    element -> ‘<’ NAME attributes? ‘>’ ( element | CHAR_DATA | CDATA | COMMENT)* ‘</’
    NAME ‘>’
    attribute -> NAME ‘=’ STRING
    CHAR_DATA -> /[^\r\n<]+/
    CDATA -> ‘<![CDATA [‘ ( /[^\]/ | ‘]’ / ]^\]]/ )* ‘]]>’
    COMMENT -> ‘<!--" /[^-]+/ ‘-->’
    NAME -> /[a-zA-Z_:][a-zA-Z0-9._:-]*/
//  STRING -> /"([^"]*)"|'([^']*)'/



public class Grammar {
    public Dictionary<string, List<List<object>>> grammar = new Dictionary<string, List<List<object>>>() 
    {
        { "xml", new List<List<object>>() { new List<object>() { "element" } } },
        { "element", new List<List<object>>()
            {
                new List<object>() { "<", "NAME", "attributes?", ">", "content", "</", "NAME", ">" }
            }
        },
        { "content", new List<List<object>>()
            {
                new List<object>() { "element" },
                new List<object>() { "CHAR_DATA" },
                new List<object>() { "CDATA" },
                new List<object>() { "COMMENT" }
            }
        },
        { "attributes?", new List<List<object>>()
            {
                new List<object>(),
                new List<object>() { "attribute", "attributes?" }
            }
        },
        { "attribute", new List<List<object>>()
            {
                new List<object>() { "NAME", "=", "STRING" }
            }
        },
        { "CHAR_DATA", new List<List<object>>()
            {
                new List<object>() { "STRING" },
                new List<object>() { "NAME" }
            }
        },
        { "CDATA", new List<List<object>>()
            {
                new List<object>() { "<![CDATA[", "STRING", "]]>" }
            }
        },
        { "COMMENT", new List<List<object>>()
            {
                new List<object>() { "<!--", "STRING", "-->" }
            }
        },
        { "NAME", new List<List<object>>()
            {
                new List<object>() { @"/[a-zA-Z_:][a-zA-Z0-9._:-]*/" }
            }
        },
        { "STRING", new List<List<object>>()
            {
                new List<object>() { @"/""([^""]*)""|'([^']*)'/" }
            }
        }
    };
}


public enum TokenType
{
    OPEN_TAG,
    CLOSE_TAG,
    EQUALS,
    QUOTE,
    SLASH,
    CDATA,
    COMMENT,
    NAME,
    CHAR_DATA,
    END,
    ERROR
}

public class Token
{
    public TokenType TokenType { get; }
    public string StringValue { get; }

    public Token(TokenType tokenType)
    {
        TokenType = tokenType;
        StringValue = string.Empty;
    }

    public Token(TokenType tokenType, string stringValue)
    {
        TokenType = tokenType;
        StringValue = stringValue;
    }


    public override string ToString()
    {
        if (StringValue != null)
        {
            return $"{TokenType}('{StringValue}')";
        }
        else
        {
            return TokenType.ToString();
        }
    }
}
