using Microsoft.Extensions.Logging;

namespace SecLog.Model;

/// <summary>Parses log lines and logs the lines it rejects.</summary>
public sealed class LineValidator
{
    private readonly ILogger<LineValidator> logger;

    /// <summary>Creates a validator.</summary>
    /// <param name="logger">Receives a message for every rejected line.</param>
    public LineValidator(ILogger<LineValidator> logger)
    {
        this.logger = logger;
    }

    /// <summary>Parses a line and logs it when it is rejected.</summary>
    /// <param name="line">One line of the log.</param>
    /// <param name="lineNumber">The number of the line (the first is 1).</param>
    /// <returns>The event, or <c>null</c> for a rejected line.</returns>
    public LogEvent? TryParse(string line, int lineNumber)
    {
        try
        {
            LogEvent parsed = LogParser.ParseLine(line, lineNumber);
            logger.LogDebug("Line {Number} accepted", lineNumber);
            return parsed;
        }
        catch (InvalidLogLineException ex)
        {
            logger.LogInformation("Line {Number} rejected: {Reason}",
                lineNumber, ex.Message);
            return null;
        }
    }
}
