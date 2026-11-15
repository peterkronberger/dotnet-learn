namespace SecLog.Model;

/// <summary>A period of time that starts at a given moment.</summary>
readonly struct TimeWindow
{
    /// <summary>Creates a window from its start and its length.</summary>
    /// <param name="start">The first moment of the window.</param>
    /// <param name="length">How long the window lasts.</param>
    public TimeWindow(DateTimeOffset start, TimeSpan length)
    {
        Start = start;
        Length = length;
    }

    /// <summary>The first moment of the window.</summary>
    public DateTimeOffset Start { get; }

    /// <summary>How long the window lasts.</summary>
    public TimeSpan Length { get; }

    /// <summary>The last moment of the window.</summary>
    public DateTimeOffset End => Start + Length;

    /// <summary>Checks if a moment lies in the window.</summary>
    /// <param name="time">The moment to check.</param>
    public bool Contains(DateTimeOffset time)
    {
        return time >= Start && time <= End;
    }
}
