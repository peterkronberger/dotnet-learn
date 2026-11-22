namespace SecLog.Model;

/// <summary>Rates a reported message as possible phishing (teaching rule).</summary>
public sealed class MessageRating
{
    private readonly string[] signalWords;

    /// <summary>The signal words that the console application uses.</summary>
    public static IReadOnlyList<string> DefaultSignalWords { get; } =
        ["password", "login", "urgent", "locked", "prize", "click", "verify"];

    /// <summary>Creates a rating with its own copy of the signal words.</summary>
    /// <param name="signalWords">The words that make a message suspicious.</param>
    /// <exception cref="ArgumentNullException"><paramref name="signalWords"/> is null.</exception>
    /// <exception cref="ArgumentException">The list is empty or contains a blank word.</exception>
    public MessageRating(IReadOnlyList<string> signalWords)
    {
        ArgumentNullException.ThrowIfNull(signalWords);
        if (signalWords.Count == 0)
        {
            throw new ArgumentException(
                "At least one signal word is needed.", nameof(signalWords));
        }
        foreach (string word in signalWords)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(word);
        }

        this.signalWords = [.. signalWords];
    }

    /// <summary>
    /// Checks for a plain http:// link and at least one signal word.
    /// </summary>
    /// <param name="message">The reported text. An empty text is harmless.</param>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
    public bool IsPossiblePhishing(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        bool hasLink = message.Contains("http://", StringComparison.Ordinal);
        return hasLink && signalWords.Any(word =>
            message.Contains(word, StringComparison.OrdinalIgnoreCase));
    }
}
