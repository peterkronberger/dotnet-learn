using SecLog.Model;

namespace SecLog.Tests.Doubles;

// A spy: it remembers every line instead of writing it
public sealed class ReportSpy : IReportWriter
{
    public List<string> Lines { get; } = [];

    public void WriteLine(string line) => Lines.Add(line);
}
