namespace SeshLib.Generators.HttpClient.Helpers
{
    internal static class TypeHelper
    {
        public static string TypeAsCodeSnippet(Type type)
        {
            if (_primitiveMatches.ContainsKey(type.Name))
            {
                return _primitiveMatches[type.Name];
            }

            if (IsEnumerable(type))
            {
                ReadOnlySpan<char> name = ReturnTypeWithoutEnumerableSuffix(type.Name);
                var s = $"{name.ToString()}<{string.Join(", ", type.GenericTypeArguments.Select(t => TypeAsCodeSnippet(t)))}>"; ;
                return s;
            }

            return type.Name;
        }

        public static bool IsEnumerable(Type type)
        {
            if (type.Name.StartsWith("IEnumerable")) //for some reason this isn't caught by the predicate below, (i.IsGenericType is false)
            {
                return true;
            }            
            else
            {
                return Array.Exists(
                    type.GetInterfaces(),
                    i => (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
            }
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
            { typeof(byte).Name, "byte" }
        };

        private static ReadOnlySpan<char> ReturnTypeWithoutEnumerableSuffix(ReadOnlySpan<char> typeSpan)
        {
            if (typeSpan.Contains('`'))
            {
                var first = typeSpan[..typeSpan.IndexOf('`')];
                return $"{first}";
            }
            else
            {
                return typeSpan;
            }
        }

    }
}
