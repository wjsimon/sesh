using Simons.Generators.ApiClient.Collection.Methods;

namespace Simons.Generators.ApiClient.Collection
{
    internal record class AutogenerationInformation(
        Type ControllerType,
        string ControllerName,
        string ControllerRoute)
    {
        public IEnumerable<AutogenerationMethodInformation> Methods = Enumerable.Empty<AutogenerationMethodInformation>();
        public string NameSpace { get; set; } = string.Empty;
        public bool GenerateAsPartial { get; set; } = false;
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
                var partials = args.GetPartials();
                if (!partials.Any() || partials.Contains(ControllerType))
                {
                    GenerateAsPartial = true;
                }
            }
        }
    }
}
