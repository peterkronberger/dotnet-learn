// The clock is a parameter: the program passes the system clock
TimeProvider system = TimeProvider.System;
DateTimeOffset fiveMinutesAgo = system.GetUtcNow().AddMinutes(-5);
Console.WriteLine($"System clock, 5 minutes ago: {IsRecent(fiveMinutesAgo, system)}");

// A test passes a clock that always shows the same moment
var now = new DateTimeOffset(2026, 3, 2, 8, 16, 48, TimeSpan.Zero);
var fixedClock = new FixedTimeProvider(now);
var failure = new DateTimeOffset(2026, 3, 2, 8, 6, 48, TimeSpan.Zero);
Console.WriteLine($"Exactly 10 minutes ago: {IsRecent(failure, fixedClock)}");
Console.WriteLine($"One second more: {IsRecent(failure.AddSeconds(-1), fixedClock)}");

static bool IsRecent(DateTimeOffset time, TimeProvider clock)
{
    return clock.GetUtcNow() - time <= TimeSpan.FromMinutes(10);
}

class FixedTimeProvider(DateTimeOffset moment) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => moment;
}
