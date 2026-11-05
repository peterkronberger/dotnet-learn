// "throw;" keeps the stack trace, "throw ex;" restarts it
bool[] modes = [true, false];
foreach (bool keep in modes)
{
    try
    {
        LogTools.Outer("x", keep);
    }
    catch (FormatException ex)
    {
        Console.WriteLine(keep ? "With throw;" : "With throw ex;");
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
            Console.WriteLine("Outer saw the error");
            FormatException same = ex;
            if (keep)
            {
                throw;
            }
            throw same;
        }
    }
}
