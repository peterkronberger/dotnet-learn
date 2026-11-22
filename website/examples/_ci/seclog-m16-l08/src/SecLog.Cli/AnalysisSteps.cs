// The steps of the analysis: each one reports itself through StepLog
static class AnalysisSteps
{
    public static int CountFailures(string[] lines)
    {
        int count = lines.Count(line => line.Contains("login-failed"));
        StepLog.Write($"{count} failed logins in {lines.Length} lines");
        return count;
    }

    public static int CountUsers(string[] lines)
    {
        int count = lines
            .Select(line => line.Split("user=")[1].Split(' ')[0])
            .Distinct()
            .Count();
        StepLog.Write($"{count} users appear in the log");
        return count;
    }
}
