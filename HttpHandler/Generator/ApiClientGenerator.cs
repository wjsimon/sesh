using System.Reflection;

namespace SSHC.Generator
{
    public static class ApiClientGenerator
    {
        public static void Generate(Assembly controllerAssembly, bool save = true) //support multiple assemblies
        {
            foreach (var info in ControllerInformationCollector.Collect(controllerAssembly).Where(i => i is not null))
            {
                string fileContent = GenerateApiClient(info!);
                string fileName = $"{info!.ControllerRoute}ApiClient.cs";
                string filePath = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\{fileName}";

                if (save && !File.Exists(filePath))
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
