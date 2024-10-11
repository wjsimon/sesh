namespace SSHC.Generator
{
    internal record class AutogenerationInformation(Type ControllerType, string ControllerName, string ControllerRoute)
    {
        public IEnumerable<AutogenerationMethodInformation> Methods = Enumerable.Empty<AutogenerationMethodInformation>();
    }
}
