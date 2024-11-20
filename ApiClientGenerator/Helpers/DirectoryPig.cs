namespace Simons.Generators.Http.Helpers
{
    internal static class DirectoryPig
    {
        public static string? GetFileDirectory(Type type, bool matching)
        {
            if (!matching)
            {
                throw new NotSupportedException($"Advanced class file search not yet supported. " +
                    $"Please make sure the type you use for assembly location matches its file name.");
            }

            string root = GetRoot();
            var files = Directory.GetFiles(root, $"{type.Name}.cs", SearchOption.AllDirectories);

            if (files.Length == 0) { return null; }
            return files[0];
        }

        private static string GetRoot()
        {
            //oink oink
            var arr = Directory.GetCurrentDirectory()[..Environment.CurrentDirectory.IndexOf("bin")][..^1].Split('\\');
            string slnRoot = string.Join("\\", arr[..^1]);
            return slnRoot;
        }
    }
}