using SSHC.Client;
using SSHC.Controllers;
using SSHC.Generator;
using System.Reflection;

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


        //command line arguments for conversion => if no arguments are given, print stuff
        //command line argument for interactive mode also, which allows generation in the terminal as a simple command line app
        //Console.WriteLine("---------------------");
        //Console.WriteLine("Starting generator...");
        //Console.WriteLine("---------------------");

        var controllerAssembly = Assembly.GetAssembly(typeof(TestController));
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

        //Console.WriteLine("---------------------");
        //Console.WriteLine("Finished generating.");
        //Console.WriteLine("---------------------");
    }
}