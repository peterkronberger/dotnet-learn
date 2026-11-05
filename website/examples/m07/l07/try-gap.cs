// Write TryParsePercent: true and the number for "0" to "100",
// otherwise false and 0. Do not throw and do not catch.
// Wanted output: 85%, Not a percent: 140, Not a percent: high
string[] texts = ["85", "140", "high"];
foreach (string text in texts)
{
    if (TryParsePercent(text, out int percent))
    {
        Console.WriteLine($"{percent}%");
    }
    else
    {
        Console.WriteLine($"Not a percent: {text}");
    }
}

bool TryParsePercent(string text, out int percent)
{
    // Parse the text with int.TryParse
    // Check the range 0 to 100
    // Return the result
    percent = 0;
    return false;
}
