using SeshLib.Generators.HttpClient.Helpers;

namespace SeshLib.Generators.HttpClient.Collection.Methods
{
    internal sealed class GetMethodBody : MethodBodyDescription
    {
        private GetMethodBody(AutogenerationMethodInformation methodInfo) : base(methodInfo) { }
        public static GetMethodBody Create(AutogenerationMethodInformation methodInfo)
            => new GetMethodBody(methodInfo);

        protected override string MakeMethodPass()
            => $"GetAsync";

        protected override string MakeTaskSnippet()
            => TaskSnippetFromMethodReturnAnnotation(MethodInfo.ReturnType);

        protected override string MakeUri()
        {
            switch (ParameterCount)
            {
                case 0:
                    return "Uri()";
                case 1:
                    return MakeUri(Parameters.First().Name);
                case 2:
                    return MakeUri((Parameters.ElementAt(0).Name, Parameters.ElementAt(1).Name));
                default:
                    return MakeUri(Parameters.Select(p => p.Name));
            }
        }

        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType)
            => returnType != typeof(void) ? TaskReturnValueSnippet(returnType) : "";

        private static string TaskReturnValueSnippet(Type returnType)
        {
            return $"<{TypeHelper.TypeAsCodeSnippet(returnType)}>";
        }
    }
}
