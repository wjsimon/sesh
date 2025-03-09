using Simons.Clients.Http;
using Simons.Generators.HttpClient.Collection;
using Simons.Generators.HttpClient.Tracing;
using System.Reflection;

namespace Simons.Generators.HttpClient
{
    public static class ApiClientGenerator
    {
        public const string DEFAULT_INSET = "generated";
        //support for returning null or default / empty collections
        //support interface generation
        //maybe support some Span<T> tech for the generated clients if there's a sensible way to use it

        public static void Generate(GeneratorArguments args)
        {
            GeneratorTrace trace = new();
            if (args.PrintProgress) { trace.PrintHeader(); }
            foreach (var assembly in CollectAssemblies(args, trace))
            {
                Generate(assembly, args, trace);
                if (args.PrintProgress) { trace.Flush(); }
            }

            if (args.PrintProgress) 
            {
                trace.PrintSummary();
                trace.PrintFooter();
            }
        }

        private static IEnumerable<Assembly> CollectAssemblies(GeneratorArguments args, GeneratorTrace trace)
        {
            var mappings = args.PathMappings;
            HashSet<Assembly> set = [];

            if (mappings.Count == 0) { yield break; }

            for (int i = 0; i < mappings.Count; i++)
            {
                var mapping = mappings.ElementAt(i);
                Assembly? assembly = Assembly.GetAssembly(mapping.Key);
                if (assembly is null)
                {
                    if (args.PrintProgress) { AddSkipInfo(mapping.Key, trace); }
                    continue;
                }
                if (!set.Add(assembly)) 
                { 
                    if (args.PrintProgress) { AddDuplicateInfo(mapping.Key, trace); }
                    continue; 
                }

                yield return assembly;
            }
        }

        private static void Generate(Assembly controllerAssembly, GeneratorArguments args, GeneratorTrace trace)
        {
            foreach (var info in ControllerInformationCollector.Collect(controllerAssembly, args).Where(i => i is not null))
            {
                if (!args.PathMappings.ContainsKey(info!.ControllerType)) { continue; }

                info.Apply(args);
                string fileContent = GenerateApiClient(info!, args.TypeMappings[info!.ControllerType], trace);
                string fileName = MakeFileName(info!);
                string filePath = $"{args.PathMappings[info!.ControllerType]}";

                if (args.PrintProgress) {  trace.Add(info); }
                if (args.PrintGeneratedCode) { trace.Add(fileContent);}

                if (args.Save && !File.Exists(filePath))
                {
                    SaveFile(fileName, filePath, fileContent);
                    trace.Add($"Succesfully saved generated ApiClient for {info!.ControllerType} to {filePath}");
                }
                else
                {
                    trace.Add($"{info!.ControllerType} target path: {filePath}\\{fileName}");
                }
            }
        }

        private static string GenerateApiClient(AutogenerationInformation info, Type targetType, GeneratorTrace trace)
            => GenerateApiClient(info, GetNativeNamespace(targetType), trace);

        private static string GenerateApiClient(AutogenerationInformation info, string nameSpace, GeneratorTrace trace)
        {
            trace.Add($"Starting generation of {info.ControllerName}...");
            FormattingClassGenerator generator = new();

            generator
                .AddNamespace(nameSpace)
                .AddClass(info)
                .AddConstructor(
                    $"{info.ControllerRoute}ApiClient", 
                    [new("IHttpWrapper", "httpWrapper")])
                .AddConstructor(
                    $"{info.ControllerRoute}ApiClient",
                    [new("HttpClient", "httpClient")])
                .AddConstructor(
                    $"{info.ControllerRoute}ApiClient",
                    [new("HttpClientHandler", "httpClientHandler")])
                .AddGetOnlyProperty(typeof(string), "ApiControllerName", info.ControllerName);

            foreach (var method in info.Methods)
            {
                generator.AddPublicMethod(method);
            }

            string fileContent = generator.Generate();
            info.AutogenerationResult = AutogenerationResult.Success;

            return fileContent;
        }

        private static string MakeFileName(AutogenerationInformation info)
        {
            if (info.GenerateAsPartial) //partial inset cannot be null if GenerateAsPartial is set
            {
                return $"{info.ControllerRoute}ApiClient.{info.PartialInset}.cs";
            }
            else
            {
                return $"{info.ControllerRoute}ApiClient.cs";
            }
        }
        private static void SaveFile(string fileName, string filePath, string fileContent)
            => File.WriteAllText($"{filePath}\\{fileName}", fileContent);

        private static void AddSkipInfo(Type type, GeneratorTrace trace)
        {
            trace.Add($"Could not find assembly for {type}, skipping...");

            AutogenerationInformation info = ControllerInformationCollector.MakeInfo(type);
            info.AutogenerationResult = AutogenerationResult.Skipped;
            info.Reason = $"Could not find assembly for {type}";
        }

        private static void AddDuplicateInfo(Type type, GeneratorTrace trace)
        {
            trace.Add($"{type} was already generated, skipping...");

            AutogenerationInformation info = ControllerInformationCollector.MakeInfo(type);
            info.AutogenerationResult = AutogenerationResult.Skipped;
            info.Reason = $"{type} was already generated";
        }

        private static string GetNativeNamespace(Type type)
        {
            if (string.IsNullOrEmpty(type.FullName)) { return string.Empty; }
            return string.Join('.', type.FullName.Split('.')[..^1]);
        }
    }
}