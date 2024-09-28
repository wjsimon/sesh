using System.Reflection;
using System.Text;

namespace SSHC.Generator
{
    internal class FormattingClassGenerator
    {
        private AutogenerationCodeContainer _container = new();

        public FormattingClassGenerator AddUsings(IEnumerable<string> usings)
        {
            foreach (string line in usings)
            {
                _container.Lines.Insert(0, $"{line};");
            }

            return this;
        }

        public FormattingClassGenerator AddNamespace(string nameSpace)
        {
            _container.Lines.Add($"namespace {nameSpace}");
            return this;
        }

        public FormattingClassGenerator AddClassDefinition(AutogenerationInformation classInfo)
        {
            _container.Lines.Add($"public class {classInfo.ControllerRoute}ApiClient : ApiClient");
            return this;
        }

        public FormattingClassGenerator AddGetOnlyProperty(Type returnValue, string propertyName, string propertyValue)
        {
            _container.Lines.Add($"public {returnValue.Name} {propertyName} => {propertyValue};");
            return this;
        }

        public FormattingClassGenerator AddMethodDefintion(AutogenerationMethodInformation methodInfo)
        {
            //get only here; this is a GET
            _container.Lines.Add(
                $"public Task {methodInfo.MethodName}" +
                $"({string.Join(", ", methodInfo.ParametersMetaData.Select(kvp => $"{kvp.Key} {kvp.Value}"))})"
            );

            return this;
        }

        public string Generate()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in _container.Lines)
            {
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType)
        {
            //<{ methodInfo.ReturnType}>
            var snippet = $"";
            return snippet;
        }
    }
}
