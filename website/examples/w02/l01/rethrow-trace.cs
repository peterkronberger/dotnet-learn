foreach (bool keep in new[] { true, false })
{
    try
    {
        LogTools.Outer("x", keep);
    }
    catch (FormatException ex)
    {
        Console.WriteLine(keep ? "throw;" : "throw ex;");
        foreach (string line in (ex.StackTrace ?? "").Split('\n'))
        {
            string frame = line.Trim();
            int cut = frame.IndexOf('(');
            if (frame.StartsWith("at LogTools"))
            {
                Console.WriteLine(frame[..cut]);
                break;
            }
        }
    }
}

static class LogTools
{
    public static void Inner(string text)
    {
        int.Parse(text);
    }

    public static void Outer(string text, bool keep)
    {
        try
        {
            Inner(text);
        }
        catch (FormatException ex)
        {
            FormatException same = ex;
            if (keep)
            {
                throw;
            }
            throw same;
        }
    }
}
