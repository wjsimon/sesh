using System.Reflection;

namespace SSHC.Generator
{
    public class ApiClientGenerator(GeneratorArguments generatorArguments)
    {
        private GeneratorArguments _args = generatorArguments;
        public void Generate() 
        {
            var classFilePath = DirectoryPig.GetFileDirectory(typeof(ApiClientGenerator), _args.FileNameMatchesClassName);
            Console.WriteLine(classFilePath);
            return;

            var mappings = _args.FileMappings;
            if (mappings.Count == 0) { return; }

            for (int i = 0; i < mappings.Count; i++)
            {
                var mapping = mappings.ElementAt(i);
                Assembly? assembly = Assembly.GetAssembly(mapping.Key);
                if (assembly is null)
                {
                    //print
                    continue;
                }

                Generate(assembly, _args, i);
            }
        } 

        private static void Generate(Assembly controllerAssembly, GeneratorArguments args, int index)
        {
            var location = args.FileMappings.ElementAt(index).Value;
            foreach (var info in ControllerInformationCollector.Collect(controllerAssembly).Where(i => i is not null))
            {
                string fileContent = GenerateApiClient(info!);
                string fileName = $"{info!.ControllerRoute}ApiClient.cs";
                string filePath = $"{location}";

                if (args.Save && !File.Exists(filePath))
                {
                    var dir = Path.GetDirectoryName(filePath);
                    Console.WriteLine($"Saving generated ApiClient to {filePath}");
                    SaveFile(filePath, fileContent);
                    Console.WriteLine($"{fileName} saved!");
                }
            }
        }

        private static string GenerateApiClient(AutogenerationInformation generationInformation)
        {
            Console.WriteLine($"Starting to generate ApiClient for {generationInformation.ControllerName}...");
            FormattingClassGenerator generator = new();

            generator.AddNamespace($"TestSpace");
            generator.AddClass(generationInformation);
            generator.AddGetOnlyProperty(typeof(string), "ApiControllerName", generationInformation.ControllerName);
            foreach (var method in generationInformation.Methods)
            {
                generator.AddPublicMethod(method);
            }

            string fileContent = generator.Generate();
            Console.WriteLine(fileContent);

            return fileContent;
        }

        private static void SaveFile(string fileName, string fileContent)
            => File.WriteAllText(fileName, fileContent);
    }
}
