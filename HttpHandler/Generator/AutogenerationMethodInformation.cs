namespace SSHC.Generator
{
    internal record AutogenerationMethodInformation(
        string MethodName, 
        Dictionary<Type, string> ParametersMetaData, 
        Type ReturnType);
}
