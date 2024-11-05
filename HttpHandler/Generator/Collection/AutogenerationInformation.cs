using SSHC.Generator.Collection.Methods;

namespace SSHC.Generator.Collection
{
    internal record class AutogenerationInformation(
        Type ControllerType,
        string ControllerName,
        string ControllerRoute)
    {
        public IEnumerable<AutogenerationMethodInformation> Methods = Enumerable.Empty<AutogenerationMethodInformation>();
        public string NameSpace { get; set; } = string.Empty;
        public AutogenerationResult AutogenerationResult { get; set; } = AutogenerationResult.Success;
        public string Reason { get; set; } = string.Empty;
    }
}
