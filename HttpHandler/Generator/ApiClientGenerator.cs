using System.Reflection;

namespace SSHC.Generator
{
    public class ApiClientGenerator(GeneratorArguments generatorArguments)
    {
        private GeneratorArguments _args = generatorArguments;
        private GeneratorTrace _trace = new();

        public void Generate()
        {
            foreach (var assembly in CollectAssemblies(_args, _trace))
            {
                Generate(assembly, _args, _trace);
            }
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
                    //print
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

                string fileContent = GenerateApiClient(info!);
                string fileName = $"{info!.ControllerRoute}ApiClient.cs";
                string filePath = $"{args.FileMappings[info!.ControllerType]}";

                if (args.Save && !File.Exists(filePath))
                {
                    var dir = Path.GetDirectoryName(filePath);
                    //Console.WriteLine($"Saving generated ApiClient to {filePath}");
                    SaveFile(filePath, fileContent);
                    //Console.WriteLine($"{fileName} saved!");
                }
            }
        }

        private static string GenerateApiClient(AutogenerationInformation generationInformation)
        {
            //Console.WriteLine($"Starting to generate ApiClient for {generationInformation.ControllerName}...");
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