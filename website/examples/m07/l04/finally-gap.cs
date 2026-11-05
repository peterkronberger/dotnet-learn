// Move "attempts++;" into a finally block behind the catch clause.
// The output stays: Attempts: 3, failed: 1
string[] inputs = ["4", "x", "6"];
int attempts = 0;
int failed = 0;
foreach (string text in inputs)
{
    try
    {
        int.Parse(text);
    }
    catch (FormatException)
    {
        failed++;
    }
    attempts++;
}
Console.WriteLine($"Attempts: {attempts}, failed: {failed}");
