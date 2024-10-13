namespace SSHC.Generator
{
    internal class GeneratorTrace
    {
        private List<string> _trace = new();

        public void Add(string trace)
        {
            _trace.Add(trace);
        }

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
    }
}
