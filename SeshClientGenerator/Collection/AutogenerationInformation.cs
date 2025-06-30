using Sesh.Generators.HttpClient.Collection.Methods;

namespace Sesh.Generators.HttpClient.Collection
{
    internal record class AutogenerationInformation(
        Type ControllerType,
        string ControllerName,
        string ControllerRoute)
    {
        public IEnumerable<AutogenerationMethodInformation> Methods = Enumerable.Empty<AutogenerationMethodInformation>();
        public string NameSpace { get; set; } = string.Empty;
        public bool GenerateAsPartial { get; set; } = false;
        public bool AreNullReturnsAllowed { get; set; } = false;
        public string? PartialInset {  get; set; }
        public AutogenerationResult AutogenerationResult { get; set; } = AutogenerationResult.Success;
        public string Reason { get; set; } = string.Empty;

        public void Apply(GeneratorArguments args)
        {
            ApplyPartialGeneration(args);
        }

        private void ApplyPartialGeneration(GeneratorArguments args)
        {
            if (args.GeneratePartials)
            {
                if (args.IsPartial(ControllerType))
                {
                    GenerateAsPartial = true;
                    PartialInset = args.GetInset(ControllerType);
                }
            }
        }
    }
}
