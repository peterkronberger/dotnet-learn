interface ILabeled
{
    string Label { get; }

    string Describe();
}

class Scanner : ILabeled
{
    public string Label { get; } = "archive scanner";
}
