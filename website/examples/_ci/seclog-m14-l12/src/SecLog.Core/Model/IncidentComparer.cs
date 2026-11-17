namespace SecLog.Model;

/// <summary>Orders incidents: most failed logins first, then by user.</summary>
public sealed class IncidentComparer : IComparer<Incident>
{
    /// <summary>Compares two incidents for sorting.</summary>
    /// <param name="x">The first incident.</param>
    /// <param name="y">The second incident.</param>
    /// <returns>
    /// A negative number if <paramref name="x"/> comes first, zero if
    /// both have the same place, a positive number if it comes last.
    /// </returns>
    public int Compare(Incident? x, Incident? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }
        if (x is null)
        {
            return -1;
        }
        if (y is null)
        {
            return 1;
        }
        int byFailures = y.FailureCount.CompareTo(x.FailureCount);
        if (byFailures != 0)
        {
            return byFailures;
        }
        return string.CompareOrdinal(x.User, y.User);
    }
}
