bool ok = Enum.TryParse("Medium", out Severity found);
Console.WriteLine($"{ok}: {found}");

bool bad = Enum.TryParse("Critical", out Severity unknown);
Console.WriteLine($"{bad}: {unknown}");

try
{
    Severity parsed = Enum.Parse<Severity>("Critical");
    Console.WriteLine(parsed);
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

enum Severity { None, Low, Medium, High }
