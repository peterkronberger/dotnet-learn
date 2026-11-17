// Three failed logins in a row
FailureCounter.Record("login-failed");
FailureCounter.Record("login-failed");
FailureCounter.Record("login-failed");

static class FailureCounter
{
    private static int failures;

    public static void Record(string eventType)
    {
        if (eventType == "login-failed")
        {
            failures++;
        }
        if (failures >= 3)
        {
            Console.WriteLine("ALARM: too many failed logins");
        }
    }
}
