namespace SecLog.Model;

/// <summary>A rule that gets its check as a function.</summary>
public sealed class FuncRule : IDetectionRule
{
    private readonly Func<Incident, bool> check;

    /// <summary>Creates a rule from a name and a check.</summary>
    /// <param name="name">The short name of the rule.</param>
    /// <param name="check">Decides whether an incident matches.</param>
    public FuncRule(string name, Func<Incident, bool> check)
    {
        Name = name;
        this.check = check;
    }

    /// <summary>The short name of the rule.</summary>
    public string Name { get; }

    /// <summary>Matches if the check returns true.</summary>
    public bool IsMatch(Incident incident)
    {
        return check(incident);
    }
}
