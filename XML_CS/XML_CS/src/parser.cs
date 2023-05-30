
/*
Algorithm of a parser:
1. Initialize an empty root node for syntax tree
2. Initialize an empty stack for the element nodes
3. Request the token from a scanner
4. Perform actions, based on the token type, simultaneously checking the syntax of obtained
token using recursive descent approach with an algorithm given below.
    - OPEN_TAG: Create a new element node with the tag name and add it as a child node to the current top element node. Push the new element node onto the stack
    - CLOSE_TAG: Pop the top element from the stack
    - CHAR_DATA: Add it as a child node to the current top element node
    - NAME: Set the the name of the current top element node to the name token value
    - QUOTE: Read all tokens until the next QUOTE token as the attribute value. Add the attribute to the current top element node with the previous token as the attribute name
    - EQUALS: Read the next token as the attribute value and it to the current top element node as an attribute with the previous token as the attribute name
    - SLASH: Check the next token. If it is CLOSE_TAG with the same tag name as the current top element node, pop the top element form the stack
    - COMMENT or CDATA: Ignore it
5. Return the root node of the tree

Recursive descent parsing algorithm:
1. Start with the starting symbol of the grammar
2. If token does not match with the starting symbol, raise an error
3. Compare token on the input with the expected token
    a. If token matches, ask for the next token and continue with the current production, calling required other productions if needed
    b. If token doesn’t match, go to the next production. If there are no more productions, raise an error

*/

using System;
using System.Collections.Generic;


public class Parser
{
    private Scanner scanner;
    private Token currentToken;

    public Parser(Scanner scanner) {
        this.scanner = scanner;
        this.currentToken = scanner.create_token();
    }

    public XmlNode Parse() {
        return ParseXml();
    }

    private XmlNode ParseXml() {
        XmlNode node = new XmlNode("xml");
        //node.AddChild(ParseElement());
        ParseContent(node);
        return node;
    }

    private XmlNode ParseElement() {
        Expect(TokenType.OPEN_TAG);
        string name = Expect(TokenType.NAME).StringValue; 
        XmlNode node = new XmlNode(name); 
        ParseAttributes(node);
        
        if(currentToken.TokenType == TokenType.SLASH)
        {
            Expect(TokenType.SLASH);
            Expect(TokenType.CLOSE_TAG);
            return node;
        }
        
        Expect(TokenType.CLOSE_TAG);
        ParseContent(node);
        Expect(TokenType.OPEN_TAG);
        Expect(TokenType.SLASH);
        Expect(TokenType.NAME, name);
        Expect(TokenType.CLOSE_TAG);
        return node;
    }

    private void ParseAttributes(XmlNode node) {
        if (Check(TokenType.NAME)) {
            ParseAttribute(node);
            ParseAttributes(node);
        }
    }

    private void ParseAttribute(XmlNode node) {
        string name = Expect(TokenType.NAME).StringValue; // if token is null then expect will throw an exeption and null.StringValue will not be called
        Expect(TokenType.EQUALS);
        string value = Expect(TokenType.QUOTE).StringValue; // if token is null then expect will throw an exeption and null.StringValue will not be called
        node.AddAttribute(name, value);
    }

    private void ParseContent(XmlNode node) {
        if (currentToken.TokenType == TokenType.END) { // null token prevention
            throw new Exception("Error: Unexpected end of file");
        }
        else if (Check(TokenType.OPEN_TAG)) {
            Token nextToken = scanner.check_token(0);
            if (nextToken.TokenType == TokenType.SLASH) return;
            node.AddChild(ParseElement());
        }
        else if (Check(TokenType.CDATA)) {
            node.AddChild(new XmlNode("CDATA", currentToken.StringValue));
            Advance();
        }
        else if (Check(TokenType.COMMENT)) {
            node.AddChild(new XmlNode("COMMENT", currentToken.StringValue));
            Advance();
        }

        while (!CheckEndOfNode(node.Name))
        {
            node.AddChild(ParseElement());
        }
    }

    private Token Expect(TokenType expectedTokenType, string expectedValue = null) {
        if (Check(expectedTokenType, expectedValue)) {
            Token token = currentToken;
            Advance();
            return token;
        }

        throw new Exception($"Error: Expected {expectedTokenType} '{expectedValue}', but got {currentToken}");
    }

    private bool Check(TokenType expectedTokenType, string expectedValue = null) {
        return currentToken.TokenType == expectedTokenType && (expectedValue == null || currentToken.StringValue == expectedValue);
    }

    private void Advance() {
        currentToken = scanner.create_token();
    }

    private bool CheckEndOfNode(string nodeName)
    {
        // check if the next tokens form a closing tag with a given name
        Token nextToken = scanner.check_token(0);

        if (nodeName == "xml" && nextToken.TokenType == TokenType.END)
        {
            return true;
        }
        
        if (nextToken.TokenType == TokenType.SLASH)
        {
             nextToken = scanner.check_token(1);
             if (nextToken.TokenType == TokenType.NAME && nextToken.StringValue == nodeName)
             {
                return true;
             }
             return true;
        }
        return false;
    }

}

public class XmlNode {
    public string Name { get; }
    public List<XmlNode> Children { get; }
    public Dictionary<string, string> Attributes { get; }

    public XmlNode(string name) {
        Name = name;
        Children = new List<XmlNode>();
        Attributes = new Dictionary<string, string>();
    }

    public XmlNode(string name, string value) : this(name) {
        Children.Add(new XmlNode("CHAR_DATA", value));
    }

    public void AddChild(XmlNode child) {
        Children.Add(child);
    }

    public void AddAttribute(string name, string value) {
        Attributes[name] = value;
    }

    public override string ToString() {
         return ToString(0);
    }

    private string ToString(int indentLevel) {
        string indent = new string(' ', indentLevel * 4);
        string result = $"{indent}<{Name}>\n";

        foreach (var attribute in Attributes) {
            result += $"{indent}    {attribute.Key} = \"{attribute.Value}\"\n";
        }

        foreach (var child in Children) {
            result += child.ToString(indentLevel + 1);
        }

        result += $"{indent}</{Name}>\n";
        return result;
    }
}
