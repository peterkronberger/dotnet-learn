FailureLimit limit = new FailureLimit(4);
Console.WriteLine(limit.IsExceeded(3));
Console.WriteLine(limit.IsExceeded(5));

/// <summary>A limit for the number of failed logins.</summary>
class FailureLimit
{
    private readonly int maximum;

    /// <summary>Creates a limit.</summary>
    /// <param name="maximum">The highest number of failures that is allowed.</param>
    public FailureLimit(int maximum)
    {
        this.maximum = maximum;
    }

    /// <summary>Checks a number of failures against the limit.</summary>
    /// <param name="failures">The number of failed logins.</param>
    /// <returns><c>true</c> if the number is above the limit.</returns>
    public bool IsExceeded(int failures)
    {
        return failures > maximum;
    }
}
