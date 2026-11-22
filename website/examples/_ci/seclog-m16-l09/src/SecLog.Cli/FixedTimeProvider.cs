// A clock that always shows the same moment, so the program output is stable
sealed class FixedTimeProvider(DateTimeOffset moment) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => moment;
}
