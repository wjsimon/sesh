namespace SSHC.Generator
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ReturnsAttribute(Type? returnType) : Attribute 
    {
        public Type? ReturnType { get; init; } = returnType;
    }
}
