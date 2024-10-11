namespace SSHC.Generator
{
    internal class GeneratorTrace
    {
        private List<string> _trace = new();

        public void Add(string trace)
        {
            _trace.Add(trace);
        }

        public IReadOnlyList<string> Get()
            => _trace.AsReadOnly();

        public override string ToString()
            => string.Join("\r\n", _trace);

    }
}
