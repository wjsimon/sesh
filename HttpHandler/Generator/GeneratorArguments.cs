using System.Collections.ObjectModel;
using System.Reflection;

namespace SSHC.Generator
{
    public class GeneratorArguments
    {
        private GeneratorArguments() { }
        //type is the type of the controller (server), and the string is the location where the new file will be written (client)
        private Dictionary<Type, string> _fileMappings { get; set; } = new();

        public ReadOnlyDictionary<Type, string> FileMappings => _fileMappings.AsReadOnly();
        public bool Save { get; set; } = true;
        public bool FileNameMatchesClassName = true;

        public static GeneratorArguments Create()
        {
            var args = new GeneratorArguments();
            return args;
        }

        public GeneratorArguments AddRange(IEnumerable<(Type ctlr, Type target)> typePairings)
        {
            foreach(var pair in typePairings) { Add(pair.ctlr, pair.target); }   
            return this;
        }

        public GeneratorArguments Add(Type controllerType, Type targetAssemblyType)
            => Add(controllerType, GetLocation(targetAssemblyType));

        public GeneratorArguments Add(Type controllerType, Assembly targetAssembly)
            => Add(controllerType, GetLocation(targetAssembly));         

        private GeneratorArguments Add(Type type, string location)
        {
            _fileMappings[type] = location;
            return this;
        }

        private static string GetLocation(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            if (assembly is null)
            {
                throw new ArgumentException(
                    $"Assembly for given type {type.FullName} could not be resolved");
            }

            return GetLocation(assembly);
        }

        private static string GetLocation(Assembly assembly)
            => assembly.Location;
    }
}
