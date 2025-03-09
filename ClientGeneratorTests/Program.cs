using Microsoft.AspNetCore.WebUtilities;
using Simons.Clients.Http;
using Simons.Generators.HttpClient;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;

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
                (typeof(TestController), typeof(FastApiClientBase)),
                //(typeof(AutogenerateController), typeof(AutogenerateController))
            ])
            .AllowNullReturns()
        );
    }
}