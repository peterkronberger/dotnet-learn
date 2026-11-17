/// <summary>The time of the last failed login, or <c>null</c> if none.</summary>
public DateTimeOffset? LastFailure =>
    failures.Count == 0 ? null : failures[^1].Timestamp;
