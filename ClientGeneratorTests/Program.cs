using SeshLib.Generators.HttpClient;

internal class Program
{
    private static void Main(string[] args)
    {            
        SeshClientGenerator.Generate(
            GeneratorArguments
            .Create(
                save: true, 
                printGeneratedCode: true,
                verbose: true,
                outputDir: "C:\\Users\\ws\\Source\\Repos\\sesh\\ClientGeneratorTests\\output\\")
            .AddRange([
                typeof(TestController),
            ])
            .AllowNullReturns()
        );
    }
}