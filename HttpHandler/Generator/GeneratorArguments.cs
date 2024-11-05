using SSHC.Generator.Helpers;
using System.Collections.ObjectModel;

namespace SSHC.Generator
{
    public class GeneratorArguments
    {
        private GeneratorArguments(
            bool save,
            bool fileNameMatchesClassName,
            bool printGeneratedCode
        ) 
        {
            this.Save = save;
            this.FileNameMatchesClassName = fileNameMatchesClassName;
            this.PrintGeneratedCode = printGeneratedCode;
        }

        private Dictionary<Type, string> _pathMappings { get; set; } = new(); //type = server, string = client
        private Dictionary<Type, Type> _typeMappings { get; set; } = new();
        public ReadOnlyDictionary<Type, string> PathMappings => _pathMappings.AsReadOnly();
        public ReadOnlyDictionary<Type, Type> TypeMappings => _typeMappings.AsReadOnly();

        public bool Save { get; set; } = true;
        public bool FileNameMatchesClassName = true;
        public bool PrintProgress = true;
        public bool PrintGeneratedCode = false;

        public static GeneratorArguments Create(
            bool save = true, 
            bool fileNameMatchesClassName = true,
            bool printGeneratedCode = false)
        {
            var args = new GeneratorArguments(save, fileNameMatchesClassName, printGeneratedCode);
            return args;
        }

        public GeneratorArguments AddRange(IEnumerable<(Type ctlr, Type target)> typePairings)
        {
            foreach(var pair in typePairings) { Add(pair.ctlr, pair.target); }   
            return this;
        }

        public GeneratorArguments Add(Type controllerType, Type targetAssemblyType)
        {
            string? location = GetLocation(targetAssemblyType, this.FileNameMatchesClassName);
            if (!string.IsNullOrEmpty(location)) 
            {
                Add(controllerType, location);
                _typeMappings.TryAdd(controllerType, targetAssemblyType);
            }

            return this;
        }

        private GeneratorArguments Add(Type type, string location)
        {
            if (location is null)
            {
                throw new ArgumentNullException(nameof(location)); //compiler can't handle "ThrowIfNull()" yet
            }
            
            _pathMappings[type] = location;
            return this;
        }

        private static string? GetLocation(Type type, bool fileNameMatchesClassName)
            => DirectoryPig.GetFileDirectory(type, fileNameMatchesClassName); //oink oink
    }
}
