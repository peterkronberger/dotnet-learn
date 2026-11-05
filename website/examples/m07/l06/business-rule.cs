// A business rule is checked in one place; the callers only react
try
{
    Console.WriteLine(Incidents.Open("bob", 5));
    Console.WriteLine(Incidents.Open("alice", 1));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Rule broken: {ex.Message}");
}

try
{
    Console.WriteLine(Incidents.Escalate("dave", 1));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Rule broken: {ex.Message}");
}

static class Incidents
{
    // The rule: an incident needs at least 3 failed logins
    private static void CheckFailures(int failures)
    {
        if (failures < 3)
        {
            throw new InvalidOperationException(
                $"An incident needs at least 3 failures, found {failures}.");
        }
    }

    public static string Open(string user, int failures)
    {
        CheckFailures(failures);
        return $"Incident opened for {user}";
    }

    public static string Escalate(string user, int failures)
    {
        CheckFailures(failures);
        return $"Incident escalated for {user}";
    }
}
