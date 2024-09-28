namespace SSHC.Generator
{
    internal record class AutogenerationInformation
    {
        public string ControllerName { get; set; } //needed for ApiClient.ApiControllerName
        public string ControllerRoute { get; set; }
        public List<AutogenerationMethodInformation> Methods { get; set; } = new();
    }
}
