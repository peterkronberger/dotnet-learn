// Program.cs before: the parser is a set of local functions
// Turn the text of a log line into an event type: unknown text is an error
EventType ParseEventType(string text)
{
    return text switch
    {
        "login-success" => EventType.LoginSuccess,
        "login-failed" => EventType.LoginFailed,
        "logout" => EventType.Logout,
        _ => throw new ArgumentException($"Unknown event type: {text}"),
    };
}

// Parse a log line with a list pattern: null for an invalid line
SecurityEvent? ParseWithPatterns(string line) =>
    line.Split(' ') switch
    {
        [var time, ("login-success" or "login-failed" or "logout") and var type,
            var userPart, _] => new SecurityEvent(
                DateTimeOffset.Parse(time, CultureInfo.InvariantCulture),
                ParseEventType(type),
                userPart is ['u', 's', 'e', 'r', '=', .. var name] ? name : null),
        _ => null,
    };

// Parse a line: a SecurityEvent, or a LineError that says why not
ParseResult Parse(string line)
{
    SecurityEvent? parsed = ParseWithPatterns(line);
    if (parsed is not null)
    {
        return parsed;
    }
    return new LineError(line, "not a valid log line");
}
