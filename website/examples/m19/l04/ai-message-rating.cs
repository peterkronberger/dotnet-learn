string[] words =
    ["password", "login", "urgent", "locked", "prize", "click", "verify"];
string phishing =
    "Your account is locked. Confirm at http://example.com/verify now.";

var rating = new MessageRating(words);
Console.WriteLine($"Message: {rating.IsPossiblePhishing(phishing)}");

// The word list changes later, somewhere else in the program
Array.Fill(words, "none");
Console.WriteLine($"After the change: {rating.IsPossiblePhishing(phishing)}");

try
{
    rating.IsPossiblePhishing(null!);
}
catch (Exception ex)
{
    Console.WriteLine($"Missing text: {ex.GetType().Name}");
}

public sealed class MessageRating
{
    private readonly string[] signalWords;

    public MessageRating(string[] signalWords)
    {
        this.signalWords = signalWords;
    }

    public bool IsPossiblePhishing(string message)
    {
        bool hasLink = message.Contains("http://");
        return hasLink && signalWords.Any(word =>
            message.Contains(word, StringComparison.OrdinalIgnoreCase));
    }
}
