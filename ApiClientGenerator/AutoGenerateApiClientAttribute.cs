namespace Simons.Generators.HttpClient
{
    /// <summary>
    /// Used exclusively to mark an ApiController as AutoGeneratable by the code generator, which will result in the according
    /// ApiClient class being generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoGenerateApiClientAttribute : Attribute;
}
