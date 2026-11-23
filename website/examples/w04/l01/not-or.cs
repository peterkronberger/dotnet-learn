// Check three counts of failed logins
int[] failures = { 1, 2, 3 };
foreach (int count in failures)
{
    bool alert = count is not < 3 or 1;
    Console.WriteLine($"{count}: {alert}");
}
