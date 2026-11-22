namespace SecLog.Model;

/// <summary>Rates how serious a number of failed logins is.</summary>
public static class SeverityRating
{
    /// <summary>The severity for a number of failed logins.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The number is negative.</exception>
    public static Severity For(int failureCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(failureCount);

        return failureCount switch
        {
            0 => Severity.None,
            < 3 => Severity.Low,
            < 5 => Severity.Medium,
            _ => Severity.High,
        };
    }
}
