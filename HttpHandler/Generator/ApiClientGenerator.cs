using System.Reflection;

namespace SSHC.Generator
{
    public static class ApiClientGenerator
    {
        public static void Generate(Assembly controllerAssembly) //support multiple assemblies
        {
            //get all controllers to be generate
            //do them step-by-step
            foreach (var info in ControllerInformationCollector.Collect(controllerAssembly).Where(i => i is not null))
            {
                GenerateApiClient(info!);
            }
        }

        private static void GenerateApiClient(AutogenerationInformation generationInformation)
        {
            Console.WriteLine($"Starting to generate ApiClient for {generationInformation.ControllerName}...");
            FormattingClassGenerator generator = new();

            generator.AddNamespace($"TestSpace");
            generator.AddClass(generationInformation);
            generator.AddGetOnlyProperty(typeof(string), "ApiControllerName", generationInformation.ControllerName);
            foreach (var method in generationInformation.Methods)
            {
                generator.AddPublicMethod(method);
            }

            Console.WriteLine(generator.Generate());
            //public class TestApiClient, TestControllerApiClient via option
            //generate one method per methodinformation => 
            //  get number of parameters, at more than 3 parameters: linebreak all of them for readability (nice-to-have)
            //  public Task<returnvalue> MethodInformation.MethodName([FromQuery]param.Key1 param.Value1, [FromQuery]paramKey2 param.Value2, etc...)  
        }

        //string controllerName = GetControllerRoute(type);
        //string fileName = $"{controllerName}ApiClient.cs";






        //        var testMethods = from assembly in assemblies
        //                          from type in assembly.GetTypes()
        //                          from method in type.GetMethods()
        //                          where method.IsDefined(typeof(TestMethodAttribute))
        //                          select method;

        //foreach (var method in testMethods)
        //{
        //    Console.WriteLine(method);
        //}
    }
}
