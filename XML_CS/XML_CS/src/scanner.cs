/*
Algorithm of scanner:
1. Read the input string (reformatted XML file)
2. Go through the XML file character by character
3 .Perform actions based on character until the input string is over:
    < : Create ‘OPEN_TAG” token (no value)
    > : Create ‘CLOSE_TAG’ token (no value)
    = : Create ‘EQUALS’ token  (no value)
    “ : Create ‘QUOTE’ token (string value until next quote token)
    / : Create ‘SLASH’ token
    ! : Check the next two characters to see if it is the start of a CDATA section or a comment. Then skip to the next token
    Letter or _ : Create ‘NAME’ token, read input until non-letter and _ character occurred
    Non-letter or non-whitespace : Create ‘CHAR_DATA’, read input until letter, whitespace or ‘<’ character occurred 
    Whitespace : Ignore it and continue
    Other : Raise an error
4. Save the state of the input file (position of scanner inside of the file)
5. Return a token
*/

public class Scanner
{
    private string path_to_xml { get; set; }
    private long current_position { get; set; }
    
    private Token? last_token { get; set; }
    public Scanner(string filename)
    {
        // set the position to the first character of the file
        this.path_to_xml = filename;
        using StreamReader reader = new StreamReader(this.path_to_xml);
        this.current_position = 0;
    }

