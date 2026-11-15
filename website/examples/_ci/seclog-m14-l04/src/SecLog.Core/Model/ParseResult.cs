namespace SecLog.Model;

/// <summary>The result of parsing: an event or an error.</summary>
public union ParseResult(SecurityEvent, LineError);
