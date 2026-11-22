namespace SecLog.Model;

/// <summary>A rule that matches incidents under investigation.</summary>
public sealed class StatusRule : IDetectionRule
{
    /// <summary>The short name of the rule.</summary>
    public string Name => "status investigating";

    /// <summary>Matches if the incident is under investigation.</summary>
    public bool IsMatch(Incident incident)
    {
        return string.Equals(
            incident.Status, "investigating", StringComparison.Ordinal);
    }
}
