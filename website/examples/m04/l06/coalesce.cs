string? missing = null;
string? present = "alice";
string first = missing ?? Fallback();
string second = present ?? Fallback();
Console.WriteLine(first);
Console.WriteLine(second);

string Fallback()
{
    Console.WriteLine("Fallback called");
    return "unknown";
}
