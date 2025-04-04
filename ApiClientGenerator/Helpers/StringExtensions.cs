namespace Simons.Generators.HttpClient.Helpers
{
    internal static class StringExtensions
    {
        public static string FirstToLower(this string str) 
            => $"{Char.ToLower(str[0])}{str[1..]}";
    }
}
