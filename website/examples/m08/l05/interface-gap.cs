// Task 2: change the type of this variable to the interface
Scanner item = new Scanner("archive scanner");
Console.WriteLine(item.Describe());

interface ILabeled
{
    string Label { get; }

    string Describe();
}

// Task 1: let Scanner implement ILabeled
class Scanner
{
    public string Label { get; }

    public Scanner(string label)
    {
        Label = label;
    }

    public string Describe() => $"Scanner: {Label}";
}
