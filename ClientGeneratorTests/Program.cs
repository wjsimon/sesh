using SSHC.Generator;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting generator...");
        Console.WriteLine("---------------------");
        var controllerAssembly = Assembly.GetAssembly(typeof(TestController));
        ApiClientGenerator.Generate(controllerAssembly, false);
        Console.WriteLine("---------------------");
        Console.WriteLine("Finished generating.");
        Console.WriteLine("---------------------");
    }
}