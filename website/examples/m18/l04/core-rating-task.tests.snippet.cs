// Test code for core-rating-task.cs (m18-l04). Compiled together with the learner code.
using SecLog.Model;

public static class TaskTests
{
    private const string Phishing =
        "You won a prize! Click http://example.com/claim to get your reward today.";
    private const string Harmless =
        "The team meeting moves to 10:00 tomorrow. The agenda is in the shared folder.";

    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Rate("Message 3 is possible phishing", true, Phishing, true,
            "A plain http:// link and a signal word make it possible phishing."),
        Rate("Message 4 is harmless", true, Harmless, false,
            "Without a link and a signal word the message is harmless."),
        Throws<ArgumentNullException>("A null message throws", true,
            () => new MessageRating(["login"]).IsPossiblePhishing(null!)),
        Throws<ArgumentException>("An empty word list throws", false,
            () => new MessageRating([])),
        Throws<ArgumentException>("A blank word throws", false,
            () => new MessageRating(["login", " "])),
    ];

    private static (string, bool, bool, string) Rate(
        string name, bool visible, string message, bool expected, string hint)
    {
        try
        {
            bool actual = new MessageRating(MessageRating.DefaultSignalWords)
                .IsPossiblePhishing(message);
            return (name, visible, actual == expected, actual == expected ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static (string, bool, bool, string) Throws<TException>(
        string name, bool visible, Action action)
        where TException : Exception
    {
        try
        {
            action();
            return (name, visible, false, $"Expected {typeof(TException).Name}.");
        }
        catch (TException)
        {
            return (name, visible, true, "");
        }
        catch (Exception ex)
        {
            return (name, visible, false,
                $"Threw {ex.GetType().Name} instead of {typeof(TException).Name}.");
        }
    }
}
