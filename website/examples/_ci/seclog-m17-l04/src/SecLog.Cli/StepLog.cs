using System.Runtime.CompilerServices;

// Prints a message with the name of the method that called it
static class StepLog
{
    public static void Write(string message,
        [CallerMemberName] string step = "")
    {
        Console.WriteLine($"[{step}] {message}");
    }
}
