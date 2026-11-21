using System.Globalization;

namespace SecLog.Model;

/// <summary>Turns the text of log lines into events. No input or output.</summary>
public static class LogParser
{
    /// <summary>Turns the text of an event type into the enum value.</summary>
    /// <param name="text">For example <c>login-failed</c>.</param>
    /// <returns>The event type.</returns>
    /// <exception cref="ArgumentException">The text is not a known type.</exception>
    public static EventType ParseEventType(string text)
    {
        return text switch
        {
            "login-success" => EventType.LoginSuccess,
            "login-failed" => EventType.LoginFailed,
            "logout" => EventType.Logout,
            _ => throw new ArgumentException($"Unknown event type: {text}"),
        };
    }

    /// <summary>Parses a log line with a list pattern.</summary>
    /// <param name="line">One line of the log.</param>
    /// <returns>The event, or <c>null</c> for an invalid line.</returns>
    public static SecurityEvent? ParseWithPatterns(string line) =>
        line.Split(' ') switch
        {
            [var time, ("login-success" or "login-failed" or "logout") and var type,
                var userPart, _] => new SecurityEvent(
                    DateTimeOffset.Parse(time, CultureInfo.InvariantCulture),
                    ParseEventType(type),
                    userPart is ['u', 's', 'e', 'r', '=', .. var name] ? name : null),
            _ => null,
        };

    /// <summary>Parses a line into an event or an error.</summary>
    /// <param name="line">One line of the log.</param>
    /// <returns>A <see cref="SecurityEvent"/> or a <see cref="LineError"/>.</returns>
    public static ParseResult Parse(string line)
    {
        SecurityEvent? parsed = ParseWithPatterns(line);
        if (parsed is not null)
        {
            return parsed;
        }
        return new LineError(line, "not a valid log line");
    }

    /// <summary>Turns one log line into a LogEvent, or rejects it.</summary>
    /// <param name="line">One line of the log.</param>
    /// <param name="lineNumber">The number of the line (the first is 1).</param>
    /// <returns>The event of the line.</returns>
    /// <exception cref="InvalidLogLineException">The line is not valid.</exception>
    public static LogEvent ParseLine(string line, int lineNumber)
    {
        string[] parts = line.Split(' ');
        if (parts.Length != 4)
        {
            throw new InvalidLogLineException(
                lineNumber, $"expected 4 parts, found {parts.Length}");
        }
        DateTimeOffset time;
        try
        {
            time = DateTimeOffset.Parse(parts[0], CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
            throw new InvalidLogLineException(lineNumber, "invalid timestamp", ex);
        }
        string kind = parts[1];
        bool known =
            string.Equals(kind, "login-success", StringComparison.Ordinal) ||
            string.Equals(kind, "login-failed", StringComparison.Ordinal) ||
            string.Equals(kind, "logout", StringComparison.Ordinal);
        if (!known)
        {
            throw new InvalidLogLineException(
                lineNumber, $"unknown event type {kind}");
        }
        return new LogEvent(time, kind, UserOf(parts));
    }

    // The value of the first part that starts with user=, or null
    private static string? UserOf(string[] parts)
    {
        foreach (string part in parts)
        {
            if (part.StartsWith("user=", StringComparison.Ordinal))
            {
                return part["user=".Length..];
            }
        }
        return null;
    }
}
