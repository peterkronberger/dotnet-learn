try
{
    // The caller passes 0 events, although Share cannot divide by 0
    Console.WriteLine(Report.Summary(3, 0));
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    foreach (string line in (ex.StackTrace ?? "").Split('\n'))
    {
        // Show only the frames of our own class Report
        string frame = line.Trim();
        int cut = frame.IndexOf(" in ");
        if (frame.StartsWith("at Report"))
        {
            Console.WriteLine(cut >= 0 ? frame[..cut] : frame);
        }
    }
}

static class Report
{
    public static int Share(int failures, int events)
    {
        return failures * 100 / events;
    }

    public static string Summary(int failures, int events)
    {
        return $"Share: {Share(failures, events)} %";
    }
}
