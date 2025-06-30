using Sesh.Generators.HttpClient.Helpers;
using System.Text;

namespace Sesh.Generators.HttpClient.Collection.Methods
{
    internal abstract class MethodBodyDescription
    {
        public MethodBodyDescription(AutogenerationMethodInformation methodInfo)
        {
            MethodInfo = methodInfo;
            Make();
        }

        protected AutogenerationMethodInformation MethodInfo { get; private init; }
        protected Type ReturnType => MethodInfo.ReturnType;
        protected int ParameterCount => MethodInfo.ParametersMetaData.Count;
        protected bool HasParameters => ParameterCount > 0;
        protected List<(Type Type, string Name)> Parameters => MethodInfo.ParametersMetaData;
        protected bool HasPayload => MethodInfo.FromBodyIndex > 0;

        private string? _methodPass;
        private string? _taskSnippet;
        private string? _uri;

        protected abstract string MakeMethodPass();
        protected abstract string MakeTaskSnippet();
        protected abstract string MakeUri();

        protected virtual IEnumerable<string> MakeUriDict()
        {
            if (this.HasPayload ? ParameterCount < 4 : ParameterCount < 3) { return []; }

            var parameters = this.HasPayload ? Parameters.Take(ParameterCount-1) : Parameters;
            return DictFromTuples(parameters);
        }

        protected static IEnumerable<string> DictFromTuples(IEnumerable<(Type Type, string Name)>? parameterValues)
        {
            if (parameterValues is null) { return []; }

            List<string> lines = [$"Dictionary<string, string> dict = new();"];
            foreach (var value in parameterValues)
            {
                lines.Add($"dict.Add({$"\"{value.Name}\""}, {value.Name}.ToString());"); //need to implement "ToString" correctly. big issue with datetime; need to do something about that
            }
            lines.Add("");

            return lines;
        }

        protected virtual void Make()
        {
            _methodPass = MakeMethodPass();
            _taskSnippet = MakeTaskSnippet();
            _uri = MakeUri();
        }

        protected static string MakeUri(string parameterValueName)
            => $"(Uri({$"\"{parameterValueName}\""}, {parameterValueName}))";

        protected static string MakeUri((string, string) parameterValueNames)
        {
            return $"(Uri(" +
                $"{$"(\"{parameterValueNames.Item1}\", {parameterValueNames.Item1})"}, " +
                $"{$"(\"{parameterValueNames.Item2}\", {parameterValueNames.Item2})"}" +
                $"))";
        }

        protected static string MakeUri(IEnumerable<string> parameterValueNames)
        {
            List<string> lines = [$"Dictionary<string, string> dict = new();"];
            foreach (var value in parameterValueNames)
            {
                lines.Add($"dict.Add({$"\"{value}\""}, {value}.ToString());"); //need to implement "ToString" correctly. big issue with datetime; need to do something about that
            }

            StringBuilder sb = new StringBuilder();
            foreach(var value in parameterValueNames)
            {
                sb.AppendLine($"({$"\"{value}\""}, {value}.ToString()),");
            }

            return $"Uri({sb.ToString()})";
        }

        public new List<string> ToString()
        {
            var lines = new List<string>();
            lines.AddRange(MakeUriDict()); //returns empty list if theres no parameters
            lines.Add(MakeReturnLine(isSingleLine: lines.Count == 0));
            return lines;
        }

        private string MakeReturnLine(bool isSingleLine = false)
        {
            string returnSnippet = (isSingleLine ? "" : "return ");
            if (MethodInfo.AreNullReturnsAllowed)
            {
                return $"{returnSnippet}{_methodPass}{_taskSnippet}({_uri});";
            }
            else
            {
                return $"{returnSnippet} {_methodPass}{_taskSnippet}({_uri}){MakeNullFallthrough(ReturnType)};";
            }
        }

        private string MakeNullFallthrough(Type returnType)
        {
            return $" ?? {ReturnTypeNullFallthroughSnippet(returnType)}";
        }

        private string ReturnTypeNullFallthroughSnippet(Type type)
        {
            if (TypeHelper.IsEnumerable(type)) { return EnumerableNullFallthrough(); }
            else { return GenericNullFallthrough(); }
        }

        private static string EnumerableNullFallthrough()
        {
            return $"[]";
        }

        private static string GenericNullFallthrough()
            => $"default";     
    }
}
