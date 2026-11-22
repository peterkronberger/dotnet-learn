// Task 1: reject null with ArgumentNullException.ThrowIfNull
// Task 2: reject an empty list and a blank word with an ArgumentException
// Task 3: store a copy of the words
// Expected: ArgumentNullException, ArgumentException, ArgumentException, ok: 2
string?[] tries = ["null", "empty", "blank", "valid"];
foreach (string? name in tries)
{
    IReadOnlyList<string>? input = name switch
    {
        "null" => null,
        "empty" => [],
        "blank" => ["login", " "],
        _ => ["login", "prize"],
    };
    try
    {
        SignalWords words = new(input!);
        Console.WriteLine($"ok: {words.Count}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.GetType().Name);
    }
}

class SignalWords
{
    private readonly string[] words;

    public SignalWords(IReadOnlyList<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);
        if (words.Count == 0)
        {
            throw new ArgumentException(
                "At least one signal word is needed.", nameof(words));
        }
        foreach (string word in words)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(word);
        }

        this.words = [.. words];
    }

    public int Count => words.Length;
}
