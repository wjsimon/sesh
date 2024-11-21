using Simons.Clients.Http;
using Simons.Generators.ApiClient;
using Simons.Generators.ApiClient.Controllers;

internal class Program
{
    private static void Main(string[] args)
    {            
        ApiClientGenerator.Generate(
            GeneratorArguments
            .Create(
                save: false, 
                printGeneratedCode: true)
            .AddRange([
                (typeof(TestController), typeof(ApiClient)),
                //(typeof(AutogenerateController), typeof(AutogenerateController))
            ])
            //.DisableNullable()
        );
    }
}