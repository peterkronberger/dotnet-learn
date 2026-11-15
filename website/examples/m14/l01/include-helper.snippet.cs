// Helper file: only types, no top-level statements.
static class FailureText
{
    public static string For(int failures)
    {
        string word = failures == 1 ? "attempt" : "attempts";
        return $"{failures} failed {word}";
    }
}
