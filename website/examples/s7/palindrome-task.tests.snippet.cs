// Test code for palindrome-task.cs (spike S7, ADR 0006). Compiled together with the learner code.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Check("Level is a palindrome", true, "Level", true, "Compare the text with its reversed form."),
        Check("Docs is not a palindrome", true, "Docs", false, "Return false when the characters differ."),
        Check("Ignores case", false, "RaceCar", true, "Upper and lower case must count as equal (ToLowerInvariant)."),
        Check("Empty text", false, "", true, "An empty text reads the same in both directions."),
    ];

    private static (string, bool, bool, string) Check(string name, bool visible, string input, bool expected, string hint)
    {
        try
        {
            var actual = Palindrome.IsPalindrome(input);
            return (name, visible, actual == expected, actual == expected ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
