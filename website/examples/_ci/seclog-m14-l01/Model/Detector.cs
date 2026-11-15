namespace SecLog.Model;

/// <summary>Holds exchangeable rules and counts how many match.</summary>
sealed class Detector
{
    /// <summary>Creates a detector with its rules.</summary>
    /// <param name="rules">The rules to check.</param>
    public Detector(List<IDetectionRule> rules)
    {
        Rules = rules;
    }

    /// <summary>The rules of the detector.</summary>
    public IReadOnlyList<IDetectionRule> Rules { get; }

    /// <summary>Counts the rules that match an incident.</summary>
    /// <param name="incident">The incident to check.</param>
    /// <returns>The number of matching rules.</returns>
    public int CountMatches(Incident incident)
    {
        int count = 0;
        foreach (IDetectionRule rule in Rules)
        {
            if (rule.IsMatch(incident))
            {
                count++;
            }
        }
        return count;
    }
}
