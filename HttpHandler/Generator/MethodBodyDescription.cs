namespace SSHC.Generator
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
        protected int ParameterCount => MethodInfo.ParametersMetaData.Count();
        protected bool HasParameters => ParameterCount > 0;
        protected List<(Type Type, string Name)> Parameters => MethodInfo.ParametersMetaData;

        private string _methodPass { get; set; }
        private string _taskSnippet { get; set; }
        private string _uri { get; set; }

        protected abstract string MakeMethodPass();
        protected abstract string MakeTaskSnippet();
        protected abstract string MakeUri();
        protected virtual List<string> MakeUriDict()
        {
            if (ParameterCount < 2) { return new(); }
            return DictFromTuples(Parameters);
        }

        protected List<string> DictFromTuples(List<(Type Type, string Name)>? parameterValues)
        {
            var lines = new List<string>();
            lines.Add($"Dictionary<string, string> dict = new();"); //using the newer instance syntax breaks my bracket parsing :(
            foreach (var value in Parameters)
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
        protected string MakeUri(string parameterValueName)
            => $"(Uri({$"\"{parameterValueName}\""}, {parameterValueName}))";

        protected string MakeUri((string, string) parameterValueNames)
        {
            return $"(Uri(" +
                $"{$"(\"{parameterValueNames.Item1}\", {parameterValueNames.Item1})"}, " +
                $"{$"(\"{parameterValueNames.Item2}\", {parameterValueNames.Item2})"}" +
                $"))";
        }

        protected string MakeUri(IEnumerable<string> parameterValueNames)
        {
            var lines = new List<string>();
            lines.Add($"Dictionary<string, string> dict = new();");
            foreach (var value in parameterValueNames)
            {
                lines.Add($"dict.Add({$"\"{value}\""}, {value}.ToString());"); //need to implement "ToString" correctly. big issue with datetime; need to do something about that
            }

            return $"Uri(dict)";
        }

        public new List<string> ToString()
        {
            var lines = new List<string>();
            lines.AddRange(MakeUriDict()); //returns empty list if theres no parameters
            lines.Add(MakeReturnLine());
            return lines;
        }

        private string MakeReturnLine()
            => $"return {_methodPass}{_taskSnippet}({_uri});";
    }
}
