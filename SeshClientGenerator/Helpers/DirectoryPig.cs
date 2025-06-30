namespace Sesh.Generators.HttpClient.Helpers
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

            //files checks if the type of the passed mapping exists as a cs file; if you pass the FastApiClientBase e.g. 
            //it won't be there if you've included this only as a library... need to find a workaround for this

            Console.WriteLine($"{root}; found {files.Count()} files");

            if (files.Length == 0) { return null; }
            return ParentDir(files[0].AsSpan()).ToString(); //saves a single string copy within the method, but hey
        }

        private static string GetRoot()
        {
            //oink oink
            var arr = Directory.GetCurrentDirectory()[..Environment.CurrentDirectory.IndexOf("bin")][..^1].Split('\\');
            string slnRoot = string.Join("\\", arr[..^1]);
            return slnRoot;
        }

        private static ReadOnlySpan<char> ParentDir(ReadOnlySpan<char> filePath)
        {
            return filePath[..filePath.LastIndexOf('\\')];
        }
    }
}