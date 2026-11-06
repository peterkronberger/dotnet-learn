Scanner scanner = new("archive scanner", 600);

Console.WriteLine($"Resolution: {scanner.Dpi} dpi");
// Task 3: print scanner.Name here

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }
}

// Task 1: derive Scanner from Device
class Scanner
{
    public int Dpi { get; }

    // Task 2: pass the name on to the base constructor
    public Scanner(string name, int dpi)
    {
        Dpi = dpi;
    }
}
