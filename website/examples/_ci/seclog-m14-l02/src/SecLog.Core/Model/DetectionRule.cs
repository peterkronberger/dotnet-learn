namespace SecLog.Model;

/// <summary>A rule that decides whether an incident needs attention.</summary>
public abstract class DetectionRule : IDetectionRule
{
    /// <summary>The short name of the rule.</summary>
    public string Name { get; }

    /// <summary>Creates a rule with a name.</summary>
    /// <param name="name">The short name of the rule.</param>
    protected DetectionRule(string name)
    {
        Name = name;
    }

    /// <summary>Describes the rule in one line.</summary>
    /// <returns>A line such as <c>Rule: failed logins</c>.</returns>
    public virtual string Describe()
    {
        return $"Rule: {Name}";
    }

    /// <summary>The text of a rule is its description.</summary>
    public override string ToString()
    {
        return Describe();
    }

    /// <summary>Decides whether an incident matches the rule.</summary>
    /// <param name="incident">The incident to check.</param>
    /// <returns><c>true</c> if the rule matches the incident.</returns>
    public abstract bool IsMatch(Incident incident);
}
