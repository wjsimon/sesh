using System.Text;

namespace SSHC.Generator
{
    internal class FormattingClassGenerator
    {
        private AutogenerationCodeContainer _container = new();

        public FormattingClassGenerator AddUsings(IEnumerable<string> usings)
        {
            _container.AddUsings(usings);
            return this;
        }

        public FormattingClassGenerator AddNamespace(string nameSpace)
        {
            _container.SetNamespace($"namespace {nameSpace}");
            return this;
        }

        public FormattingClassGenerator AddClass(AutogenerationInformation classInfo)
        {
            _container.SetClassDefinition($"public class {classInfo.ControllerRoute}ApiClient : ApiClient");
            return this;
        }

        public FormattingClassGenerator AddGetOnlyProperty(Type returnValue, string propertyName, string propertyValue)
        {
            _container.AddPublicPropertyDefintion($"public {returnValue.Name} {propertyName} => \"{propertyValue}\";");
            return this;
        }

        public FormattingClassGenerator AddPublicMethod(AutogenerationMethodInformation methodInfo)
        {
            //get only here; this is a GET
            _container.AddPublicMethodDefinition(
                MakeMethodDefinition(methodInfo),
                MakeMethodBody(methodInfo)
            );

            return this;
        }

        private static string MakeMethodDefinition(AutogenerationMethodInformation methodInfo)
        {
            return $"public Task{TaskSnippetFromMethodReturnAnnotation(methodInfo.ReturnType)} {methodInfo.MethodName}" +
                            $"({string.Join(", ", methodInfo.ParametersMetaData.Select(kvp => $"{kvp.Key.Name} {kvp.Value}"))}) ";
        }

        private static List<string> MakeMethodBody(AutogenerationMethodInformation methodInfo)
        {
            return new();
        }

        public string Generate()
        {
            return _container.ToString();
        }

        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType)
        {
            var snippet = returnType != typeof(void) ? $"<{returnType.Name}>" : "";
            return snippet;
        }
    }
}
