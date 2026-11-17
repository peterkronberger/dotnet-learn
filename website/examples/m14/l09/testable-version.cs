// The same rule as a method: values in, value out
string[] types = ["login-failed", "login-failed", "login-failed"];

int failures = CountFailures(types);
bool alarm = IsAlarm(failures, threshold: 3);

Console.WriteLine($"Failures: {failures}");
Console.WriteLine($"Alarm: {alarm}");
Console.WriteLine($"Alarm with threshold 5: {IsAlarm(failures, 5)}");

static int CountFailures(IEnumerable<string> eventTypes)
{
    int count = 0;
    foreach (string eventType in eventTypes)
    {
        if (eventType == "login-failed")
        {
            count++;
        }
    }
    return count;
}

static bool IsAlarm(int failures, int threshold)
{
    return failures >= threshold;
}