    public Token create_token()
    {
        // opening the file at the current position
        using StreamReader reader = new StreamReader(this.path_to_xml);
        reader.BaseStream.Seek(this.current_position, SeekOrigin.Begin);
        int character = reader.Read();
        if (character == -1) return new Token(TokenType.END);
        char currentChar = (char)character;

        // checking the character and creating a token using a switch
        switch (currentChar) {
            case '<':
                current_position++;
                last_token = new Token(TokenType.OPEN_TAG);
                return last_token;

            case '>':
                current_position++;
                last_token =  new Token(TokenType.CLOSE_TAG);
                return last_token;

            case '=':
                string value = "";
                char nextChar = (char)reader.Read();
                current_position++;
                if (nextChar != '"') {
                    while (true) {
                        if (nextChar == ' ') {
                         // if the next character is a whitespace, then stop reading
                            break;
                        }
                        else {
                            value += nextChar;
                        }
                        nextChar = (char)reader.Read();
                        current_position++;
                    }
                    last_token = new Token(TokenType.EQUALS, value);
                    return last_token;
                }
                last_token = new Token(TokenType.EQUALS);
                return last_token;

            case '"':
                value = "";
                while (true) {
                    nextChar = (char)reader.Read();
                    current_position++;
                    if (nextChar == '"') {
                        // if the next character is a quote, then stop reading
                        current_position++;
                        break;
                    }
                    else {
                        value += nextChar;
                    }
                }
                last_token = new Token(TokenType.QUOTE, value);
                return last_token;

            case '/':
                current_position++;
                last_token =  new Token(TokenType.SLASH);
                return last_token;

            case '!': 
                // checking the next two characters to see if it is the start of a CDATA section or a comment
                nextChar = (char)reader.Read();
                current_position++;
                char nextNextChar = (char)reader.Read();
                current_position++;
                if (nextChar == '-' && nextNextChar == '-') {
                    while (true) {
                        nextChar = (char)reader.Read();
                        current_position++;
                        nextNextChar = (char)reader.Read();
                        current_position++;
                        if (nextChar == '-' && nextNextChar == '-') {
                            // if the next two characters are --, then stop reading
                            nextNextChar = (char)reader.Read(); // go to the next character after the comment
                            current_position++;
                            break;
                        }
                    }
                    last_token =  new Token(TokenType.COMMENT);
                    return last_token;
                }
                else if (nextChar == '[' && nextNextChar == 'C') {
                    while(true) {
                        nextChar = (char)reader.Read();
                        current_position++;
                        nextNextChar = (char)reader.Read();
                        current_position++;
                        if (nextChar == ']' && nextNextChar == ']') {
                            // if the next two characters are ]], then stop reading
                            nextNextChar = (char)reader.Read(); // go to the next character after the CDATA
                            current_position++;
                            break;
                        }
                    }
                    last_token = new Token(TokenType.CDATA);
                    return last_token;
                }
                else {
                    // if it is not, then raise an error
                    throw new Exception("Error: invalid character");
                }

            case char letter when (letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z') || letter == '_':
                // if the character is a letter or an underscore, create a NAME token
                string name = "";
                name += letter;
                while (true) {
                    char nextLetter = (char)reader.Read();
                    current_position++;
                    if ((nextLetter >= 'a' && nextLetter <= 'z') || (nextLetter >= 'A' && nextLetter <= 'Z') || nextLetter == '_' || (nextLetter >= '0' && nextLetter <= '9')) {
                        name += nextLetter;
                    }
                    else {
                        // if the next character is not a letter, an underscore or a number, then stop reading
                        break;
                    }
                }
                // set the position to the next character after the name
                last_token = new Token(TokenType.NAME, name);
                return last_token;

            case char endOfLine when endOfLine == '\r' || endOfLine == '\n':
                current_position++;
                last_token = this.create_token();
                return last_token;

            case char nonLetter when !(nonLetter >= 'a' && nonLetter <= 'z') && !(nonLetter >= 'A' && nonLetter <= 'Z') && nonLetter != '_':
                // if the character is not a letter, an underscore or a number, create a CHAR_DATA token
                current_position++;
                last_token = this.create_token();
                return last_token;

        }
        return new Token(TokenType.END);
    }

    public Token check_token(int offset)
    {
        // opening the file at the current position
        using StreamReader reader = new StreamReader(this.path_to_xml);
        long position = current_position + offset;
        reader.BaseStream.Seek(position, SeekOrigin.Begin);
        int character = reader.Read();
        if (character == -1) return new Token(TokenType.END);
        char currentChar = (char)character;

        // checking the character and creating a token using a switch
        switch (currentChar)
        {
            case '<':
                position++;
                return new Token(TokenType.OPEN_TAG);

            case '>':
                position++;
                return new Token(TokenType.CLOSE_TAG);

            case '=':
                string value = "";
                char nextChar = (char)reader.Read();
                position++;
                if (nextChar != '"')
                {
                    while (true)
                    {
                        if (nextChar == ' ')
                        {
                            // if the next character is a whitespace, then stop reading
                            break;
                        }
                        else
                        {
                            value += nextChar;
                        }
                        nextChar = (char)reader.Read();
                        position++;
                    }
                    return new Token(TokenType.EQUALS, value);
                }
                return new Token(TokenType.EQUALS);

            case '"':
                value = "";
                while (true)
                {
                    nextChar = (char)reader.Read();
                    position++;
                    if (nextChar == '"')
                    {
                        // if the next character is a quote, then stop reading
                        break;
                    }
                    else
                    {
                        value += nextChar;
                    }
                }
                return new Token(TokenType.QUOTE, value);

            case '/':
                position++;
                return new Token(TokenType.SLASH);

            case '!':
                // checking the next two characters to see if it is the start of a CDATA section or a comment
                nextChar = (char)reader.Read();
                position++;
                char nextNextChar = (char)reader.Read();
                position++;
                if (nextChar == '-' && nextNextChar == '-')
                {
                    while (true)
                    {
                        nextChar = (char)reader.Read();
                        position++;
                        nextNextChar = (char)reader.Read();
                        position++;
                        if (nextChar == '-' && nextNextChar == '-')
                        {
                            // if the next two characters are --, then stop reading
                            nextNextChar = (char)reader.Read(); // go to the next character after the comment
                            position++;
                            break;
                        }
                    }
                    return new Token(TokenType.COMMENT);
                }
                else if (nextChar == '[' && nextNextChar == 'C')
                {
                    while (true)
                    {
                        nextChar = (char)reader.Read();
                        position++;
                        nextNextChar = (char)reader.Read();
                        position++;
                        if (nextChar == ']' && nextNextChar == ']')
                        {
                            // if the next two characters are ]], then stop reading
                            nextNextChar = (char)reader.Read(); // go to the next character after the CDATA
                            position++;
                            break;
                        }
                    }
                    return new Token(TokenType.CDATA);
                }
                else
                {
                    // if it is not, then raise an error
                    throw new Exception("Error: invalid character");
                }

            case char letter when (letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z') || letter == '_':
                // if the character is a letter or an underscore, create a NAME token
                string name = "";
                name += letter;
                while (true)
                {
                    char nextLetter = (char)reader.Read();
                    position++;
                    if ((nextLetter >= 'a' && nextLetter <= 'z') || (nextLetter >= 'A' && nextLetter <= 'Z') || nextLetter == '_' || (nextLetter >= '0' && nextLetter <= '9'))
                    {
                        name += nextLetter;
                    }
                    else
                    {
                        // if the next character is not a letter, an underscore or a number, then stop reading
                        break;
                    }
                }
                // set the position to the next character after the name
                return new Token(TokenType.NAME, name);

            case char endOfLine when endOfLine == '\r' || endOfLine == '\n':
                position++;
                return this.create_token();

            case char nonLetter when !(nonLetter >= 'a' && nonLetter <= 'z') && !(nonLetter >= 'A' && nonLetter <= 'Z') && nonLetter != '_':
                position++;
                return this.create_token();

        }
        return new Token(TokenType.END);
    }

}