try
{
    Console.WriteLine(LogTools.Total("4", "x"));
}
catch (FormatException ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    foreach (string line in (ex.StackTrace ?? "").Split('\n'))
    {
        // Cut each frame at " in " so no file path and no line number remain
        int cut = line.IndexOf(" in ");
        Console.WriteLine(cut >= 0 ? line[..cut].Trim() : line.Trim());
    }
}

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
