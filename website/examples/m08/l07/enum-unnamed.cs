Severity odd = (Severity)42;
Console.WriteLine(odd);
Console.WriteLine(Enum.IsDefined(odd));
Console.WriteLine(Enum.IsDefined(Severity.High));

switch (odd)
{
    case Severity.None:
    case Severity.Low:
        Console.WriteLine("quiet");
        break;
    case Severity.Medium:
    case Severity.High:
        Console.WriteLine("loud");
        break;
    default:
        Console.WriteLine("unknown value");
        break;
}

enum Severity { None, Low, Medium, High }
