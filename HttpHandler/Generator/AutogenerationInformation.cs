namespace SSHC.Generator
{
    internal record class AutogenerationInformation(string ControllerName, string ControllerRoute)
    {
        public IEnumerable<AutogenerationMethodInformation> Methods = Enumerable.Empty<AutogenerationMethodInformation>();
    }
}
