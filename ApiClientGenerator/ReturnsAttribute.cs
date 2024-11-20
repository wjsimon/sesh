namespace Simons.Generators.Http
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnsAttribute(Type returnType) : Attribute
    {
        public Type ReturnType { get; init; } = returnType;
    }
}
