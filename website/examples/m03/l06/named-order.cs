Console.WriteLine(Pair(second: Next("B"), first: Next("A")));

string Next(string text)
{
    Console.WriteLine($"evaluate {text}");
    return text;
}

string Pair(string first, string second) => first + second;
