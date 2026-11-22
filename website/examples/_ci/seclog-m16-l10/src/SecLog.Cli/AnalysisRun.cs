// One analysis run (scoped): it has its own state and lives as long as its scope
sealed class AnalysisRun(
    FailureMatcher matcher, FailureCounter counter, TimeProvider clock)
{
    public DateTimeOffset StartedAt { get; } = clock.GetUtcNow();

    public int Failures { get; private set; }

    public void Analyze(string[] lines)
    {
        int found = lines.Count(matcher.IsFailure);
        Failures += found;
        counter.Add(found);
    }
}
