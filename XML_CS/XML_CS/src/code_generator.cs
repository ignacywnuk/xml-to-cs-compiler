public class CodeGenerator
{
    private string? generatedCode;

    public CodeGenerator()
    {
        this.generatedCode = null;
    }

    public string Generate(XmlNode rootNode)
    {
        foreach (var child in rootNode.Children)
        {
            GenerateCode(child);
        }

        return generatedCode;
    }
    
    private void GenerateCode(XmlNode rootNode)
    {
        generatedCode = "";
        GenerateClassDefinition(rootNode);
        WriteToFile(rootNode.Name, generatedCode);
        GenerateObjects(rootNode);
        GenerateNestedClasses(rootNode);
    }

    private void GenerateClassDefinition(XmlNode node)
    {
        generatedCode += $"public class {FormatClassName(node.Name)}\n";
        generatedCode += "{\n";
        GenerateAttributeProperties(node);
        GenerateChildProperties(node);
        GenerateConstructors(node);
        GenerateOverrideFunctions(node);
        generatedCode += "}\n\n";
    }

    private void GenerateAttributeProperties(XmlNode node)
    {
        foreach (var attribute in node.Attributes)
        {
            generatedCode += $"    public {GetAttributeDataType(attribute.Value)} {FormatName(attribute.Key)} {{ get; set; }}\n";
        }
        if (node.Attributes.Count > 0)
            generatedCode += "\n";
    }

    private string GetAttributeDataType(string attributeValue)
    {
        if (int.TryParse(attributeValue, out _))
            return "int";
        else if (float.TryParse(attributeValue, out _))
            return "float";
        else if (double.TryParse(attributeValue, out _))
            return "double";
        else if (bool.TryParse(attributeValue, out _))
            return "bool";
        else
            return "string";
    }
    
    private void GenerateConstructors(XmlNode node)
    {
        generatedCode += $"    public {FormatName(node.Name)}() {{ }}\n";

        if (node.Attributes.Count + node.Children.Count() > 0)
        {
            generatedCode += $"    public {FormatName(node.Name)}(";
            int attributeCount = node.Attributes.Count + node.Children.Count;
            int count = 0;

            foreach (var attribute in node.Attributes)
            {
                generatedCode += $"{GetAttributeDataType(attribute.Value)} {FormatName(attribute.Key)}";
                count++;
                if (count < attributeCount)
                    generatedCode += ", ";
            }
            
            foreach (var child in node.Children)
            {
                generatedCode += $"{FormatClassName(child.Name)} {FormatName(child.Name)}";
                count++;
                if (count < attributeCount)
                    generatedCode += ", ";
            }

            generatedCode += ")\n";
            generatedCode += "    {\n";

            foreach (var attribute in node.Attributes)
            {
                generatedCode += $"        {FormatName(attribute.Key)} = {FormatName(attribute.Key)};\n";
            }
            
            foreach (var child in node.Children)
            {
                generatedCode += $"        {FormatName(child.Name)} = new {FormatClassName(child.Name)}({FormatName(child.Name)});\n";
            }

            generatedCode += "    }\n\n";
        }
    }

    private void GenerateChildProperties(XmlNode node)
    {
        foreach (var child in node.Children)
        {
            generatedCode += $"    public {FormatClassName(child.Name)} {FormatName(child.Name)} {{ get; set; }}\n";
        }
        if (node.Children.Count > 0)
            generatedCode += "\n";
    }

    private void GenerateNestedClasses(XmlNode node)
    {
        foreach (var child in node.Children)
        {
            GenerateCode(child);
        }
    }

    private void GenerateObjects(XmlNode rootNode)
    {
        string objectDeclarations = GenerateObjectDeclaration(rootNode);

        string filePath = "..\\Results\\main.cs";
        //string filePath = "C:\\Studia Semestr 7\\ECOTE\\XML_CS\\Results\\main.cs";
        if (File.Exists(filePath))
        {
            File.AppendAllText(filePath, objectDeclarations);
        }
        else
        {
            File.WriteAllText(filePath, objectDeclarations);
        }
    }

    private string GenerateObjectDeclaration(XmlNode node)
    {
        string objectDeclarations = "";

        string className = FormatClassName(node.Name);
        string objectName = FormatName(node.Name);
        List<string> constructorArguments = new List<string>();

        foreach (var attribute in node.Attributes)
        {
            constructorArguments.Add($"\"{attribute.Value}\"");
        }

        objectDeclarations += $"{className} {objectName} = new {className}({string.Join(", ", constructorArguments)});\n";

        return objectDeclarations;
    }

    private void GenerateToString(XmlNode node)
    {
        if (node.Attributes.Count() + node.Children.Count() == 0) return;
        int count = 0;
        generatedCode += "    public override string ToString()\n";
        generatedCode += "    {\n";
        generatedCode += "        return $\"";
        foreach (var attribute in node.Attributes)
        {
            generatedCode += $"{attribute.Key}: {{{FormatName(attribute.Key)}}}";
            count++;
            if (count < node.Attributes.Count())
            {
                generatedCode += ", ";
            }
            else generatedCode += "\";\n";
        }

        count = 0;
        foreach (var child in node.Children)
        {
            generatedCode += $"{FormatClassName(child.Name)}: {{{FormatName(child.Name)}}}";
            count++;
            if (count < node.Children.Count())
            {
                generatedCode += ", ";
            }
            else generatedCode += "\";\n";
        }

        generatedCode += "    }\n\n";
    }
    
    private void GenerateOverrideFunctions(XmlNode node)
    {
        GenerateToString(node);
        generatedCode += "    public override bool Equals(object obj)\n";
        generatedCode += "    {\n";
        generatedCode += "        if (obj == null || GetType() != obj.GetType())\n";
        generatedCode += "        {\n";
        generatedCode += "            return false;\n";
        generatedCode += "        }\n";
        generatedCode += "        return ToString() == obj.ToString();\n";
        generatedCode += "    }\n\n";
        
        generatedCode += "    public override int GetHashCode()\n";
        generatedCode += "    {\n";
        generatedCode += "        return ToString().GetHashCode();\n";
        generatedCode += "    }\n\n";
        
        generatedCode += "    public override Type GetType()\n";
        generatedCode += "    {\n";
        generatedCode += "        return typeof(" + FormatClassName(node.Name) + ");\n";  
        generatedCode += "    }\n\n";
    }

    private void WriteToFile(string className, string code)
    {
        //string fileName = $"C:\\Users\\Ignacy\\Documents\\XML_CS\\Results\\{className}.cs";
        //string fileName = $"C:\\Studia Semestr 7\\ECOTE\\XML_CS\\Results\\{className}.cs";
        string fileName = $"..\\Results\\{className}.cs";
        File.WriteAllText(fileName, code);
    }

    private string FormatClassName(string name)
    {
        return char.ToUpper(name[0]) + name.Substring(1);
    }
    
    private string FormatName(string name)
    {
        return char.ToLower(name[0]) + name.Substring(1);
    }
    
}
