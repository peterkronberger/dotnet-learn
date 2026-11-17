// The method reads the real clock itself
Console.WriteLine($"5 minutes ago: {IsRecent(DateTimeOffset.UtcNow.AddMinutes(-5))}");
Console.WriteLine($"15 minutes ago: {IsRecent(DateTimeOffset.UtcNow.AddMinutes(-15))}");

static bool IsRecent(DateTimeOffset time)
{
    return DateTimeOffset.UtcNow - time <= TimeSpan.FromMinutes(10);
}
