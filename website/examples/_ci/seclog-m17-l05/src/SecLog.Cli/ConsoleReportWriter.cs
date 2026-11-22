using SecLog.Model;

// Writes the lines of a report to the console
sealed class ConsoleReportWriter : IReportWriter
{
    public void WriteLine(string line) => Console.WriteLine(line);
}
