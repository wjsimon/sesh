namespace SSHC.Generator
{
    internal class AutogenerationMethodInformation
    {
        public string MethodName { get; set; }
        public Dictionary<Type, string> ParametersMetaData { get; set; } 
        public Type ReturnType { get; set; }
    }
}
