using Simons.Generators.Http.Helpers;
using System.Collections.ObjectModel;

namespace Simons.Generators.Http
{
    public class GeneratorArguments
    {
        private GeneratorArguments(
            bool save,
            bool fileNameMatchesClassName,
            bool printGeneratedCode,
            bool printProgress
        )
        {
            this.Save = save;
            this.FileNameMatchesClassName = fileNameMatchesClassName;
            this.PrintGeneratedCode = printGeneratedCode;
            this.PrintProgress = printProgress;
        }

        private Dictionary<Type, string> _pathMappings { get; set; } = new(); //type = server, string = client
        private Dictionary<Type, Type> _typeMappings { get; set; } = new();
        public ReadOnlyDictionary<Type, string> PathMappings => _pathMappings.AsReadOnly();
        public ReadOnlyDictionary<Type, Type> TypeMappings => _typeMappings.AsReadOnly();

        public bool Save { get; private set; }
        public bool FileNameMatchesClassName { get; private set; }
        public bool PrintGeneratedCode { get; private set; }
        public bool PrintProgress { get; private set; }

        public static GeneratorArguments Create(
            bool save = true, 
            bool fileNameMatchesClassName = true,
            bool printGeneratedCode = false,
            bool printProgress = true)
        {
            var args = new GeneratorArguments(save, fileNameMatchesClassName, printGeneratedCode, printProgress);
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
