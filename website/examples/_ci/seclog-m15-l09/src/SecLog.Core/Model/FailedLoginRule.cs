using Microsoft.Extensions.Options;

namespace SecLog.Model;

/// <summary>A rule that counts failed logins against a threshold.</summary>
public sealed class FailedLoginRule : DetectionRule
{
    /// <summary>The number of failed logins that triggers the rule.</summary>
    public int Threshold { get; }

    /// <summary>Creates the rule with its threshold from the options.</summary>
    /// <param name="options">The settings of the rule.</param>
    public FailedLoginRule(IOptions<DetectionOptions> options) : base("failed logins")
    {
        Threshold = options.Value.MaxFailures;
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
