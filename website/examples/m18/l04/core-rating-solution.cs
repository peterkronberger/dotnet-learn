using SecLog.Model;

// @fold-start
// Made-up reported messages of the sample data
string[] reportedMessages =
[
    "Your account is locked. Confirm your password now at http://example.com/verify or it will be deleted.",
    "Urgent: unusual sign-in. Open http://example.org/secure and enter your login data within 24 hours.",
    "You won a prize! Click http://example.com/claim to get your reward today.",
    "The team meeting moves to 10:00 tomorrow. The agenda is in the shared folder.",
    "Reminder: the quarterly report is due on Friday. Please send your part to the team lead.",
    "Thanks for your help with the server update. The maintenance window ended without problems.",
];
// @fold-end

// Rate the messages with the default signal words
var messageRating = new MessageRating(MessageRating.DefaultSignalWords);
for (int number = 1; number <= reportedMessages.Length; number++)
{
    bool phishing = messageRating.IsPossiblePhishing(reportedMessages[number - 1]);
    string rating = phishing ? "possible phishing" : "harmless";
    Console.WriteLine($"Message {number}: {rating}");
}

namespace SecLog.Model
{
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
}
