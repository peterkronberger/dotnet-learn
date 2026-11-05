// Change the program so that it prints only the FIRST frame of LogTools.
// Hint: a loop can be left early with break.
try
{
    Console.WriteLine(LogTools.Total("4", "x"));
}
catch (FormatException ex)
{
    foreach (string line in (ex.StackTrace ?? "").Split('\n'))
    {
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
