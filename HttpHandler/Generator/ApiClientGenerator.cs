using System.Reflection;

namespace SSHC.Generator
{
    public class ApiClientGenerator(GeneratorArguments generatorArguments)
    {
        private GeneratorArguments _args = generatorArguments;
        private GeneratorTrace _trace = new();

        public void Generate()
        {
            if (_args.PrintProgress) { _trace.PrintHeader(); }
            foreach (var assembly in CollectAssemblies(_args, _trace))
            {
                Generate(assembly, _args, _trace);
                if (_args.PrintProgress) { _trace.Flush(); }
            }
            if (_args.PrintProgress) { _trace.PrintFooter(); }
        }

        private static IEnumerable<Assembly> CollectAssemblies(GeneratorArguments args, GeneratorTrace trace)
        {
            var mappings = args.FileMappings;
            HashSet<Assembly> set = new HashSet<Assembly>();
            if (mappings.Count == 0) { yield break; }

            for (int i = 0; i < mappings.Count; i++)
            {
                var mapping = mappings.ElementAt(i);
                Assembly? assembly = Assembly.GetAssembly(mapping.Key);
                if (assembly is null)
                {
                    trace.Add($"Could not find assembly for {mapping.Key}, skipping...");
                    continue;
                }
                if (!set.Add(assembly)) 
                { 
                    continue; 
                }

                yield return assembly;
            }
        }

        private static void Generate(Assembly controllerAssembly, GeneratorArguments args, GeneratorTrace trace)
        {
            foreach (var info in ControllerInformationCollector.Collect(controllerAssembly).Where(i => i is not null))
            {
                if (!args.FileMappings.ContainsKey(info!.ControllerType)) { continue; }

                string fileContent = GenerateApiClient(info!, trace);
                string fileName = $"{info!.ControllerRoute}ApiClient.cs";
                string filePath = $"{args.FileMappings[info!.ControllerType]}";

                if (args.PrintGeneratedCode) { trace.Add(fileContent); }

                if (args.Save && !File.Exists(filePath))
                {
                    var dir = Path.GetDirectoryName(filePath);
                    SaveFile(filePath, fileContent);
                    trace.Add($"Succesfully saved generated ApiClient for {info!.ControllerType} to {filePath}");
                }
            }
        }

        private static string GenerateApiClient(AutogenerationInformation generationInformation, GeneratorTrace trace)
        {
            trace.Add($"Starting generation of {generationInformation.ControllerName}...");
            FormattingClassGenerator generator = new();

            generator.AddNamespace($"TestSpace");
            generator.AddClass(generationInformation);
            generator.AddGetOnlyProperty(typeof(string), "ApiControllerName", generationInformation.ControllerName);
            foreach (var method in generationInformation.Methods)
            {
                generator.AddPublicMethod(method);
            }

            string fileContent = generator.Generate();
            return fileContent;
        }

        private static void SaveFile(string fileName, string fileContent)
            => File.WriteAllText(fileName, fileContent);
    }
}