using Simons.Generators.Http.Helpers;

namespace Simons.Generators.Http.Collection.Methods
{
    internal sealed class PostMethodBody : MethodBodyDescription
    {
        private PostMethodBody(AutogenerationMethodInformation methodInfo) : base(methodInfo) { }

        private Type PayloadType => ParameterCount > 0 ? Parameters.Last().Type : typeof(void);
        private string PayloadName => ParameterCount > 0 ? Parameters.Last().Name : string.Empty;
        private bool HasPayload => MethodInfo.FromBodyIndex > 0;

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
            => $"return this.PostAsync{TaskSnippetFromMethodReturnAnnotation(returnType, payloadType)}(Uri(), {payloadName});";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, string parameterValueName)
            => $"return this.PostAsync{TaskSnippetFromMethodReturnAnnotation(returnType, payloadType)}" +
                $"(Uri({$"\"{parameterValueName}\""}, {parameterValueName}), {payloadName})";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, (string, string) parameterValueNames)
            => $"return this.PostAsync{TaskSnippetFromMethodReturnAnnotation(returnType, payloadType)}" +
                $"(Uri(" +
                $"{$"(\"{parameterValueNames.Item1}\", {parameterValueNames.Item1})"}, " +
                $"{$"(\"{parameterValueNames.Item2}\", {parameterValueNames.Item2})"}" +
                $"), {payloadName})";

        private static string MakeUri(Type returnType, Type payloadType, string payloadName, List<(Type Type, string Name)> parameterValues)
            => $"return this.PostAsync{TaskSnippetFromMethodReturnAnnotation(returnType, payloadType)}(Uri(dict), {payloadName})";

        private static string TaskSnippetFromMethodReturnAnnotation(Type returnType, Type payloadType)
        {
            var returnStr = returnType != typeof(void) ? $"<{PrimitiveHelper.SwapPrimitive(returnType)}, " : "<";
            return $"{returnStr}{(payloadType != typeof(void) ? $"{PrimitiveHelper.SwapPrimitive(payloadType)}" : "")}>";
        }
    }
}
