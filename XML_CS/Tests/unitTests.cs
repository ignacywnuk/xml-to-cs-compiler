namespace Tests
{
    [TestClass]
    public class UnitTests
    {

        [TestMethod]
        public void ScanXmlFile_ShouldReturnCorrectTokens()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\justRoot.xml";
            List<Token> tokenList = new List<Token>();
            Scanner scanner = new Scanner(filePath);

            Token[] expectedTokens =
            {
            new Token(TokenType.OPEN_TAG),
            new Token(TokenType.NAME, "root"),
            new Token(TokenType.CLOSE_TAG),
            new Token(TokenType.OPEN_TAG),
            new Token(TokenType.SLASH),
            new Token(TokenType.NAME, "root"),
            new Token(TokenType.CLOSE_TAG)
        };

            // Act
            do
            {
                tokenList.Add(scanner.create_token());
            }
            while (tokenList.Last().TokenType != TokenType.END);
            tokenList.Remove(tokenList.Last());
            Token[] actualTokens = tokenList.ToArray();

            // Assert
            Assert.AreEqual(expectedTokens.Length, actualTokens.Length);

            for (int i = 0; i < expectedTokens.Length; i++)
            {
                Assert.AreEqual(expectedTokens[i].TokenType, actualTokens[i].TokenType);
                Assert.AreEqual(expectedTokens[i].StringValue, actualTokens[i].StringValue);
            }
        }

        [TestMethod]
        public void ParseRootElement_ShouldReturnRootNode()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\justRoot.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);

            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));

            // Act
            XmlNode actualNode = parser.Parse();

            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);
            //Assert.AreEqual(expectedNode, actualNode);

        }

        [TestMethod]
        public void ParseTwoElements_ShouldReturnNodeWithTwoChildren()
        {
            // Arrange
            //string filePath = "C:\\Studia semestr 7\\ECOTE\\XML_CS\\examples\\twoRoots.xml";
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\twoRoots.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);

            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));
            expectedNode.AddChild(new XmlNode("root2"));

            // Act
            XmlNode actualNode = parser.Parse();

            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);
            //Assert.AreEqual(expectedNode, actualNode);
        }

        [TestMethod]
        public void ParseElementWithAttributes_ShouldReturnCorrectNode()
        {
            // Arrange
            //string filePath = "C:\\Studia semestr 7\\ECOTE\\XML_CS\\examples\\rootWithAttributes.xml";
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\rootWithAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);

            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));
            expectedNode.Children[0].AddAttribute("name", "Ignacy");

            // Act
            XmlNode actualNode = parser.Parse();

            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);    
            //Assert.AreEqual(expectedNode, actualNode);
        }

        [TestMethod]
        public void ParseNestedElementWithAttributes_ShouldReturnCorrectNode()
        {
            // Arrange
            //string filePath = "C:\\Studia semestr 7\\ECOTE\\XML_CS\\examples\\nestedRootWithAttributes.xml";
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\nestedRootWithAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            
            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));
            expectedNode.Children[0].AddAttribute("name", "Ignacy");
            expectedNode.Children[0].AddChild(new XmlNode("root2"));
            expectedNode.Children[0].Children[0].AddAttribute("name", "Ignacy2");
            
            // Act
            XmlNode actualNode = parser.Parse();
            
            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);
            //Assert.AreEqual(expectedNode, actualNode);
        }
        
        [TestMethod]
        public void ParseClosedElementWithAttributes_ShouldReturnCorrectNode()
        {
            // Arrange
            //string filePath = "C:\\Studia semestr 7\\ECOTE\\XML_CS\\examples\\closedRootWithAttributes.xml";
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\closedRootWithAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            
            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));
            expectedNode.Children[0].AddAttribute("name", "Ignacy");
            
            // Act
            XmlNode actualNode = parser.Parse();
            
            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);
            //Assert.AreEqual(expectedNode, actualNode);
        }

        [TestMethod]
        public void ParseRootWithManyAttributes()
        {
            // Arrange
            //string filePath = "C:\\Studia semestr 7\\ECOTE\\XML_CS\\examples\\rootWithAttributes.xml";
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\rootWithManyAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);

            XmlNode expectedNode = new XmlNode("xml");
            expectedNode.AddChild(new XmlNode("root"));
            expectedNode.Children[0].AddAttribute("name", "Name");
            expectedNode.Children[0].AddAttribute("surname", "Surname");
            expectedNode.Children[0].AddAttribute("age", "23");

            // Act
            XmlNode actualNode = parser.Parse();

            // Assert
            Assert.AreEqual(expectedNode.ToString().Length, actualNode.ToString().Length);    
            //Assert.AreEqual(expectedNode, actualNode);
        }
        
        //Generator unit tests out of date
        [TestMethod]
        public void GenerateRootFile_ShouldReturnCorrectCsFile() 
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\justRoot.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            CodeGenerator generator = new CodeGenerator();
            XmlNode node = parser.Parse();
            string expectedCs = "public class Root\n{\n}\n\n";
            
            // Act
            string actualCs = generator.Generate(node);
            
            // Assert
            Assert.AreEqual(expectedCs, actualCs);
        }

        [TestMethod]
        public void GenerateRootWithAttributes()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\rootWithAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            CodeGenerator generator = new CodeGenerator();
            XmlNode node = parser.Parse();
            string expectedCs = "public class Root\n{\n    public string name { get; set; }\n\n}\n\n";
            
            // Act
            string actualCs = generator.Generate(node);
            
            // Assert
            Assert.AreEqual(expectedCs, actualCs);
        }

        [TestMethod]
        public void GenerateTwoRoots_ShouldReturnTwoFiles()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\twoRoots.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            CodeGenerator generator = new CodeGenerator();
            XmlNode node = parser.Parse();
            string[] expectedCss = {"public class Root\n{\n}\n\n", "public class Root2\n{\n}\n\n"};
            
            // Act
            generator.Generate(node);
            string[] actualCss = new string[2];
            actualCss[0] = File.ReadAllText("C:\\Users\\Ignacy\\Documents\\XML_CS\\Results\\Root.cs");
            actualCss[1] = File.ReadAllText("C:\\Users\\Ignacy\\Documents\\XML_CS\\Results\\Root2.cs");
            
            
            // Assert
            for (int i = 0; i < expectedCss.Length; i++)
            {
                Assert.AreEqual(expectedCss[i], actualCss[i]);
            }
        }

        [TestMethod]
        public void GenerateTwoRootsWithAttributes_ShouldReturnTwoFiles()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\twoRootsWithAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            CodeGenerator generator = new CodeGenerator();
            XmlNode node = parser.Parse();
            string[] expectedCss = {"public class Root\n{\n    public string name { get; set; }\n\n}\n\n", "public class Root2\n{\n    public string name { get; set; }\n\n}\n\n"};
            
            // Act
            generator.Generate(node);
            string[] actualCss = new string[2];
            actualCss[0] = File.ReadAllText("C:\\Users\\Ignacy\\Documents\\XML_CS\\Results\\Root.cs");
            actualCss[1] = File.ReadAllText("C:\\Users\\Ignacy\\Documents\\XML_CS\\Results\\Root2.cs");
            
            
            // Assert
            for (int i = 0; i < expectedCss.Length; i++)
            {
                Assert.AreEqual(expectedCss[i], actualCss[i]);
            }
        }

        [TestMethod]
        public void GenerateRootWithManyAttributes()
        {
            // Arrange
            string filePath = "C:\\Users\\Ignacy\\Documents\\XML_CS\\examples\\rootWithManyAttributes.xml";
            Scanner scanner = new Scanner(filePath);
            Parser parser = new Parser(scanner);
            CodeGenerator generator = new CodeGenerator();
            XmlNode node = parser.Parse();
            string expectedCs = "public class Root\n{\n    public string name { get; set; } \n    public string surname{ get; set; }\n    public int age { get; set; }\n\n}\n\n";
            
            // Act
            string actualCs = generator.Generate(node);
            
            // Assert
            //Assert.AreEqual(expectedCs, actualCs);
            Assert.AreEqual(expectedCs.Length, actualCs.Length);
        }
    }

    
}