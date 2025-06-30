using Sesh.Clients.Http;
using Sesh.Generators.HttpClient;

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
                outputDir: "D:\\simonssoftware\\libs\\http-api-prototyping\\simons-http\\ClientGeneratorTests\\output")
            .AddRange([
                (typeof(TestController), typeof(SeshBase)),
                //(typeof(AutogenerateController), typeof(AutogenerateController))
            ])
            .AllowNullReturns()
        );
    }
}