namespace SSHC.Generator
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnsAttribute(Type returnType) : Attribute
    {
        public Type ReturnType { get; init; } = returnType;
    }
}
