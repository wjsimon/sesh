using System.Text;

namespace SSHC.Generator.Collection
{
    internal class AutogenerationCodeContainer
    {
        private List<string> _usings = new();
        private string _namespace = "";
        private string _class = "";
        private List<string> _publicProperties = new();
        private List<(string, string[])> _publicMethods = new();

        public void AddUsings(IEnumerable<string> usings)
        {
            foreach (string line in usings)
            {
                _usings.Add($"{line};");
            }
        }

        public void SetNamespace(string nameSpace)
        {
            _namespace = nameSpace;
        }

        public void SetClassDefinition(string classDefinition)
        {
            _class = classDefinition;
        }

        public void AddPublicPropertyDefintion(string propertyDefinition)
        {
            _publicProperties.Add(propertyDefinition);
        }

        public void AddPublicMethodDefinition(string methodDefinition, List<string> methodBody)
        {
            _publicMethods.Add((methodDefinition, methodBody.ToArray()));
        }

        public string Get()
        {
            return LinesToString(PrependTabs(GenerateFileContent()));
        }

        public override string ToString()
            => Get();

        private List<string> GenerateFileContent()
        {
            List<string> lines = new();
            lines.AddRange(_usings);
            lines.Add("");
            lines.Add(_namespace);
            lines.Add("{");
            lines.Add(_class);
            lines.Add("{");
            foreach (string line in _publicProperties)
            {
                lines.Add(line);
            }
            lines.Add("");
            foreach ((string Def, string[] Body) method in _publicMethods)
            {
                lines.Add(method.Def);
                if (method.Body.Count() == 1)
                {
                    lines.Add($"\t=> {method.Body.First()}");
                }
                else
                {
                    lines.Add("{");
                    lines.AddRange(method.Body);
                    lines.Add("}");
                }

                lines.Add("");
            }
            lines.RemoveAt(lines.Count - 1); //removes line break after last method
            lines.Add("}");
            lines.Add("}");

            return lines;
        }

        private List<string> PrependTabs(List<string> lines)
        {
            int opened = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] == "{")
                {
                    opened += 1;
                    lines[i] = PrependSingleIndent(lines[i], opened - 1);
                    continue;
                }

                if (lines[i] == "}") { opened -= 1; }
                lines[i] = PrependSingleIndent(lines[i], opened);
            }

            return lines;
        }

        private static string LinesToString(List<string> lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines) { sb.AppendLine(line); }
            return sb.ToString();
        }

        private static string PrependSingleIndent(string str, int tabs)
        {
            if (tabs <= 0) { return str; }
            for (int i = 0; i < tabs; i++) { str = "\t" + str; }
            return str;
        }
    }
}
