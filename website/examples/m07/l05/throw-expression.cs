// A throw expression stands where a value is expected
Console.WriteLine(UserOrThrow("bob"));
int[] counts = [7, -1];
foreach (int failures in counts)
{
    try
    {
        Console.WriteLine(Severity(failures));
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine($"Rejected: {failures}");
    }
}
try
{
    Console.WriteLine(UserOrThrow(null));
}
catch (ArgumentNullException ex)
{
    Console.WriteLine(ex.Message);
}

// In ??: the right side throws when the left side is null
string UserOrThrow(string? user) =>
    user ?? throw new ArgumentNullException("user");

// In ?: the third operand throws; the second gives the value
string Severity(int failures) =>
    failures >= 0
        ? (failures >= 5 ? "high" : "low")
        : throw new ArgumentOutOfRangeException("failures");
