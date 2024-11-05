using SSHC.Client;
using SSHC.Controllers;
using SSHC.Generator;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 1 && (args[0] == "-i" || args[0] == "--interactive"))
        {
            //launch interactive console app
        }
        else
        {
            //print error, print info
        }

        GeneratorArguments generatorArguments = GeneratorArguments
            .Create(save: false)
            .AddRange(
                [
                    (typeof(TestController), typeof(ApiClient)),
                    (typeof(AutogenerateController), typeof(AutogenerateController))
                ]
            );

        ApiClientGenerator generator = new(generatorArguments);
        generator.Generate();
    }
}