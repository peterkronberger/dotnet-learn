try
{
    Console.WriteLine(LogTools.Total("4", "x"));
}
catch (FormatException ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    foreach (string line in (ex.StackTrace ?? "").Split('\n'))
    {
        // Show only the frames of our own class LogTools
        string frame = line.Trim();
        int cut = frame.IndexOf(" in ");
        if (frame.StartsWith("at LogTools"))
        {
            Console.WriteLine(cut >= 0 ? frame[..cut] : frame);
        }
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
