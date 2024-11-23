using Simons.Clients.Http;
using Simons.Generators.HttpClient;
using Simons.Generators.HttpClient.Controllers;
using System.Collections.Generic;

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
            //.AllowNullReturns()
        );

        List<string> list= new List<string>();
    }
}