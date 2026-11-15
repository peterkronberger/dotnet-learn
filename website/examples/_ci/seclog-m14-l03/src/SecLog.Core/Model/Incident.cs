namespace SecLog.Model;

/// <summary>The failed logins of one user.</summary>
/// <param name="user">The name of the user.</param>
public class Incident(string user)
{
    private static int nextNumber;
    private readonly List<LogEvent> failures = [];

    /// <summary>The number of this incident. Every new one gets the next.</summary>
    public int Number { get; } = ++nextNumber;

    /// <summary>The number of incidents created so far.</summary>
    public static int Count => nextNumber;

    /// <summary>The name of the user of this incident.</summary>
    public string User => user;

    /// <summary>Where the incident was found. Set once, at creation.</summary>
    public required string Source { get; init; }

    /// <summary>Adds a failed login of this user.</summary>
    /// <param name="logEvent">The event to add.</param>
    /// <returns>
    /// <c>true</c> if the event was added, <c>false</c> if it is not a
    /// failed login of this user.
    /// </returns>
    public bool Add(LogEvent logEvent)
    {
        bool failed = string.Equals(
            logEvent.EventType, "login-failed", StringComparison.Ordinal);
        bool sameUser = string.Equals(
            logEvent.User, user, StringComparison.Ordinal);
        if (!failed || !sameUser)
        {
            return false;
        }
        failures.Add(logEvent);
        return true;
    }

    /// <summary>The number of failed logins in this incident.</summary>
    public int FailureCount => failures.Count;

    /// <summary>
    /// The state of the incident: open, investigating or closed.
    /// Any other value throws an ArgumentException.
    /// </summary>
    public string Status
    {
        get => field;
        set
        {
            bool known =
                string.Equals(value, "open", StringComparison.Ordinal) ||
                string.Equals(value, "investigating", StringComparison.Ordinal) ||
                string.Equals(value, "closed", StringComparison.Ordinal);
            if (!known)
            {
                throw new ArgumentException($"Unknown status: {value}");
            }
            field = value;
        }
    } = "open";

    /// <summary>Describes this incident in one line.</summary>
    /// <returns>
    /// A line such as <c>Incident #1 bob: 5 failed logins (sample log)</c>.
    /// </returns>
    public string Summary()
    {
        return $"Incident #{Number} {user}: {FailureCount} failed logins ({Source})";
    }
}
