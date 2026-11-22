using SecLog.Model;

// Creates log source readers; the container gives it the writer
sealed class LogReaderFactory(IReportWriter writer)
{
    public LogSourceReader Create(string name, string[] lines) =>
        new(name, lines, writer);
}
