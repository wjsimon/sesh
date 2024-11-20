namespace Simons.Generators.ApiClient.Helpers
{
    internal class PrimitiveHelper
    {

        public static string SwapPrimitive(Type type)
        {
            var name = type.Name;
            if (_primitiveMatches.ContainsKey(name))
            {
                return _primitiveMatches[type.Name];
            }

            return type.Name;
        }


        private static readonly Dictionary<string, string> _primitiveMatches = new Dictionary<string, string>()
        {
            { typeof(string).Name, "string" },
            { typeof(bool).Name, "bool" },
            { typeof(int).Name, "int" },
            { typeof(uint).Name, "uint" },
            { typeof(long).Name, "long" },
            { typeof(float).Name, "float" },
            { typeof(double).Name, "double" },
            { typeof(void).Name, "void" },
            { typeof(object).Name, "object" },
        };
    }
}
