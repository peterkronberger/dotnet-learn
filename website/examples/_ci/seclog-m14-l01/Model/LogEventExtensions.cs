namespace SecLog.Model;

/// <summary>Extension methods that ask a log event a question.</summary>
static class LogEventExtensions
{
    /// <summary>Checks whether the event is a failed login.</summary>
    /// <param name="logEvent">The event to check.</param>
    public static bool IsFailure(this LogEvent logEvent)
    {
        return logEvent.EventType == "login-failed";
    }

    /// <summary>Checks whether the event belongs to a user.</summary>
    /// <param name="logEvent">The event to check.</param>
    /// <param name="user">The name of the user.</param>
    public static bool IsFrom(this LogEvent logEvent, string user)
    {
        return logEvent.User == user;
    }
}
