// Task 1: reject a negative number with a guard clause
// Task 2: return "Quiet" for up to 9, "Busy" for 10 to 99, else "Flooded"
// Task 3: the checks below must print Busy for 10 and Flooded for 100
Console.WriteLine(TrafficRating.For(9));
Console.WriteLine(TrafficRating.For(10));
Console.WriteLine(TrafficRating.For(100));

static class TrafficRating
{
    // Rate the number of log lines per minute (the limits are made up)
    public static string For(int linesPerMinute)
    {
        return "Quiet";
    }
}
