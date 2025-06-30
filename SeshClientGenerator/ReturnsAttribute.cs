namespace SeshLib.Generators.HttpClient
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnsAttribute(Type returnType) : Attribute
    {
        public Type ReturnType { get; init; } = returnType;
    }
}
