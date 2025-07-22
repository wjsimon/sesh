using SeshLib.Generators.HttpClient.Helpers;
using System.Collections.ObjectModel;

namespace SeshLib.Generators.HttpClient
{
    public class GeneratorArguments
    {
        private GeneratorArguments(
            bool save,
            bool fileNameMatchesClassName,
            bool printGeneratedCode,
            bool printProgress,
            bool verbose,
            string? outputDir
        )
        {
            this.Save = save;
            this.FileNameMatchesClassName = fileNameMatchesClassName;
            this.PrintGeneratedCode = printGeneratedCode;
            this.PrintProgress = printProgress;
            this.Verbose = verbose;

            if (outputDir != null)
            {
                this.OutputDir = Path.GetFullPath(outputDir.TrimEnd('\\').Trim()); //trim trailing slashes; copying folder paths from VS includes them but we don't want them for the filepath generation
            }
        }

        private Dictionary<Type, string> _pathMappings { get; set; } = []; //type = server, string = client
        private Dictionary<Type, Type> _typeMappings { get; set; } = []; ////type = server, type = client
        private HashSet<Type>? _selectedPartials;
        private Dictionary<Type, string>? _partialInsets;

        internal ReadOnlyDictionary<Type, string> PathMappings => _pathMappings.AsReadOnly();
        internal ReadOnlyDictionary<Type, Type> TypeMappings => _typeMappings.AsReadOnly();

        public bool Save { get; private set; }
        public bool FileNameMatchesClassName { get; private set; }
        public bool PrintGeneratedCode { get; private set; }
        public bool PrintProgress { get; private set; }
        public bool Verbose { get; private set; }
        public string? OutputDir { get; private set; }
        public bool GeneratePartials { get; private set; }
        public bool AreNullReturnsAllowed { get; private set; }

        public static GeneratorArguments Create(
            bool save = true, 
            bool fileNameMatchesClassName = true,
            bool printGeneratedCode = false,
            bool printProgress = true,
            bool verbose = false,
            string? outputDir = null)
        {
            var args = new GeneratorArguments(save, fileNameMatchesClassName, printGeneratedCode, printProgress, verbose, outputDir);
            return args;
        }

        public GeneratorArguments AddRange(IEnumerable<(Type ctlr, Type target)> typePairings)
        {
            foreach(var pair in typePairings) { Add(pair.ctlr, pair.target); }   
            return this;
        }

        public GeneratorArguments AddRange(IEnumerable<Type> types)
        {
            if (this.OutputDir == null) { throw new InvalidOperationException("OutputDir needs to be set when using simple types"); }
            foreach (var type in types)
            {
                Add(type);
            }

            return this;
        }

        public GeneratorArguments Add(Type controllerType, Type targetAssemblyType)
        {
            string? location = this.OutputDir != null ? this.OutputDir :
                GetLocation(targetAssemblyType, this.FileNameMatchesClassName);

            Console.WriteLine($"location: {location}");
            if (!string.IsNullOrEmpty(location)) 
            {
                Add(controllerType, location);
                _typeMappings.TryAdd(controllerType, targetAssemblyType);
            }

            return this;
        }

        public GeneratorArguments Add(Type controllerType)
            => Add(controllerType, this.OutputDir!);


        public GeneratorArguments WithPartials() 
            => WithPartials(null);

        public GeneratorArguments WithPartials(params IEnumerable<Type>? controllers)
        {
            bool selective = false;
            if (controllers is not null && controllers.Any()) { selective = true; }

            if (selective)
            {
                _selectedPartials = [..controllers];
            }

            //swap this with schemes per controller
            _partialInsets = _selectedPartials?.ToDictionary(p => p, p => SeshClientGenerator.DEFAULT_INSET);

            this.GeneratePartials = true;
            return this;
        }

        public GeneratorArguments AllowNullReturns()
        {
            AreNullReturnsAllowed = true;
            return this;
        }

        public bool IsPartial(Type controllerType)
        {
            if (_selectedPartials is null) { return true; }
            else if (!_selectedPartials.Any()) { return true; }
            else { return _selectedPartials.Contains(controllerType); }
        }

        public string GetInset(Type partial)
        {
            string? inset = null;
            _partialInsets?.TryGetValue(partial, out inset);

            return inset ?? SeshClientGenerator.DEFAULT_INSET;
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
