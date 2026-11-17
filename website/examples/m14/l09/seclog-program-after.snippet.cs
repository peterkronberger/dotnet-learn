// Program.cs: the three local functions are gone, the calls use LogParser
Console.WriteLine(Describe(LogParser.Parse(sampleLog[1])));
Console.WriteLine(Describe(LogParser.Parse(brokenLines[0])));
Console.WriteLine(Describe(LogParser.Parse(brokenLines[1])));

// Convert a LogEvent into the compact record form
SecurityEvent ToSecurityEvent(LogEvent logEvent)
{
    EventType type = LogParser.ParseEventType(logEvent.EventType);
    return new SecurityEvent(logEvent.Timestamp, type, logEvent.User);
}
