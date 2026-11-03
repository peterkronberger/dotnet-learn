string? first = Console.ReadLine();
string? second = Console.ReadLine();

string firstText = first ?? "none";
string secondText = second ?? "none";

if (int.TryParse(firstText, out int count))
{
    Console.WriteLine($"Count: {count}");
}
else
{
    Console.WriteLine($"Not a number: {firstText}");
}

if (int.TryParse(secondText, out int other))
{
    Console.WriteLine($"Count: {other}");
}
else
{
    Console.WriteLine($"Not a number: {secondText}");
}
