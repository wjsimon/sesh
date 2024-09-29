namespace SSHC.Generator
{
    internal record class AutogenerationInformation(
        string ControllerName, 
        string ControllerRoute, 
        List<AutogenerationMethodInformation> Methods);
}
