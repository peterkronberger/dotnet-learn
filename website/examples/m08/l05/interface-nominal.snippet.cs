ILabeled item = new Scanner();

interface ILabeled
{
    string Label { get; }
}

class Scanner
{
    public string Label { get; } = "archive scanner";
}
