namespace SSHC.Generator
{
    internal class AutogenerationCodeContainer
    {
        private List<string> _lines = new();
        private Usings _usings = new();
        private Namespace _namespace = new();
        private Class _class = new();
        private PublicProperties _publicProperties = new();
        private PublicMethods _publicMethods = new();

        public void AddUsings(IEnumerable<string> usings) { }
        public void AddNamespace(string nameSpace) { }
        public void AddClassDefintion(AutogenerationInformation classInfo) { }
        public void AddPublicGetOnlyProperty(Type returnValue, string propertyName, string propertyValue) { }
        public void AddPublicMethod(AutogenerationMethodInformation methodInfo) { }
        public string Get() { }
        public override string ToString()
            => Get();

        private class CodeGenerationBlock { }
        private class Usings { }
        private class Namespace : CodeGenerationBlock { }
        private class Class : CodeGenerationBlock { }
        private class PublicProperties : CodeGenerationBlock { }
        private class PublicMethods : CodeGenerationBlock { }
    }
}
