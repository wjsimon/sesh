using Simons.Clients.Http;
using Simons.Generators.HttpClient;

internal class Program
{
    private static void Main(string[] args)
    {            
        ApiClientGenerator.Generate(
            GeneratorArguments
            .Create(
                save: true, 
                printGeneratedCode: true,
                verbose: true)
            .AddRange([
                (typeof(TestController), typeof(FastApiClientBase)),
                //(typeof(AutogenerateController), typeof(AutogenerateController))
            ])
            .AllowNullReturns()
        );
    }
}