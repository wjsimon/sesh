using Sesh.Generators.HttpClient.Collection;

namespace Sesh.Generators.HttpClient.Tracing
{
    internal class GeneratorTrace
    {
        private readonly List<string> _trace = [];
        private readonly List<AutogenerationInformation> _genStack = [];
        private int _fileCount;
        private int _successCount;
        private IEnumerable<AutogenerationInformation>? _failedGenerations;
        private IEnumerable<AutogenerationInformation>? _skippedGenerations;

        public void Add(AutogenerationInformation info)
            => _genStack.Add(info);

        public void Add(IEnumerable<AutogenerationInformation> info)
            => _genStack.AddRange(info);

        public void Add(string trace)
            => _trace.Add(trace);

        public void AddNewLine()
            => _trace.Add("");

        public void PrintHeader()
        {
            Add($"===================================");
            Add($"======= Starting generating =======");
            Add($"===================================");
            AddNewLine();
            Flush();
        }

        public void PrintFooter()
        {
            AddNewLine();
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

        public virtual string PrintSummary()
        {
            if (_genStack.Count == 0) { return string.Empty; }
            AggregateSummary();

            AddNewLine();
            AddNewLine();
            Add($"Successfully generated {_successCount} / {_fileCount}");

            if (_skippedGenerations is not null && _skippedGenerations.Any())
            {
                Add($"{_skippedGenerations.Count()} clients skipped...");
                foreach (var skipped in _skippedGenerations)
                {
                    Add($"{skipped.ControllerRoute}: {skipped.Reason}");
                }
            }

            if (_failedGenerations is not null && _failedGenerations.Any())
            {
                Add($"{_failedGenerations.Count()} clients failed...");
                foreach (var failed in _failedGenerations)
                {
                    Add($"{failed.ControllerRoute}: {failed.Reason}");
                }
            }

            return Flush();
        }

        private void AggregateSummary()
        {
            if (_genStack.Count == 0) { return; }

            _fileCount = _genStack.Count;
            _successCount = _genStack.Where(i => i.AutogenerationResult == AutogenerationResult.Success).Count();
            _failedGenerations = _genStack.Where(i => i.AutogenerationResult == AutogenerationResult.Failure);
            _skippedGenerations = _genStack.Where(i => i.AutogenerationResult == AutogenerationResult.Skipped);
        }
    }
}
