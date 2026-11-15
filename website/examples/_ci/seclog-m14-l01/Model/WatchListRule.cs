namespace SecLog.Model;

/// <summary>A rule that watches one user.</summary>
sealed class WatchListRule : DetectionRule
{
    /// <summary>The name of the watched user.</summary>
    public string WatchedUser { get; }

    /// <summary>Creates the rule for one user.</summary>
    /// <param name="watchedUser">The name of the watched user.</param>
    public WatchListRule(string watchedUser) : base("watch list")
    {
        WatchedUser = watchedUser;
    }

    /// <summary>Adds the watched user to the description.</summary>
    public override string Describe()
    {
        return base.Describe() + $" ({WatchedUser})";
    }

    /// <summary>Matches if the incident belongs to the watched user.</summary>
    public override bool IsMatch(Incident incident)
    {
        return string.Equals(
            incident.User, WatchedUser, StringComparison.Ordinal);
    }
}
