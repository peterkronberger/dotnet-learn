namespace SecLog.Model;

/// <summary>A log line that cannot be turned into a LogEvent.</summary>
class InvalidLogLineException : Exception
{
    /// <summary>The number of the rejected line (the first line is 1).</summary>
    public int LineNumber { get; }

    /// <summary>Creates the exception for a rejected line.</summary>
    /// <param name="lineNumber">The number of the line.</param>
    /// <param name="message">The broken rule.</param>
    public InvalidLogLineException(int lineNumber, string message)
        : base(message)
    {
        LineNumber = lineNumber;
    }

    /// <summary>Creates the exception with the error that caused it.</summary>
    /// <param name="lineNumber">The number of the line.</param>
    /// <param name="message">The broken rule.</param>
    /// <param name="innerException">The error that caused this one.</param>
    public InvalidLogLineException(
        int lineNumber, string message, Exception innerException)
        : base(message, innerException)
    {
        LineNumber = lineNumber;
    }
}
