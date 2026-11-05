Console.WriteLine(LogTools.Total("4", "x"));

static class LogTools
{
    public static int ParseCount(string text)
    {
        return int.Parse(text);
    }

    public static int Total(string first, string second)
    {
        return ParseCount(first) + ParseCount(second);
    }
}
