namespace SecLog.Model;

/// <summary>The contract of every detection rule.</summary>
public interface IDetectionRule
{
    /// <summary>The short name of the rule.</summary>
    string Name { get; }

    /// <summary>Decides whether an incident matches the rule.</summary>
    bool IsMatch(Incident incident);
}
