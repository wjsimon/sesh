using Simons.Http;
using Simons.Generators.Http;
using Simons.Generators.Http.Controllers;

internal class Program
{
    private static void Main(string[] args)
    {
        GeneratorArguments generatorArguments = 
            GeneratorArguments
            .Create(save: false)
            .AddRange([
                (typeof(TestController), typeof(ApiClient)),
                (typeof(AutogenerateController), typeof(AutogenerateController))
            ]);

        ApiClientGenerator generator = new(generatorArguments);
        generator.Generate();
    }
}