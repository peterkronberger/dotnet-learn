string? name = "ann";
Console.WriteLine(name ?? Fallback());

string Fallback()
{
    Console.WriteLine("Fallback called");
    return "none";
}
