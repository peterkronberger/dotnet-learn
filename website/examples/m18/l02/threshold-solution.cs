Console.WriteLine(TrafficRating.For(9));
Console.WriteLine(TrafficRating.For(10));
Console.WriteLine(TrafficRating.For(100));

static class TrafficRating
{
    // Rate the number of log lines per minute (the limits are made up)
    public static string For(int linesPerMinute)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(linesPerMinute);

        return linesPerMinute switch
        {
            < 10 => "Quiet",
            < 100 => "Busy",
            _ => "Flooded",
        };
    }
}
