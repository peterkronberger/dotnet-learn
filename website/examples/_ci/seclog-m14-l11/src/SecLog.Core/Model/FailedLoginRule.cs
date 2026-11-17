namespace SecLog.Model;

/// <summary>A rule that counts failed logins against a threshold.</summary>
public sealed class FailedLoginRule : DetectionRule
{
    /// <summary>The number of failed logins that triggers the rule.</summary>
    public int Threshold { get; }

    /// <summary>Creates the rule with its threshold.</summary>
    /// <param name="threshold">The number of failed logins.</param>
    public FailedLoginRule(int threshold) : base("failed logins")
    {
        Threshold = threshold;
    }

    /// <summary>Adds the threshold to the description.</summary>
    public override string Describe()
    {
        return base.Describe() + $" (threshold {Threshold})";
    }

    /// <summary>Matches if the incident has enough failed logins.</summary>
    public override bool IsMatch(Incident incident)
    {
        return incident.FailureCount >= Threshold;
    }
}
