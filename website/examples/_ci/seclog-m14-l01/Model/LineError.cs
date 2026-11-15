namespace SecLog.Model;

/// <summary>A line that could not be parsed, and the reason.</summary>
/// <param name="Line">The text of the line.</param>
/// <param name="Reason">Why the line is not valid.</param>
sealed record LineError(string Line, string Reason);
