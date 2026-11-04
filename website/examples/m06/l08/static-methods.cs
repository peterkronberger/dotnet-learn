int larger = Math.Max(3, 5);
int parsed = int.Parse("42");
Console.WriteLine($"{larger} {parsed}");
Console.WriteLine(Rating.Describe(0));
Console.WriteLine(Rating.Describe(4));

static class Rating
{
    public static string Describe(int failures)
    {
        return failures == 0 ? "quiet" : "check the log";
    }
}
