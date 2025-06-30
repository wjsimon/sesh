using Sesh.Generators.HttpClient.Helpers;

namespace Sesh.Generators.HttpClient.Collection.Methods
{
    internal sealed class PostMethodBody : MethodBodyDescription
    {
        private PostMethodBody(AutogenerationMethodInformation methodInfo) : base(methodInfo) { }

        private Type PayloadType => ParameterCount > 0 ? Parameters.Last().Type : typeof(void);
        private string PayloadName => ParameterCount > 0 ? Parameters.Last().Name : string.Empty;

        public static PostMethodBody Create(AutogenerationMethodInformation methodInfo)
            => new PostMethodBody(methodInfo);

        protected override string MakeMethodPass()
            => $"PostAsync";

        protected override string MakeTaskSnippet()
            => TaskSnippetFromMethodReturnAnnotation(ReturnType, PayloadType);

        protected override string MakeUri()
            => HasPayload ? MakePayloadUri() : MakeUriInternal();

        private string MakeUriInternal() => ParameterCount switch
        {
            0 => "Uri()",
            1 => MakeUri(Parameters.First().Name),
            2 => MakeUri((Parameters.ElementAt(0).Name, Parameters.ElementAt(1).Name)),
            _ => MakeUri(Parameters.Select(p => p.Name))
        };

        private string MakePayloadUri() => ParameterCount switch
        {
            1 => MakeUri(ReturnType, PayloadType, PayloadName),
            2 => MakeUri(ReturnType, PayloadType, PayloadName, Parameters.First().Name),
            3 => MakeUri(ReturnType, PayloadType, PayloadName, (Parameters.ElementAt(0).Name, Parameters.ElementAt(1).Name)),
            _ => MakeUri(ReturnType, PayloadType, PayloadName, Parameters)
        };

        private static string MakeUri(Type returnType, Type payloadType, string payloadName)
            => $"Uri(), {payloadName});";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, string parameterValueName)
            => $"Uri({$"\"{parameterValueName}\""}, {parameterValueName}), {payloadName})";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, (string, string) parameterValueNames)
            => $"Uri([" +
                $"{$"(\"{parameterValueNames.Item1}\", {parameterValueNames.Item1}.ToString())"}, " +
                $"{$"(\"{parameterValueNames.Item2}\", {parameterValueNames.Item2}.ToString())"}]), " +
                $"{payloadName}";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, List<(Type Type, string Name)> parameterValues)
            => $"Uri(dict), {payloadName}";

        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType, Type payloadType)
        {
            var returnStr = returnType != typeof(void) ? $"<{TypeHelper.TypeAsCodeSnippet(payloadType)}, " : "<";
            return $"{returnStr}{(payloadType != typeof(void) ? $"{TypeHelper.TypeAsCodeSnippet(returnType)}" : "")}>";
        }
    }
}
