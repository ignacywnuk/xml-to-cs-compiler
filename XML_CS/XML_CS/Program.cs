using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
         if (args.Length == 0)
         {
             Console.WriteLine("Please provide the path to the XML file as a command line argument.");
             return;
         }

        string filePath = args[0];
        Scanner scanner = new Scanner(filePath);
        Parser parser = new Parser(scanner);
        CodeGenerator generator = new CodeGenerator();
        XmlNode rootNode = parser.Parse();
        generator.Generate(rootNode);
        
    }
}
