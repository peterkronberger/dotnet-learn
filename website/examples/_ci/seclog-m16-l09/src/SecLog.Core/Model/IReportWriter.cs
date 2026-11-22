namespace SecLog.Model;

/// <summary>The contract for writing one line of a report.</summary>
public interface IReportWriter
{
    /// <summary>Writes one line.</summary>
    /// <param name="line">The text of the line.</param>
    void WriteLine(string line);
}
