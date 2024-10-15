namespace SSHC.Generator
{
    internal class GeneratorTrace
    {
        private List<string> _trace = new();
        private List<AutogenerationInformation> _genStack = new();
        private int _fileCount;

        public void Add(AutogenerationInformation info)
            => _genStack.Add(info);
        
        public void Add(IEnumerable<AutogenerationInformation> info)
            => _genStack.AddRange(info);
        
        public void Add(string trace)
            => _trace.Add(trace);
        
        public void PrintHeader()
        {
            Add($"===================================");
            Add($"======= Starting generating =======");
            Add($"===================================");
            Add("");
            Flush();
        }

        public void PrintFooter()
        {
            Add("");
            Add($"===================================");
            Add($"========= Done generating =========");
            Add($"===================================");
            Flush();
        }

        public IReadOnlyList<string> Get()
            => _trace.AsReadOnly();

        public override string ToString()
            => string.Join("\r\n", _trace);

        public virtual string Flush()
        {
            string trace = ToString();
            _trace.Clear();

            Console.Write(trace + "\r\n");
            return trace;
        }

        public virtual string PrintResult()
        {
            if (!_genStack.Any()) { return string.Empty; }

            return string.Empty;
        }
    }
}
