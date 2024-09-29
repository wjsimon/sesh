namespace SSHC.Generator
{
    internal class FormattingClassGenerator
    {
        private static readonly Dictionary<string, string> _primitiveMatches = new Dictionary<string, string>()
        {
            { typeof(string).Name, "string" },
            { typeof(bool).Name, "bool" },
            { typeof(int).Name, "int" },
            { typeof(uint).Name, "uint" },
            { typeof(long).Name, "long" },
            { typeof(float).Name, "float" },
            { typeof(double).Name, "double" },
            { typeof(void).Name, "void" },
            { typeof(object).Name, "object" },
        };

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
            _container.AddPublicPropertyDefintion($"public {SwapPrimitive(returnValue)} {propertyName} => \"{propertyValue}\";");
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
                   $"({string.Join(", ", methodInfo.ParametersMetaData.Select(kvp => $"{SwapPrimitive(kvp.Key)} {kvp.Value}"))}) ";
        }

        private static List<string> MakeMethodBody(AutogenerationMethodInformation methodInfo)
        {
            return new();
        }

        private static string SwapPrimitive(Type type)
        {
            var name = type.Name;
            if (_primitiveMatches.ContainsKey(name))
            {
                return _primitiveMatches[type.Name];
            }

            return type.Name;
        }

        public string Generate()
            => _container.ToString();
        
        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType)
            => returnType != typeof(void) ? $"<{SwapPrimitive(returnType)}>" : "";
    }
}
