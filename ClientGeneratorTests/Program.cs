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
                verbose: true)
            .AddRange([
                (typeof(TestController), typeof(SeshBase)),
                //(typeof(AutogenerateController), typeof(AutogenerateController))
            ])
            .AllowNullReturns()
        );
    }
}