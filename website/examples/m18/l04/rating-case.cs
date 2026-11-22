string[] signalWords = ["click", "password", "verify"];
string[] messages =
[
    "PASSWORD reset at http://example.org/reset",
    "Open HTTP://example.org/reset to verify your account",
    "Please verify the report before Friday",
];
for (int number = 1; number <= messages.Length; number++)
{
    Console.WriteLine($"Message {number}: {Rate(messages[number - 1])}");
}

// A plain http:// link and at least one signal word (teaching rule)
string Rate(string message)
{
    bool hasLink = message.Contains("http://", StringComparison.Ordinal);
    bool hasWord = signalWords.Any(word =>
        message.Contains(word, StringComparison.OrdinalIgnoreCase));
    return hasLink && hasWord ? "possible phishing" : "harmless";
}
